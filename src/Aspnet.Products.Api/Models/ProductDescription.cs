using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspnet.Products.Api.Models;

[Table("ProductDescription", Schema = "SalesLT")]
public class ProductDescription
{
    [Key]
    [Column("ProductDescriptionID")]
    public int ProductDescriptionId { get; set; }

    [Required]
    [Column("Description")]
    public string? Description { get; set; }

    [Required]
    [Column("rowguid")]
    public Guid RowGuid { get; set; }

    [Required]
    [Column("ModifiedDate")]
    public DateTime ModifiedDate { get; set; }
}