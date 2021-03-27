

using CommonLayer.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer
{
    public class DataContext : DbContext
    {
       public DataContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = FunDooUserAppDB; User ID = root; Password = root; ");
        }
        public DbSet<User> Users { get; set; }
    }
}
