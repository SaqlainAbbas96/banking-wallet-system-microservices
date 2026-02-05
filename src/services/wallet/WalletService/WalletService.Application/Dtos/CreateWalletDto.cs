namespace WalletService.Application.Dtos
{
    public record CreateWalletDto(
        Guid UserId,
        string Currency);
}
