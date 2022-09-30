using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace CaloricIntakeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MealHistory mealHistory = new MealHistory();  // Instantiating a MealHistory object for storing the JSON meal data
            mealHistory = mealHistory.LoadJSON();            
            Console.Title = "Caloric Intake Console App";

            int userSelection = -1;
            int errorCode = 0;
            while (userSelection != 0)
            {                
                drawMainMenu(errorCode);
                try
                {
                    errorCode = 0;
                    userSelection = Convert.ToInt32(Console.ReadLine());
                    switch (userSelection)
                    {
                        case 1: addMealMenu(mealHistory); break;  // Add Meal menu
                        case 2: editMenu(mealHistory); break;  // Edit Meal menu
                        case 3: drawViewHistory(mealHistory); break;  // View Meal history chart
                        default: errorCode = 1; break;  // When user doesn't select viable option
                    }
                }
                catch
                {
                    errorCode = 1;
                    userSelection = -1;
                }
            }
            mealHistory.SaveJSON();
        }        
        static void drawMainMenu(int errorCode)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("╔═════════════════════════╗");
            Console.Write("║");
            Console.Write("   Caloric Intake ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("v0.7   ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╠═════════════════════════╣\n║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("        Main Menu        ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╟─────────────────────────╢\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   1. Add Meal           ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   2. Edit Meal          ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   3. View History       ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   0. Exit               ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╟─────────────────────────╢\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   #>                    ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╠═════════════════════════╣\n");            
            Console.WriteLine("║                         ║");
            Console.WriteLine("║                         ║");
            Console.WriteLine("╚═════════════════════════╝");
            setError(errorCode);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(6, 10);
        }
        static void editMenu(MealHistory mealHistory)
        {
            int userSelection = -1;
            int errorCode = 0;
            while (userSelection != 0)
            {
                errorOutput(errorCode);
                appTitle();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   Edit Meal\n\n1. Edit/Delete Meal\n0. Save & Exit\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("#> ");

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());

                    if (userSelection == 1)
                    {
                        displayMeals(mealHistory);
                        errorCode = 0;
                    }
                    else if (userSelection == 0)
                    {
                        errorCode = 0;
                        userSelection = 0;
                    }
                }
                catch
                {
                    errorCode = 1;
                    userSelection = -1;
                }
            }
        }
        static void drawViewHistory(MealHistory mealHistory)
        {
            List<DailySummary> dailySummary = mealHistory.GenerateSummary();
            dailySummary.Reverse();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(27, 0);
            Console.WriteLine("╔════════════╦══════╗");
            Console.SetCursorPosition(27, 1);
            Console.WriteLine("║ Date       ║ Cals ║");
            Console.SetCursorPosition(27, 2);
            Console.WriteLine("╟────────────╫──────╢");
            int row_count = 3;
            foreach (DailySummary item in dailySummary)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.SetCursorPosition(27, row_count);
                Console.Write("║ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.Date);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(" ║ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.TotalCalories.ToString().PadRight(4));
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(" ║");
                row_count++;
            }
            Console.SetCursorPosition(27, row_count);
            Console.WriteLine("╚════════════╩══════╝");

            Console.SetCursorPosition(48, 0);
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.SetCursorPosition(48, 1);
            Console.WriteLine("║                 Summary                 ║");
            Console.SetCursorPosition(48, 2);
            Console.WriteLine("╟──────────────────────╥──────────────────╢");
            row_count = 3;
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(48, row_count);
                Console.WriteLine("║                      ║                  ║");
                row_count++;
            }
            Console.SetCursorPosition(48, row_count);
            Console.WriteLine("╚══════════════════════╩══════════════════╝");            

            int lifetime_total_calories = 0;
            int lifetime_high_calories = 0;
            int lifetime_low_calories = 100000;
            string lifetime_high_date = "";
            string lifetime_low_date = "";
            foreach (DailySummary day in dailySummary)
            {
                lifetime_total_calories += day.TotalCalories;
                if (lifetime_high_calories < day.TotalCalories)
                {
                    lifetime_high_calories = day.TotalCalories;
                    lifetime_high_date = day.Date;
                }
                if (lifetime_low_calories > day.TotalCalories && day.Date != dailySummary.First().Date)
                {
                    lifetime_low_calories = day.TotalCalories;
                    lifetime_low_date = day.Date;
                }
            }
            int lifetime_average_calories = lifetime_total_calories / dailySummary.Count;

            dailySummary.Reverse();
            int tenday_total_calories = new int();
            int tenday_high_calories = new int();
            int tenday_low_calories = 100000;
            string tenday_high_date = "";
            string tenday_low_date = "";
            int last_ten = dailySummary.Count - 11;
            while (last_ten < dailySummary.Count - 1)
            {
                tenday_total_calories += dailySummary[last_ten].TotalCalories;
                if (tenday_high_calories < dailySummary[last_ten].TotalCalories)
                {
                    tenday_high_calories = dailySummary[last_ten].TotalCalories;
                    tenday_high_date = dailySummary[last_ten].Date;
                }
                else if (tenday_low_calories > dailySummary[last_ten].TotalCalories)
                {
                    tenday_low_calories = dailySummary[last_ten].TotalCalories;
                    tenday_low_date = dailySummary[last_ten].Date;
                }
                last_ten++;
            }
            int tenday_average_calories = tenday_total_calories / 10;

            Console.SetCursorPosition(50, 3);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Lifetime total".PadLeft(20));
            Console.SetCursorPosition(73, 3);            
            Console.Write(lifetime_total_calories);

            Console.SetCursorPosition(50, 4);
            Console.Write("Lifetime average".PadLeft(20));
            Console.SetCursorPosition(73, 4);
            Console.Write(lifetime_average_calories);

            Console.SetCursorPosition(50, 5);
            Console.Write("Lifetime highest".PadLeft(20));
            Console.SetCursorPosition(73, 5);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(lifetime_high_calories + " " + lifetime_high_date);

            Console.SetCursorPosition(50, 6);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Lifetime lowest".PadLeft(20));
            Console.SetCursorPosition(73, 6);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(lifetime_low_calories + " " + lifetime_low_date);

            Console.SetCursorPosition(50, 7);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Last 10-day total".PadLeft(20));
            Console.SetCursorPosition(73, 7);
            Console.Write(tenday_total_calories);

            Console.SetCursorPosition(50, 8);
            Console.Write("Last 10-day average".PadLeft(20));
            Console.SetCursorPosition(73, 8);
            Console.Write(tenday_average_calories);

            Console.SetCursorPosition(50, 9);
            Console.Write("Last 10-day highest".PadLeft(20));
            Console.SetCursorPosition(73, 9);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(tenday_high_calories + " " + tenday_high_date);

            Console.SetCursorPosition(50, 10);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Last 10-day lowest".PadLeft(20));
            Console.SetCursorPosition(73, 10);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(tenday_low_calories + " " + tenday_low_date);

            Console.ReadKey();
        }
        static void addMealMenu(MealHistory mealHistory)
        {
            int userSelection = -1;
            int errorCode = 0;
            Meal temp_meal = new Meal() { Date = DateTime.Now.ToString("yyyy/MM/dd"), 
                Time = DateTime.Now.ToString("HH:mm"), mealitems = new List<MealItems>()};

            while (userSelection != 0)
            {
                drawAddMenu(errorCode, temp_meal);
                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                    switch (userSelection)
                    {
                        case 1: errorCode = editMealInfo(temp_meal); break;
                        case 2: temp_meal.mealitems.Add(addMealItem(ref errorCode)); break;
                        case 3: errorCode = removeMealItem(temp_meal); break;
                        default: if (temp_meal.IsNullOrEmpty()) { errorCode = 3; userSelection = -1; }; break;
                    }
                }
                catch
                {
                    errorCode = 1;
                    userSelection = -1;
                }
            }
            mealHistory.AddMeal(temp_meal);
        }
        static void drawAddMenu(int errorCode, Meal meal)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("╔═════════════════════════╗");
            Console.Write("║");
            Console.Write("   Caloric Intake ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("v0.7   ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╠═════════════════════════╣\n║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("      Add Meal Menu      ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╟─────────────────────────╢\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   1. Edit Date/Time     ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   2. Add Item           ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   3. Remove Item        ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   0. Save & Exit        ");            
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╟─────────────────────────╢\n║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   #>                    ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("║\n╠═════════════════════════╣\n");
            Console.WriteLine("║                         ║");
            Console.WriteLine("║                         ║");
            Console.WriteLine("╚═════════════════════════╝");
            setError(errorCode);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.SetCursorPosition(27, 0);
            Console.Write("╔══════════════════╤══════════════════════════════╗");
            Console.SetCursorPosition(27, 1);
            Console.Write("║ Date:            │ Time:                        ║");
            Console.SetCursorPosition(27, 2);
            Console.Write("╠═══╦════════╦═════╧══╦════════════════════╦══════╣");
            Console.SetCursorPosition(27, 3);
            Console.Write("║ # ║ Qty    ║ Unit   ║ Description        ║ Cals ║");
            Console.SetCursorPosition(27, 4);
            Console.Write("╟───╫────────╫────────╫────────────────────╫──────╢");

            int mealitem_rows = 0;
            int last_row = 5;
            if (meal.mealitems != null) mealitem_rows = meal.mealitems.Count;
            for (int i = 0; i < mealitem_rows; i++)
            {
                Console.SetCursorPosition(27, last_row);
                Console.Write("║   ║        ║        ║                    ║      ║");
                last_row++;
            }
            Console.SetCursorPosition(27, last_row);
            Console.Write("╚═══╩════════╩════════╩════════════════════╩══════╝");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(35, 1);
            Console.Write(meal.Date);
            Console.SetCursorPosition(54, 1);
            Console.Write(meal.Time);

            last_row = 5;
            string entry, qty, unit, desc, cal;
            for (int i = 0; i < mealitem_rows; i++)
            {
                entry = Convert.ToString(i).PadRight(2);
                entry = entry.Substring(0, 2);
                qty = meal.mealitems[i].Quantity.PadRight(6);
                qty = qty.Substring(0, 6);
                unit = meal.mealitems[i].UnitMeasurement.PadRight(6);
                unit = unit.Substring(0, 6);
                desc = meal.mealitems[i].Description.PadRight(18);
                desc = desc.Substring(0, 18);
                cal = Convert.ToString(meal.mealitems[i].Calories).PadRight(4);
                cal = cal.Substring(0, 4);

                Console.SetCursorPosition(29, last_row);
                Console.Write(entry);
                Console.SetCursorPosition(33, last_row);
                Console.Write(qty);
                Console.SetCursorPosition(42, last_row);
                Console.Write(unit);
                Console.SetCursorPosition(51, last_row);
                Console.Write(desc);
                Console.SetCursorPosition(72, last_row);
                Console.Write(cal);

                last_row++;
            }
            Console.SetCursorPosition(7, 10);
        }
        static void displayMeals(MealHistory mealHistory)
        {
            List<MealList> mealList = mealHistory.ListMeal();

            Console.WriteLine("Index \t Date \t Time \t Calories");
            foreach (MealList item in mealList)
            {
                Console.WriteLine(item.Index + "\t" + item.Date + "\t" + item.Time + "\t" + item.Calories);
            }

            Console.WriteLine();
            Console.ReadKey();
        }
        static void setError(int errorCode = 0)
        {
            Console.SetCursorPosition(2, 12);
            Console.ForegroundColor = ConsoleColor.Red;
            switch (errorCode)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("  Status: No Errors     ");                    
                    break;
                case 1:
                    Console.Write("  Status: Error         ");                                    
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  Invalid Input         ");
                    break;
                case 2:
                    Console.Write("  Status: Error         ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  Invalid Input Format  ");
                    break;
                case 3:
                    Console.Write("  Status: Error         ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  Missing Meal Data     ");
                    break;
                case 4:
                    Console.Write("  Status: Error         ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  Missing Meal Item     ");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void errorOutput(int error_code = 0)
        {
            Console.Clear();
            if (error_code == 0)
            {
                // No Error
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Status: No Errors]\n");
            }
            if (error_code == 1)
            {
                // Error
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Status: Invalid Input]\n");
            }
            if (error_code == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Status: Invalid Input - Did not follow correct format]\n");
            }
            if (error_code == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Status: Invalid Input - Missing meal entry data]\n");
            }
            if (error_code == 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Status: No meal items, add one first]\n");
            }
        }
        static void appTitle()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Caloric Intake v0.6");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("+-----------------+");
        }
        static int editMealInfo(Meal meal)
        {
            try
            {
                Console.SetCursorPosition(1, 15);
                Console.Write("Set Date [YYYY/MM/DD]: ");
                string meal_date = Console.ReadLine();
                DateTime mealDate = new DateTime(Convert.ToInt32(meal_date.Substring(0, 4)),
                    Convert.ToInt32(meal_date.Substring(5, 2)),
                    Convert.ToInt32(meal_date.Substring(8, 2)));
                meal.Date = mealDate.ToString("yyyy/MM/dd");
                
                Console.SetCursorPosition(1, 16);
                Console.Write("Set Time [HH:MM]: ");
                string meal_time = Console.ReadLine();
                DateTime mealTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(meal_time.Substring(0, 2)), Convert.ToInt32(meal_time.Substring(3, 2)), 0);
                meal.Time = mealTime.ToString("HH:mm");
                return 0;
            }
            catch
            {
                return 2;
            }
        }                
        static MealItems addMealItem(ref int error_code)
        {
            try
            {
                Console.SetCursorPosition(1, 15);
                Console.WriteLine("Add Meal Item (QTY/UNIT/DESC/CAL)...");
                Console.SetCursorPosition(1, 16);
                Console.Write("Enter Quantity: ");
                string item_qty = Console.ReadLine();
                Console.SetCursorPosition(1, 17);
                Console.Write("Enter Unit: ");
                string item_unit = Console.ReadLine();
                Console.SetCursorPosition(1, 18);
                Console.Write("Enter Description: ");
                string item_desc = Console.ReadLine();
                Console.SetCursorPosition(1, 19);
                Console.Write("Enter Calories: ");
                string str_item_cals = Console.ReadLine();

                int int_item_cals = Convert.ToInt32(str_item_cals);
                error_code = 0;
                return new MealItems { Quantity = item_qty, UnitMeasurement = item_unit, 
                    Description = item_desc, Calories = int_item_cals };
            }
            catch
            {
                error_code = 1;
                return new MealItems();
            }
        }
        static int removeMealItem(Meal tempmeal)
        {
            try
            {
                if (tempmeal.mealitems.Count == 0) { return 4; }
                Console.SetCursorPosition(1, 15);
                Console.Write("Item to Remove [#]: ");
                string str_entry_to_remove = Console.ReadLine();
                int int_entry_to_remove = Convert.ToInt32(str_entry_to_remove);
                if (int_entry_to_remove > 0 || int_entry_to_remove < tempmeal.mealitems.Count)
                {
                    tempmeal.mealitems.RemoveAt(int_entry_to_remove);
                }
                return 0;
            }
            catch
            {
                return 1;
            }
        }        
    }

    public class MealHistory
    {
        private const string fileJSON = "mealHistoryJSON.json";  // Filename for JSON meal data
        public List<Meal> meals = new List<Meal>();
        public bool AddMeal(Meal meal)
        {
            meals.Add(new Meal { Date = meal.Date, Time = meal.Time, mealitems = meal.mealitems });
            return true;
        }
        public List<MealList> ListMeal()
        {
            List<MealList> mealList = new List<MealList>();
            int index = 0;

            foreach (Meal item in meals)
            {
                int mealTotalCalories = 0;
                foreach (MealItems mealItem in item.mealitems)
                {
                    mealTotalCalories += mealItem.Calories;
                }
                mealList.Add(new MealList { Index = index, Date = item.Date, Time = item.Time, Calories = mealTotalCalories });
                index++;
            }
            return mealList;
        }
        public MealHistory LoadJSON()
        {
            var options = new JsonSerializerOptions { IncludeFields = true };  // Include fields for serializing objects with objects            
            return File.Exists(fileJSON) ? JsonSerializer.Deserialize<MealHistory>(File.ReadAllText(fileJSON), options) : null;            
        }
        public void SaveJSON()
        {
            var options = new JsonSerializerOptions { IncludeFields = true };  // Include fields for serializing objects with objects
            string mealJSON = JsonSerializer.Serialize(this, options);
            File.WriteAllText(fileJSON, mealJSON);
        }
        public List<DailySummary> GenerateSummary()
        {
            List<DailySummary> dailySummary = new List<DailySummary>();

            foreach (Meal meal in meals)
            {
                if (!dailySummary.Exists(x => x.Date == meal.Date))
                {
                    dailySummary.Add(new DailySummary { Date = meal.Date, TotalCalories = 0 });
                }
            }

            dailySummary.Sort((x, y) => x.Date.CompareTo(y.Date));

            foreach (DailySummary day in dailySummary)
            {
                foreach (Meal meal in meals)
                {
                    if (day.Date == meal.Date)
                    {
                        foreach (MealItems mealitem in meal.mealitems)
                        {
                            day.TotalCalories += mealitem.Calories;
                        }
                    }
                }
            }

            return dailySummary;
        }
    }
    public class Meal
    {
        public List<MealItems> mealitems = new List<MealItems>();
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsNullOrEmpty()
        {
            return String.IsNullOrEmpty(Date) || String.IsNullOrEmpty(Time) || !mealitems.Any();
        }
        public bool UpdateDate(string str_date)
        {
            try
            {
                DateTime mealDate = new DateTime(Convert.ToInt32(str_date.Substring(0, 4)),
                    Convert.ToInt32(str_date.Substring(5, 2)),
                    Convert.ToInt32(str_date.Substring(8, 2)));
                Date = mealDate.ToString("yyyy/MM/dd");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class MealItems
    {
        public string Quantity { get; set; }
        public string UnitMeasurement { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }        
    }
    public class DailySummary
    {
        public string Date { get; set; }
        public int TotalCalories { get; set; }
    }
    public class MealList
    {
        public int Index { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Calories { get; set; }
    }
}