﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TestRestApi.Data.Models
{
    public class Item
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public double Price { get; set; }
        
        public string? Notes { get; set; }
        public byte[]? Image { get; set; }
        [ForeignKey(nameof(category))]
        public int CategoryId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Category category { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<OrderItem> OrderItems { get; set;}

    }
}
