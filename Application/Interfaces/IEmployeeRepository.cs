using Core;
using Persistence.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Person>> GetEmployeesAsync(EmployeeFilter filter, string sortField = "FullName", string sortOrder = "ASC");
        Task<List<StatisticsItem>> GetStatisticsAsync(int statusId, DateTime startDate, DateTime endDate, string statType);
        Task<List<Status>> GetStatusesAsync();
        Task<List<Department>> GetDepartmentsAsync();
        Task<List<Post>> GetPostsAsync();
    }
}
