using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    [NotMapped]
    public class StatisticsItem
    {
        public DateTime StatDate { get; set; }
        public int EmployeeCount { get; set; }
    }
}
