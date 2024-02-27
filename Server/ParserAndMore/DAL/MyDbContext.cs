using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class MyDbContext : DbContext //ToDo: migrate to MySql database
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<ImageEntity> Images { get; set; } = null!;

    public DbSet<NewInfo> News { get; set; } = null!;

    public DbSet<ResponseEntity> Responses { get; set; } = null!; //for history of requests realization

    public DbSet<RequestEntity> Requsets { get; set; } = null!;

    public DbSet<UserEntityImages> UserImages { get; set; } = null!;

    public DbSet<UserRequestEntity> UserRequests { get; set; } = null;


    protected override void OnConfiguring(DbContextOptionsBuilder builder)
     {
         builder.UseMySql("server=localhost;user=root;password=1234;database=neuroprint;", // change connectionstring to yours
             new MySqlServerVersion(new Version(8, 0, 25))); // select current version of mysql
     }



}