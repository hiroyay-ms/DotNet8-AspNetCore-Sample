using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuncProjIntAspNet.Models;

[Table("Customer", Schema = "SalesLT")]
public class Customer
{
    [Key]
    public int CustomerID { get; set; }

    [Required]
    public bool NameStyle { get; set; }

    public string? Title { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string? Suffix { get; set; } = string.Empty;

    public string? CompanyName { get; set; } = string.Empty;

    public string? SalesPerson { get; set; } = string.Empty;

    public string? EmailAddress { get; set; } = string.Empty;

    public string? Phone { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string PasswordSalt { get; set; } = string.Empty;

    [Required]
    public Guid rowguid { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
}
