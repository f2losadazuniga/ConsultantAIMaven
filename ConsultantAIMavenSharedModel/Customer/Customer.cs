using System.ComponentModel.DataAnnotations;


namespace EntregasLogyTechSharedModel.Customer
{
    public class Customer
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        [StringLength(100, ErrorMessage = "The Email field should not exceed 100 characters.")]
        public string Email { get; set; }
        [StringLength(150, ErrorMessage = "The Name field should not exceed 150 characters.")]
        public string Name { get; set; }
        public string PhotoProfile { get; set; }
        [StringLength(200, ErrorMessage = "The CountryLocation field should not exceed 200 characters.")]
        public string CountryLocation { get; set; }
        [StringLength(50, ErrorMessage = "The IdUser field should not exceed 50 characters.")]
        public string UserId { get; set; }

        public Customer()
        {
            Email = string.Empty;
        }
    }
}
