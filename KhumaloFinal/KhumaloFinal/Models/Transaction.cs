using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhumaloFinal.Models
{
    public class Transaction
    {

        [Key]
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }

        [ForeignKey("ProductId")]
        public Product Products { get; set; }

    }
}
