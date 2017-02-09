namespace Twilio.OwlFinance.Domain.Model
{
    public static class StatusCodes
    {
        public static readonly int ServerError = 500;
        public static readonly int UnprocessableEntity = 422;
        public static readonly int Conflict = 409;
        public static readonly int ItemNotFound = 404;
        public static readonly int NotAuthorized = 403;
        public static readonly int NotAuthenticated = 401;
        public static readonly int BadRequest = 400;
    }
}
