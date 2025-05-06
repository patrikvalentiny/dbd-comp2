using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Domain.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    
        
        [Column("profile_pic_url")]
        public string? ProfilePicUrl { get; set; }
    }
}
