namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class JwtOptions
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or set the audience.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or set the secret.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or set the expiry minutes.
        /// </summary>
        public int ExpiryMinutes { get; set; }
    }
}
