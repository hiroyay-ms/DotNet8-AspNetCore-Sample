using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

[Table("ProductCategory", Schema = "SalesLT")]
public class ProductCategory
{
    [Key]
    public int ProductCategoryID { get; set; }

    public int? ParentProductCategoryID { get; set; } = null;

    [Required]
    public string Name { get; set; } = string.Empty!;

    [Required]
    public Guid rowguid { get; set; }

    [Required]
    public DateTime ModifiedDate { get; set; }
}
