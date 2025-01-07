using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

[Table("Product", Schema = "SalesLT")]
public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty!;

    [Required]
    public string ProductNumber { get; set; } = string.Empty!;

    public string? Color { get; set; } = string.Empty!;

    [Required]
    [Column(TypeName = "money")]
    public decimal StandardCost { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal ListPrice { get; set; }

    public string? Size { get; set; } = string.Empty!;

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Weight { get; set; } = null;

    public int? ProductCategoryID { get; set; } = null;

    public int? ProductModelID { get; set; } = null;

    [Required]
    public DateTime SellStartDate { get; set; }

    public DateTime? SellEndDate { get; set; } = null;

    public DateTime? DiscontinuedDate { get; set; } = null;

    public byte[] ThumbNailPhoto {get ;set;} = Array.Empty<byte>();

    public string ThumbnailPhotoFileName { get; set; } = string.Empty!;

    [Required]
    public Guid rowguid { get; set; }

    [Required]
    public DateTime ModifiedDate { get; set; }
}
