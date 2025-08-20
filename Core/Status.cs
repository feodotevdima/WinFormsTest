using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
