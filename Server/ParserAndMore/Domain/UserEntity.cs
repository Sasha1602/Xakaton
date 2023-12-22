using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserEntity
{
    [Key]
    public string Id { get; set; }
    
    public string Password { get; set; }
    public string Login { get; set; }
    
}