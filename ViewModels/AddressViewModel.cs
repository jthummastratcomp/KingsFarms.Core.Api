namespace HotTowel.Web.ViewModels
{
    public class AddressViewModel
    {
        public string Display => $"{FirstLineDisplay} {LastLineDisplay}";
        public string? FirstLineDisplay => Street;
        public string LastLineDisplay => $"{City}, {State} {Zip}";

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}