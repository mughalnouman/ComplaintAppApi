using Microsoft.EntityFrameworkCore;

namespace ComplaintApi.Model
{
    public class ComplaintdbContext : DbContext
    {
        public ComplaintdbContext(DbContextOptions<ComplaintdbContext> options)
           : base(options)
        {
        }
        public DbSet<userModel> userTable { get; set; }
    }
}
