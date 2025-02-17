using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("ProductDescription", Schema = "SalesLT")]
public class ProductDescription
{
        [Key]
        public int ProductDescriptionID { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
}
