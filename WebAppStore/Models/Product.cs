using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [ForeignKey("catID")]
        public int CategoryId { get; set; }

        //Navigation Properties
        public virtual Category? catID { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();

    }
}
