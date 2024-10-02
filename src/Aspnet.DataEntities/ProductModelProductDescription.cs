using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspnet.DataEntities;

[Table("ProductModelProductDescription", Schema = "SalesLT")]
public class ProductModelProductDescription
{
        [Key]
        [Column("ProductModelID")]
        public int ProductModelId { get; set; }

        [Key]
        [Column("ProductDescriptionID")]
        public int ProductDescriptionId { get; set; }

        [Key]
        [Column("Culture")]
        public string? Culture { get; set; }

        [Required]
        [Column("rowguid")]
        public Guid RowGuid { get; set; }

        [Required]
        [Column("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
}