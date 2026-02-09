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
        public const string PasswordStrengthRegex = @"^(?=.*[A-Z])(?=.*[\W_]).+$";

        /// <summary>
        /// Error message when password fails strength check.
        /// </summary>
        public const string PasswordStrengthErrorMessage = "Password must contain at least one uppercase letter and one special character.";
        #endregion

        /// <summary>
        /// Error message when slug is invalid.
        /// </summary>
        public const string SlugInvalid = "Slug must contain only lowercase letters, numbers, and hyphens.";

        /// <summary>
        /// Error message when logo url is invalid.
        /// </summary>
        public const string InvalidLogoUrl = "Invalid logo URL format.";

        /// <summary>
        /// The default table page number.
        /// </summary>
        public static readonly uint? DefaultPageNumber = 1;

        /// <summary>
        /// The default table page size.
        /// </summary>
        public static readonly uint? DefaultPageSize = 10;

        /// <summary>
        /// Brand display name.
        /// </summary>
        public static readonly string BrandName = "SGS";

        /// <summary>
        /// Password reset link sent message.
        /// </summary>
        public const string PasswordResetLinkSent = "If an account exists with this email, password reset instructions have been sent.";

        /// <summary>
        /// Delete confirmation message.
        /// </summary>
        public const string DeleteQuestion = "Are you sure you want to delete";

        /// <summary>
        /// Confirmation pop up message.
        /// </summary>
        public const string ConfirmDeleteTitle = "Confirm Delete";

        /// <summary>
        /// Soft delete hint message. 
        /// </summary>
        public const string SoftDeleteHint = "This will perform a soft delete.";

        /// <summary>
        /// Gets the yes action button label.
        /// </summary>
        public const string Yes = "Yes";

        /// <summary>
        /// Gets the cancel action button label.
        /// </summary>
        public const string Cancel = "Cancel";

        /// <summary>
        /// Subdomain error message .
        /// </summary>
        public const string SubDomainError = "Subdomain is required";

        /// <summary>
        /// Default tenant role id.
        /// </summary>
        public const string TenantRoleId = "30dec8bc-2b22-4b3b-b721-8eb28c5d39c9";

        /// <summary>
        /// Image size 3mb.
        /// </summary>
        public const long MaxImageSize = 3 * 1024 * 1024;

        /// <summary>
        /// Image size error message.
        /// </summary>
        public const string ImageSizeErrorMessage = "Image size must be 3 MB or less.";

        /// <summary>
        /// Image format error message.
        /// </summary>
        public const string ImageFormatErrorMessage = "Only image files are allowed.";
    }
}