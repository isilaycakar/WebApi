using Microsoft.EntityFrameworkCore;

namespace _01_EFCore.Entities
{
    public class ToDoDbContext: DbContext
    {
        public ToDoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder.IsConfigured == false)
        //    {
        //        optionsBuilder.UseSqlServer("server=ISILAY; database=ToDoApi; Trusted_Connection=true; TrustedServerCertificate=true;");
        //    }
        //}
    }
}
