using Domain;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DAL;

public class MyDbContext : DbContext //ToDo: migrate to MySql database
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<ImageEntity> Images { get; set; }

    public DbSet<NewInfo> News { get; set; }

    public DbSet<ResponseEntity> Responses = null!; //for history of requests realization

    public DbSet<RequestEntity> Requsets = null!;

    static string connectionString = "server=localhost;port=3306;database=Neuroprint;uid=root;password=1234";
    MySqlConnection connection = new MySqlConnection(connectionString);
    
     protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        try
        {
            connection.Open();
            Console.WriteLine("Соединение с MySQL установлено!");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Ошибка подключения: " + ex.Message);
        }

    }



}