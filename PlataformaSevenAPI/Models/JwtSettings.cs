namespace PlataformaSeven.API.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; } = 480; // 8 horas
        public int RefreshTokenExpirationInDays { get; set; } = 7;
    }
}

