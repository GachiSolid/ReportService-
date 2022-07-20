namespace ReportingService.Token
{
    public interface IJwtGenerator
    {
        string BuildToken(string login);
        bool ValidateToken(string token);
    }
}
