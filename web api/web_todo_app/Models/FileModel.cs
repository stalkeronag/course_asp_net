using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_todo_app.Models
{
    public class FileModel
    {
        public string FileName { get; set; }

        public string RelativePath { get; set; }

        public string GeneratedName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    }
}
