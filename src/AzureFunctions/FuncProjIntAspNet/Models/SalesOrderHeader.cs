using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuncProjIntAspNet.Models;

[Table("SalesOrderHeader", Schema = "SalesLT")]
public class SalesOrderHeader
{
    [Key]
    public int SalesOrderID { get; set; }

    [Required]
    public byte RevisionNumber { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? ShipDate { get; set; } = null;

    [Required]
    public byte Status { get; set; }

    [Required]
    public bool OnlineOrderFlag { get; set; }

    [Required]
    public string SalesOrderNumber { get; set; } = string.Empty;

    public string? PurchaseOrderNumber { get; set; } = string.Empty;

    public string? AccountNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerID { get; set; }

    public int? ShipToAddressID { get; set; } = null;

    public int? BillToAddressID { get; set; } = null;

    [Required]
    public string ShipMethod { get; set; } = string.Empty;

    public string CreditCardApprovalCode { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "money")]
    public decimal SubTotal { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal TaxAmt { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal Freight { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal TotalDue { get; set; }

    [Required]
    public Guid rowguid { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
}
