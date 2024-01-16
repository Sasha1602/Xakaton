using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class MyDbContext : DbContext //ToDo: migrate to MySql database
{
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ImageEntity> Images { get; set; }

    public DbSet<NewInfo> News { get; set; }

    public DbSet<ResponseEntity> Responses = null!; //for history of requests realization

    public DbSet<RequestEntity> Requsets = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Neiroprint;UserName=postgres;Password=1234;");

        /*("server=localhost;user=root;password=123456789;database=usersdb;",
        // ToDo: download MySQL Connector for Dotnet and change MySqlServerVersion() func
        new MySqlServerVersion(new Version(8, 0, 25)));*/
        //This for MySql

    }
}