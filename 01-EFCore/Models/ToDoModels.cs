using System.ComponentModel.DataAnnotations;

namespace _01_EFCore.Models
{
    public class ToDoResponse
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string IsCompletedStr
        {
            get
            {
                return IsCompleted ? "Tamamlandı" : "Tamamlanmadı";
            }
        }
    }

    public class ToDoCreateModel
    {
        [Required]
        [StringLength(200)]
        public string Header { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }


    }

    public class ToDoUpdateModel
    {

        //public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Header { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
