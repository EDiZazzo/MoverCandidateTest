using System.ComponentModel.DataAnnotations;

namespace MoverCandidateTest.Inventory.Model
{
    public class InventoryItem
    {
        [Key]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "SKU must contain only letters and numbers.")]
        public string Sku { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, uint.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public uint Quantity { get; set; }
        
        public InventoryItem() { }
        
        public InventoryItem(string sku, string description, uint quantity)
        {
            Sku = sku;
            Description = description;
            Quantity = quantity;
        }
    }
}