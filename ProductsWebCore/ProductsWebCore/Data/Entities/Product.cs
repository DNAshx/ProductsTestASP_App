using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebCore.Data.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }        
    }
}
