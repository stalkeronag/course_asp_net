using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace WebApi.Models
{
    public class RefreshToken
    {  
        public string Token { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get ; set; }

        [Required]
        [ForeignKey("RefreshTokenSessionConnectionId")]
        public RefreshTokenSessionConnection Connection { get; set; }
    }
}
