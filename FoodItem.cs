// Student Number: S10273555
// Student Name: Joelle Heng
// Partner Name: Tiara

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273555_PRG2Assignment
{
    public class FoodItem
    {
        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        private string itemDesc;
        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }

        private double itemPrice;
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        private string customise;
        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise)
        {
            if (itemName == null || itemName == "")
                throw new ArgumentException("Item name cannot be empty");

            if (itemDesc == null || itemDesc == "")
                throw new ArgumentException("Item description cannot be empty");

            if (itemPrice <= 0)
                throw new ArgumentException("Item price must be more than 0");

            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = customise;
        }

        public override string ToString()
        {
            return ItemName + " - " + ItemDesc + " ($" + ItemPrice.ToString("0.00") + ")";
        }
    }
}