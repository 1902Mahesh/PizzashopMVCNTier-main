namespace BusinessLogicLayer.Common;
public static class NotificationMessages
{
    
    public const string InvalidCredentials = "Invalid credentials. Please try again.";
    public const string LoginSuccess = "You have successfully logged in.";
    public const string EntityUpdated = "{0} has been updated successfully!";
    public const string EntityCreated = "{0} has been added successfully!";
    public const string EntityDeleted = "{0} has been deleted successfully!";
    public const string ProfileUpdated = "Your profile has been updated successfully!";
    public const string EmailSentSuccessfully = "Email has been sent successfully!";
    public const string PasswordChanged = "Your password has been changed successfully.";


    // Error Messages
    public const string EntityUpdatedFailed = "Failed Updating {0}";
    public const string EntityCreatedFailed = "Failed Adding {0}";
    public const string EntityDeletedFailed = "Failed Deleting {0}";
    public const string EmailSendingFailed = "Failed to send the email. Please try again.";
    public const string PasswordChangeFailed = "Failed to change the password. Please try again.";
    public const string InvalidModelState = "Model State Is Invalid!";

}
