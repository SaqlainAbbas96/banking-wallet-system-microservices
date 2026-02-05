using WalletService.Application.Common;
using WalletService.Application.Dtos;
using WalletService.Application.Interfaces;
using WalletService.Domain.Entities;

namespace WalletService.Application.UseCases
{
    public class CreateWalletUseCase
    {
        private readonly IWalletRepository _walletRepository;
        public CreateWalletUseCase(IWalletRepository walletRepository) 
        {
            _walletRepository = walletRepository;
        }

        public async Task<Result<WalletResponse>> ExecuteAsync(
            CreateWalletDto dto,
            CancellationToken ct) 
        {
            if (dto.UserId == Guid.Empty)
                return Result<WalletResponse>.Failure(
                    "UserId cannot be empty.");

            var existing = await _walletRepository.GetByUserIdAsync(dto.UserId, ct);
            if (existing != null)
                return Result<WalletResponse>.Failure(
                    "Wallet already exists.");

            var wallet = new Wallet(dto.UserId, dto.Currency);

            await _walletRepository.AddAsync(wallet, ct);
            await _walletRepository.SaveAsync(ct);

            return Result<WalletResponse>.Success(new WalletResponse
            {
                WalletId = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                Currency = wallet.Currency
            });
        }
    }
}
