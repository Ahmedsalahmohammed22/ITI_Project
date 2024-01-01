using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TestRestApi.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }


        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Order? orders { get; set; }

        [ForeignKey(nameof(items))]
        public int ItemId { get; set;}
        public virtual Item? items { get; set;}
        [Required]
        public decimal Price { get; set; }
    }
}
