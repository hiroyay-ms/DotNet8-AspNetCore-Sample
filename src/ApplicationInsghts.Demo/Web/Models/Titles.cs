using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

[Table("titles", Schema = "Pubs")]
public class Titles
{
    [Key]
    public string title_id { get; set; } = string.Empty;

    [Required]
    public string title { get; set; } = string.Empty;

    [Required]
    public string type { get; set; } = string.Empty;

    public string? pub_id { get; set; } = string.Empty;

    [Column(TypeName = "money")]
    public decimal? price { get; set; }

    [Column(TypeName = "money")]
    public decimal? advance { get; set; }

    public int? royalty { get; set; }

    public int? ytd_sales { get; set; }

    public string? notes { get; set; }

    [Required]
    public DateTime pubdate { get; set; }
}
