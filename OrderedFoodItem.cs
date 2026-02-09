using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S10273555_PRG2Assignment;
// Student Number: S10275515
// Student Name: Nur Tiara Nasha
// Partner Name: Joelle
namespace PRG2Assignment
{
    public class OrderedFoodItem : FoodItem
    {
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

        // Correct constructor
        public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, string customise, int qty)
            : base(itemName, itemDesc, itemPrice, customise) // pass values to FoodItem
        {
            QtyOrdered = qty;
            SubTotal = QtyOrdered * ItemPrice; // calculate subtotal 
        }

        public double CalculateSubtotal()
        {
            SubTotal = QtyOrdered * ItemPrice;
            return SubTotal;
        }

        public override string ToString()
        {
            return base.ToString() + " x " + QtyOrdered + " = $" + SubTotal.ToString("0.00");
        }
    }
}
