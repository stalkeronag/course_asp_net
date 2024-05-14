using System.ComponentModel.DataAnnotations.Schema;

namespace web_todo_app.Models
{
    public class TaskModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}
