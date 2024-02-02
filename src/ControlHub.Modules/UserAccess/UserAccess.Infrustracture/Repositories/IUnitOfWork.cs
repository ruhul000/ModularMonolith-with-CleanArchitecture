namespace UserAccess.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        Task<bool> SaveAsync();
    }
}
