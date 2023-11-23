using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class MyDbContext : DbContext //ToDo: migrate to MySql database
{
    public DbSet<UserEntity> Users = null!;
}