using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class MyDbContext : DbContext //ToDo: migrate to MySql database
{
    public DbSet<UserEntity> Users = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=123456789;database=usersdb;", 
            // ToDo: download MySQL Connector for Dotnet and change MySqlServerVersion() func    
            new MySqlServerVersion(new Version(8, 0, 25))); 
         
    }
}