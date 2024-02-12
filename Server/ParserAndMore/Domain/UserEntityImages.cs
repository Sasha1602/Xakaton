using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserEntityImages
{
    [Key]
    public string Id { get; set; }
    public string? UserId { get; set; }
    public string? ImageId { get; set; }
}