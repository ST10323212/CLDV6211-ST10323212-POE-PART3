using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhumaloFinal.Models
{
    public class Processor
    {
        [Key]
        public int ProcessorId { get; set; }
        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public int Quantity { get; set; }
        public DateTime ProcessingDate { get; set; }

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }


    }
}
