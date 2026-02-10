// Student Number: S10273555
// Student Name: Joelle Heng
// Partner Name: Tiara

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273555_PRG2Assignment
{
    public class Menu
    {
        private string menuId;
        public string MenuId { get; private set; }
        private string menuName;
        public string MenuName { get; private set; }

        private List<FoodItem> foodItemList = new List<FoodItem>();

        public List<FoodItem> GetFoodItems()
        {
            return new List<FoodItem>(foodItemList);
        }

        public Menu(string menuId, string menuName)
        {
            if (menuId == null || menuId == "")
                throw new ArgumentException("Menu ID cannot be empty");

            if (menuName == null || menuName == "")
                throw new ArgumentException("Menu name cannot be empty");

            MenuId = menuId;
            MenuName = menuName;

            foodItemList = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            if (foodItem == null)
                throw new ArgumentException("Food item cannot be null");

            // prevent duplicates by ItemName (basic loop, no LINQ)
            for (int i = 0; i < foodItemList.Count; i++)
            {
                if (foodItemList[i].ItemName == foodItem.ItemName)
                    throw new ArgumentException("Food item already exists in the menu");
            }

            foodItemList.Add(foodItem);
        }

        public bool RemoveFoodItem(FoodItem foodItem)
        {
            if (foodItem == null)
                return false;

            for (int i = 0; i < foodItemList.Count; i++)
            {
                if (foodItemList[i].ItemName == foodItem.ItemName)
                {
                    foodItemList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void DisplayFoodItems()
        {
            if (foodItemList.Count == 0)
            {
                Console.WriteLine("No food items in this menu.");
                return;
            }

            for (int i = 0; i < foodItemList.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + foodItemList[i].ToString());
            }
        }

        public override string ToString()
        {
            return MenuName + " (" + MenuId + ") - " + foodItemList.Count + " item(s)";
        }
    }
}
