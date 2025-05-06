using KiderWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace KiderWebApplication.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<PopularTeacher> PopularTeachers { get; set; }
    }
}
