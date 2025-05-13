using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuncCustomClaimProvider.Models;

[Table("Customer", Schema = "dbo")]
public class Customers
{
    [Key]
    public int CustomerID { get; set; }

    public string? CustomerName { get; set; } = string.Empty;

    [Required]
    public string CustomerGuid { get; set; } = string.Empty;

    [Required]
    public string EmailAddressDomain { get; set; } = string.Empty;

    [Required]
    public string ServicePlan { get; set; } = string.Empty;

    public bool? TenantEnablement { get; set; }
}