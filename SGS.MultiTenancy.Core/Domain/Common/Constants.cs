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

        /// <summary>
        /// Gets the address line display label.
        /// </summary>
        public const string AddressLine = "Address Line";

        /// <summary>
        /// Gets the postal code display label.
        /// </summary>
        public const string PostalCode = "Postal Code";

        /// <summary>
        /// Gets the tenant identifier const..
        /// </summary>
        public const string TenantID = "TenantID";

        #endregion

        #region Role Names

        /// <summary>
        /// Role identifier for a super administrator.
        /// </summary>
        public const string SuperAdminHost = "SGS_SuperHost";

        /// <summary>
        /// Role identifier for a tenant administrator.
        /// </summary>
        public const string TenantHost = "SGS_TenantHost";

        #endregion
        #region Display Error Messages

        /// <summary>
        /// Gets the maximum length validation error message.
        /// </summary>
        public const string MaxErrorMessage = "{0} cannot exceed {1} characters";

        /// <summary>
        /// Gets the required field validation error message.
        /// </summary>
        public const string RequiredErrorMessage = "{0} is required";

        /// <summary>
        /// Gets the invalid email address validation error message.
        /// </summary>
        public const string EmailErrorMessage = "Please enter a valid email address";

        /// <summary>
        /// Gets the minimum length validation error message.
        /// </summary>
        public const string MinErrorMessage = "The {0} field must be at least {1} characters.";
        
        /// <summary>
        /// Authentication failed.
        /// </summary>
        public const string InvalidLogin = "Invalid username or password.";
        
        /// <summary>
        /// Password changed successfully message.
        /// </summary>
        public const string PasswordChangedSuccess = "Password changed successfully. Please login again.";

        /// <summary>
        /// User not found 
        /// </summary>
        public const string UserNotFound = "User not found.";

        /// <summary>
        /// Password incorrect message.
        /// </summary>
        public const string CurrentPasswordIncorrect = "Current password is incorrect.";

        /// <summary>
        /// Reads user name field.
        /// </summary>
        public const string UserNameDisplay = "User Name";

        /// <summary>
        /// Password match error.
        /// </summary>
        public const string PasswordsDoNotMatch = "Passwords do not match";

        ///  <summary>  
        ///  Minimum password length requirement.
        /// </summary>
        public const int PasswordMinLength = 8;

        /// <summary>
        /// Gets the new password display label.
        /// </summary>
        public const string NewPasswordDisplay = "New Password";

        /// <summary>
        /// Gets the old password display label.
        /// </summary>
        public const string CurrentPasswordDisplay = "Current Password";

        /// <summary>
        ///  Gets the confirm password display label.
        /// </summary>
        public const string ConfirmPasswordDisplay = "Confirm Password";

        /// <summary>
        /// Regex to check password strength requires one uppercase and one special char.
        /// </summary>
        public const string PasswordStrengthRegex =@"^(?=.*[A-Z])(?=.*[\W_]).+$";

        /// <summary>
        /// Error message when password fails strength check.
        /// </summary>
        public const string PasswordStrengthErrorMessage ="Password must contain at least one uppercase letter and one special character.";
        #endregion

        /// <summary>
        /// The default table page number.
        /// </summary>
        public static readonly uint? DefaultPageNumber = 1;

        /// <summary>
        /// The default table page size.
        /// </summary>
        public static readonly uint? DefaultPageSize = 10;

        public static readonly string BrandName="SGS";

        public const string PasswordResetLinkSent = "If an account exists with this email, password reset instructions have been sent.";

    }
}