using WalletService.Application.Common;
using WalletService.Application.Dtos;
using WalletService.Application.Interfaces;

namespace WalletService.Application.UseCases
{
    public sealed class GetWalletUseCase
    {
        private readonly IWalletRepository _walletRepository;
        public GetWalletUseCase(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Result<WalletResponse>> ExecuteAsync(
            GetWalletDto dto,
            CancellationToken ct)
        {
            if (dto.UserId == Guid.Empty)
                return Result<WalletResponse>.Failure(
                    "UserId is empty.");

            var wallet = await _walletRepository.GetByUserIdAsync(dto.UserId, ct);

            if (wallet == null)
                return Result<WalletResponse>.Failure(
                    "No wallet exists for this user.");

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
