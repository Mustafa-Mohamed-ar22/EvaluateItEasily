using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials = new(
            "Auth.InvalidCredentials",
            "Invalid email or password",
            StatusCodes.Status401Unauthorized);

        public static readonly Error EmailAlreadyExists = new(
            "Auth.EmailAlreadyExists",
            "This email is already registered",
            StatusCodes.Status409Conflict);

        public static readonly Error InvalidToken = new(
            "Auth.InvalidToken",
            "Refresh token is invalid or expired",
            StatusCodes.Status401Unauthorized);

        public static readonly Error UserNotFound = new(
            "Auth.UserNotFound",
            "User was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error InactiveUser = new(
            "Auth.InactiveUser",
            "This account has been deactivated",
            StatusCodes.Status401Unauthorized);
        public static readonly Error CreationFailed = new Error("Auth.CreationFailed",
            "Not Complteted Operration or, Username or password is not strong", StatusCodes.Status500InternalServerError);
    }
}
