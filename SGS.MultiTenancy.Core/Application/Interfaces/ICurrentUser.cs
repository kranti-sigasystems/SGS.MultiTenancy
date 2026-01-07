namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Represents the current authenticated user context.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Gets the current user's identifier when available.
        /// </summary>
        Guid? UserId { get; }
    }
}
