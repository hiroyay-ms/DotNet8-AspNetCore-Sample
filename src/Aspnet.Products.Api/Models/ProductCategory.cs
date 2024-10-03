using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspnet.Products.Api.Models;

[Table("ProductCategory", Schema = "SalesLT")]
public class ProductCategory
{
    [Key]
    [Column("ProductCategoryID")]
    public int ProductCategoryId { get; set; }

    [Column("ParentProductCategoryID")]
    public int? ParentProductCategoryId { get; set; }

    [Required]
    [Column("Name")]
    public string? CategoryName { get; set; }

    [Required]
    [Column("rowguid")]
    public Guid RowGuid { get; set; }

    [Required]
    [Column("ModifiedDate")]
    public DateTime ModifiedDate { get; set; } 
}