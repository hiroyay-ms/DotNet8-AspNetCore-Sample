using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("Address", Schema = "SalesLT")]
public class Address
{
    [Key]
    public int AddressID { get; set; }

    [Required]
    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string StateProvince { get; set; } = string.Empty;

    [Required]
    public string CountryRegion { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public Guid rowguid { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
}
