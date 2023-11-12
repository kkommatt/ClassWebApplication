namespace ClassWebApplication.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetAccessTokenAsync();
    }
}
