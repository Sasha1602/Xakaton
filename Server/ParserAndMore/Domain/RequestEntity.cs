using System.ComponentModel.DataAnnotations;

namespace Domain;

public class RequestEntity
{ 
    [Key]
    public Guid Id;

    public string requestBodyText { get; set; }

    public DateTime creationDate { get; set; }

    public ImageEntity? Photo { get; set; }
}