using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Website_Progress.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }


        [ValidateNever]
        [AllowNull]
        public string UserId { get; set; }


        [ValidateNever]
        [AllowNull]
        public List<CartItemViewModel> Items { get; set; }


        [Required]
        [AllowNull]
        public DeliveryUserViewModel DeliveryUser { get; set; }


        [Required]
        public DateTime CreationDateTime { get; set; }


        [Required]
        public OrderStatusViewModel Status { get; set; }


        public decimal? TotalCost { get; set; }


        public int? ItemsQuantity => Items?.Sum(item => item.Quantity);
    }
}
