using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class FingerPrint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserAgent { get; set; }

        public string Referer { get; set; }

        public string Hash { get; set; }

        [Required]
        [ForeignKey("RefreshTokenSessionConnectionId")]
        public RefreshTokenSessionConnection Connection { get; set; }
    }
}
