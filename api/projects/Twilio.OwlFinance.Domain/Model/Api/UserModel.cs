namespace Twilio.OwlFinance.Domain.Model.Api
{
    public abstract class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
    }
}
