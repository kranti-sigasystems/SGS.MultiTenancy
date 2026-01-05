namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Application constants.
    /// </summary>
    public static class Constants
    {
        #region Field Display Name

        /// <summary>
        /// Gets the business name display label.
        /// </summary>
        public const string BussinessName = "Business Name";

        /// <summary>
        /// Gets the phone number display label.
        /// </summary>
        public const string PhoneNumber = "Phone Number";

        #endregion

        #region Display Error Messages

        /// <summary>
        /// Gets the maximum length validation error message.
        /// </summary>
        public const string MaxErrorMessage = "{0} cannot exceed {1} characters";

        /// <summary>
        /// Gets the required field validation error message.
        /// </summary>
        public const string RequiredErrorMessage = "{0} is required field";

        /// <summary>
        /// Gets the invalid email address validation error message.
        /// </summary>
        public const string EmailErrorMessage = "Please enter a valid email address";

        #endregion
    }
}
