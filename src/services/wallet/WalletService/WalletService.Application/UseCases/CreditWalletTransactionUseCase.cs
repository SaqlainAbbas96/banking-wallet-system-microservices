using WalletService.Application.Common;
using WalletService.Application.Dtos;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;
using WalletService.Domain.Exceptions;

namespace WalletService.Application.UseCases
{
    public sealed class CreditWalletTransactionUseCase
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _transactionRepository;

        public CreditWalletTransactionUseCase(
            IWalletRepository walletRepository,
            IWalletTransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Result<WalletTransactionResponse>> ExecuteAsync(
            CreditWalletTransactionDto dto,
            CancellationToken ct)
        {
            if (dto.UserId == Guid.Empty)
                return Result<WalletTransactionResponse>.Failure(
                    "Invalid user");

            if (dto.WalletId == Guid.Empty)
                return Result<WalletTransactionResponse>.Failure(
                    "Invalid wallet");

            if (string.IsNullOrWhiteSpace(dto.IdempotencyKey))
                return Result<WalletTransactionResponse>.Failure(
                    "IdempotencyKey is required");

            if (dto.Type != WalletTransactionType.Credit)
                return Result<WalletTransactionResponse>.Failure(
                    "Invalid transaction type");

            var wallet = await _walletRepository.GetByIdAsync(dto.WalletId, ct);
            if (wallet == null)
                return Result<WalletTransactionResponse>.Failure(
                    "Wallet not found");

            if (wallet.UserId != dto.UserId)
                return Result<WalletTransactionResponse>.Failure(
                    "Wallet does not belong to user");

            var existingTx = await _transactionRepository
                .GetByIdempotencyKeyAsync(wallet.Id, dto.IdempotencyKey, ct);

            if (existingTx != null)
            {
                return Result<WalletTransactionResponse>.Success(new WalletTransactionResponse
                {
                    WalletId = wallet.Id,
                    TransactionId = existingTx.Id,
                    Balance = wallet.Balance,
                    Type = Enum.Parse<WalletTransactionType>(existingTx.Type)
                });
            }

            try
            {
                wallet.ApplyCredit(dto.Amount);
            }
            catch (DomainException ex)
            {
                return Result<WalletTransactionResponse>.Failure(ex.Message);
            }

            var transaction = WalletTransaction.Create(
                wallet.Id,
                WalletTransactionType.Credit,
                dto.Amount,
                dto.IdempotencyKey);

            await _transactionRepository.AddAsync(transaction, ct);
            await _walletRepository.SaveAsync(ct);

            return Result<WalletTransactionResponse>.Success(new WalletTransactionResponse
            {
                WalletId = wallet.Id,
                TransactionId = transaction.Id,
                Balance = wallet.Balance,
                Type = WalletTransactionType.Credit
            });
        }
    }
}
