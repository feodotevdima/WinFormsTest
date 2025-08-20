using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Dtos
{
    public class EmployeeFilter
    {
        public int? StatusId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PostId { get; set; }
        public string LastNameFilter { get; set; }
    }
}
