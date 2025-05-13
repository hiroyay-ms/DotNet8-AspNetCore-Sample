using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuncProjIntAspNet.Models;

[Table("ProductModelProductDescription", Schema = "SalesLT")]
public class ProductModelProductDescription
{
        [Key]
        public int ProductModelID { get; set; }

        [Key]
        public int ProductDescriptionID { get; set; }

        [Required]
        public string Culture { get; set; } = string.Empty;

        [Required]
        public Guid rowguid { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
}
