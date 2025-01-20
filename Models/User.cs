using System.ComponentModel.DataAnnotations;

namespace SquashClubAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]        
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Range (0, 1000000)]
        public decimal AccountBalance { get; set; }
        
    }
}
