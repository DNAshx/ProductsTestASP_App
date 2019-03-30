using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebCore.Data.Entities
{
    public class ProductCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CategoryType { get; set; }

        public List<Product> Products { get; set; }
    }
}
