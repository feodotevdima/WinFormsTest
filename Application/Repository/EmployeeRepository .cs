using Application.Interfaces;
using Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Dtos;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetEmployeesAsync(EmployeeFilter filter, string sortField = "FullName", string sortOrder = "ASC")
    {
        var parameters = new[]
        {
            new SqlParameter("@StatusFilter", filter.StatusId ?? (object)DBNull.Value),
            new SqlParameter("@DepartmentFilter", filter.DepartmentId ?? (object)DBNull.Value),
            new SqlParameter("@PostFilter", filter.PostId ?? (object)DBNull.Value),
            new SqlParameter("@LastNameFilter", string.IsNullOrEmpty(filter.LastNameFilter) ? (object)DBNull.Value : filter.LastNameFilter),
            new SqlParameter("@SortField", sortField),
            new SqlParameter("@SortOrder", sortOrder)
        };

        return await _context.Persons
            .FromSqlRaw("EXEC GetEmployees @StatusFilter, @DepartmentFilter, @PostFilter, @LastNameFilter, @SortField, @SortOrder", parameters)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StatisticsItem>> GetStatisticsAsync(int statusId, DateTime startDate, DateTime endDate, string statType)
    {
        var parameters = new[]
        {
            new SqlParameter("@StatusId", statusId),
            new SqlParameter("@StartDate", startDate),
            new SqlParameter("@EndDate", endDate),
            new SqlParameter("@StatType", statType)
        };

        return await _context.Set<StatisticsItem>()
            .FromSqlRaw("EXEC GetEmployeeStatistics @StatusId, @StartDate, @EndDate, @StatType", parameters)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Status>> GetStatusesAsync()
    {
        return await _context.Statuses
            .FromSqlRaw("EXEC GetStatuses")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments
            .FromSqlRaw("EXEC GetDepartments")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        return await _context.Posts
            .FromSqlRaw("EXEC GetPosts")
            .AsNoTracking()
            .ToListAsync();
    }
}
