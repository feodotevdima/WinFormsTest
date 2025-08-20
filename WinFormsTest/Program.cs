using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System.Configuration;

namespace WinFormsTest
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var Form1 = serviceProvider.GetRequiredService<Form1>();
            System.Windows.Forms.Application.Run(Form1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<Form1>();
        }
    }
}