using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class RefreshTokenSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public List<RefreshTokenSessionConnection> Connections { get; set; }

        [Required]
        public User User { get; set; }

        public string UserId { get; set; }
    }
}
