using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserRequestEntity
{
    [Key]
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public string? RequestId { get; set; }
}