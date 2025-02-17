using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuncProj.Models;

[Table("ProductModel", Schema = "SalesLT")]
public class ProductModel
{
    [Key]
    public int ProductModelID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? CatalogDescription { get; set; } = string.Empty;

    [Required]
    public Guid rowguid { get; set; }

    [Required]
    public DateTime ModifiedDate { get; set; }
}
