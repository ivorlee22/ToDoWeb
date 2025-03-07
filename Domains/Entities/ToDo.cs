using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Domains.Entities
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
