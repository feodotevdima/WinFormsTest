using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    [NotMapped]
    public class Person
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string StatusName { get; set; }

        public string DepartmentName { get; set; }

        public string PostName { get; set; }

        public DateTime? DateEmploy { get; set; }
        public DateTime? DateUnemploy { get; set; }
    }
}
