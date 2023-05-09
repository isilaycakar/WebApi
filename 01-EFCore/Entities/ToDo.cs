using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _01_EFCore.Entities
{
    [Table("ToDos")]
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Header { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
