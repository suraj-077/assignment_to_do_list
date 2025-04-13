using System.ComponentModel.DataAnnotations;

namespace assignment_to_do_list.Model
{
    public class CreateTaskDto
    {

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}

