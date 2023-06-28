using Microsoft.EntityFrameworkCore;

namespace SimpleAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}