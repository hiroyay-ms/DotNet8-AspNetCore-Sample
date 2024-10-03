using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspnet.Products.Api.Models;

[Table("Product", Schema = "SalesLT")]
public class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [Required]
    [Column("Name")]
    public string? ProductName { get; set; }

    [Required]
    [Column("ProductNumber")]
    public string? ProductNumber { get; set; }

    [Column("Color")]
    public string? Color { get; set; }

    [Required]
    [Column("StandardCost")]
    public decimal StandardCost { get; set; }

    [Required]
    [Column("ListPrice")]
    public decimal ListPrice { get; set; }

    [Column("Size")]
    public string? Size { get; set; }

    [Column("Weight")]
    public decimal? Weight { get; set; }

    [Column("ProductCategoryID")]
    public int? ProductCategoryId { get; set; }

    [Column("ProductModelID")]
    public int? ProductModelId { get; set; }

    [Required]
    [Column("SellStartDate")]
    public DateTime SellStartDate { get; set; }

    [Column("SellEndDate")]
    public DateTime? SellEndDate { get; set; }

    [Column("DiscontinuedDate")]
    public DateTime? DiscontinuedDate { get; set; }

    [Column("ThumbnailPhoto")]
    public byte[]? ThumbnailPhoto { get; set; }

    [Column("ThumbnailPhotoFileName")]
    public string? ThumbnailPhotoFileName { get; set; }

    [Required]
    [Column("rowguid")]
    public Guid Rowguid { get; set; }

    [Required]
    [Column("ModifiedDate")]
    public DateTime ModifiedDate { get; set; }
}
