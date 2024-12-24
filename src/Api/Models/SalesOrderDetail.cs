using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("SalesOrderDetail", Schema = "SalesLT")]
public class SalesOrderDetail
{
    [Required]
    public int SalesOrderID { get; set; }

    [Key]
    public int SalesOrderDetailID { get; set; }

    [Required]
    public Int16 OrderQty { get; set; }

    [Required]
    public int ProductID { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal UnitPriceDiscount { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal LineTotal { get; set; }

    [Required]
    public Guid rowguid { get; set; }

    [Required]
    public DateTime ModifiedDate { get; set; }
}
