using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspnet.DataEntities;

[Table("ProductModel", Schema = "SalesLT")]
public class ProductModel
{
    [Key]
    [Column("ProductModelID")]
    public int ProductModelId { get; set; }

    [Required]
    [Column("Name")]
    public string? ModelName { get; set; }

    [Column("CatalogDescription")]
    public string? CatalogDescription { get; set; }

    [Required]
    [Column("rowguid")]
    public Guid RowGuid { get; set; }

    [Required]
    [Column("ModifiedDate")]
    public DateTime ModifiedDate { get; set; }
}