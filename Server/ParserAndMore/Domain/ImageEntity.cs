using System.ComponentModel.DataAnnotations;

namespace Domain;

public class ImageEntity
{
    [Key]
    public int ImageId { get; set; }
    
    public string ImagePath { get; set; }
    public string ClotheType { get; set; }
    public string Color { get; set; }
    public string Tone { get; set; }
    
}