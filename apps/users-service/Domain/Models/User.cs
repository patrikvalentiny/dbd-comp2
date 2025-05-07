using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Domain.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("username", TypeName = "varchar(255)")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; } = string.Empty;
    
        
        [Column("avatar_url")]
        public string? ProfilePicUrl { get; set; }
    }
}
