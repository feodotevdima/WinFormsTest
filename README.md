docker-compose up - запуск бд.

хранимые процедуры:

CREATE PROCEDURE GetEmployees
    @StatusFilter INT = NULL,
    @DepartmentFilter INT = NULL,
    @PostFilter INT = NULL,
    @LastNameFilter VARCHAR(100) = NULL,
    @SortField VARCHAR(50) = 'last_name',
    @SortOrder VARCHAR(4) = 'ASC'
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = '
        SELECT 
            p.id,
            p.last_name + '' '' + LEFT(p.first_name, 1) + ''. '' + LEFT(p.second_name, 1) + ''.'' as FullName,
            s.name as StatusName,
            d.name as DepartmentName,
            po.name as PostName,
            p.date_employ as DateEmploy,
            p.date_uneploy as DateUnemploy
        FROM persons p
        INNER JOIN status s ON p.status = s.id
        INNER JOIN deps d ON p.id_dep = d.id
        INNER JOIN posts po ON p.id_post = po.id
        WHERE 1=1'
    
    IF @StatusFilter IS NOT NULL
        SET @SQL = @SQL + ' AND p.status = ' + CAST(@StatusFilter AS VARCHAR(10))
    
    IF @DepartmentFilter IS NOT NULL
        SET @SQL = @SQL + ' AND p.id_dep = ' + CAST(@DepartmentFilter AS VARCHAR(10))
    
    IF @PostFilter IS NOT NULL
        SET @SQL = @SQL + ' AND p.id_post = ' + CAST(@PostFilter AS VARCHAR(10))
    
    IF @LastNameFilter IS NOT NULL
        SET @SQL = @SQL + ' AND p.last_name LIKE ''%' + @LastNameFilter + '%'''
    
    SET @SQL = @SQL + ' ORDER BY ' + @SortField + ' ' + @SortOrder
    
    EXEC sp_executesql @SQL
END
GO

CREATE PROCEDURE GetEmployeeStatistics
    @StatusId INT,
    @StartDate DATETIME,
    @EndDate DATETIME,
    @StatType VARCHAR(10) 
AS
BEGIN
    IF @StatType = 'employ'
    BEGIN
        SELECT 
            CAST(date_employ AS DATE) as StatDate,
            COUNT(*) as EmployeeCount
        FROM persons
        WHERE status = @StatusId
            AND date_employ BETWEEN @StartDate AND @EndDate
        GROUP BY CAST(date_employ AS DATE)
        ORDER BY StatDate
    END
    ELSE
    BEGIN
        SELECT 
            CAST(date_uneploy AS DATE) as StatDate,
            COUNT(*) as EmployeeCount
        FROM persons
        WHERE status = @StatusId
            AND date_uneploy BETWEEN @StartDate AND @EndDate
        GROUP BY CAST(date_uneploy AS DATE)
        ORDER BY StatDate
    END
END
GO

CREATE PROCEDURE GetStatuses
AS
BEGIN
    SELECT id, name FROM status ORDER BY name
END
GO

CREATE PROCEDURE GetDepartments
AS
BEGIN
    SELECT id, name FROM deps ORDER BY name
END
GO

CREATE PROCEDURE GetPosts
AS
BEGIN
    SELECT id, name FROM posts ORDER BY name
END
GO
