using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserEntityImages
{
    [Key]
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ImageId { get; set; }
}