namespace Twilio.OwlFinance.Domain.Model.Api.Admin
{
    public class UserModel
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserType Type { get; set; }

        public SimpleUserModel LinkedUser { get; set; }

        public string IdentityID { get; set; }

        public string PhoneNumber { get; set; }

        public string WorkerSid { get; set; }

        public string Name => $"{FirstName} {LastName}";
    }
}
