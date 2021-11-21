namespace HotTowel.Web.Controllers
{
    public class ContactViewModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}