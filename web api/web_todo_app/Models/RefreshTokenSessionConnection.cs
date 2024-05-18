using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class RefreshTokenSessionConnection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public RefreshTokenSession RefreshTokenSession { get; set; }
        
        public RefreshToken RefreshToken {  get; set; }

        public FingerPrint FingerPrint { get; set; }
        public string IpAddress { get; set; } 

        public bool Equals(RefreshTokenSessionConnection other)
        {
            return other.RefreshToken.Token == RefreshToken.Token;
        }
    }
}
