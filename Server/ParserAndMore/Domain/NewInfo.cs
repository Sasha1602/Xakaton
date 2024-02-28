using System.ComponentModel.DataAnnotations;

namespace Domain;

public class NewInfo
{
    [Key]
    public Guid Id { get; set; }

    public string? Body { get; set; }

    public string? Title { get; set; }

    public DateTime CreationTime { get; set; }

    public NewInfo()
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
    }

    public NewInfo RedactInfo(string body, string title)
    {
        this.Body = body;
        this.Title = title;
        
        return this;
    }
}