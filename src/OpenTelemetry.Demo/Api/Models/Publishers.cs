using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("publishers", Schema = "Pubs")]
public class Publishers
{
    [Key]
    public string pub_id { get; set; } = string.Empty;

    public string? pub_name { get; set; } = string.Empty;

    public string? city { get; set; } = string.Empty;

    public string? state { get; set; } = string.Empty;

    public string? country { get; set; } = string.Empty;

    public string? sales_rep { get; set; } = string.Empty;
}