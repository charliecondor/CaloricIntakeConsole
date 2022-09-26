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
            string fileJSON = "mealHistoryJSON.json";  // Filename for JSON meal data
            mealHistory = readJSON(fileJSON);  // Checks if JSON meal data exists, then deserializes it into MealHistory object

            int userSelection = -1;
            int errorCode = 0;
            while (userSelection != 0)
            {
                Console.Title = "Caloric Intake Console App";
                displayMainMenu(errorCode);
                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                    switch (userSelection)
                    {
                        case 1: errorCode = 0; addMenu(errorCode, mealHistory); break;  // Add Meal menu
                        case 2: errorCode = 0; editMenu(errorCode, mealHistory); break;  // Edit Meal menu
                        case 3: errorCode = 0; viewHistory(errorCode, mealHistory); break;  // View Meal history chart
                        default: errorCode = 1; break;  // When user doesn't select viable option
                    }
                }
                catch
                {
                    errorCode = 1;
                    userSelection = -1;
                }
            }
            writeJSON(mealHistory, fileJSON);
        }
        static void displayMainMenu(int error_code)
        {
            errorOutput(error_code);
            appTitle();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n   Main Menu\n\n1. Add Meal\n2. Edit Meal\n3. View History\n0. Exit\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("#> ");
        }
        static void editMenu(int error_code, MealHistory mealHistory)
        {
            int userSelection = -1;
            while (userSelection != 0)
            {
                errorOutput(error_code);
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
                        error_code = 0;
                    }
                    else if (userSelection == 0)
                    {
                        error_code = 0;
                        userSelection = 0;
                    }
                }
                catch
                {
                    error_code = 1;
                    userSelection = -1;
                }
            }
        }
        static void viewHistory(int error_code, MealHistory mealHistory)
        {
            List<DailySummary> dailySummary = new List<DailySummary>();
            errorOutput(error_code);
            appTitle();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("+---------------+-----------------------+");
            Console.WriteLine("| Date          | Total Calories        |");
            Console.WriteLine("+---------------+-----------------------+");

            foreach (Meal meal in mealHistory.meals)
            {
                if (!dailySummary.Exists(x => x.Date == meal.Date))
                {
                    dailySummary.Add(new DailySummary { Date = meal.Date, TotalCalories = 0 });
                }
            }

            dailySummary.Sort((x, y) => x.Date.CompareTo(y.Date));
            foreach (DailySummary day in dailySummary)
            {
                foreach (Meal meal in mealHistory.meals)
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

            foreach (DailySummary item in dailySummary)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.Date);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\t| ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item.TotalCalories);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t\t\t|");
            }
            Console.WriteLine("+---------------+-----------------------+");
            Console.WriteLine("| Summary                               |");
            Console.WriteLine("+---------------------------------------+");

            int lifetime_total_calories = new int();
            int lifetime_high_calories = new int();
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
                if (lifetime_low_calories > day.TotalCalories && day.Date != dailySummary.Last().Date)
                {
                    lifetime_low_calories = day.TotalCalories;
                    lifetime_low_date = day.Date;
                }
            }
            int lifetime_average_calories = lifetime_total_calories / dailySummary.Count;
            Console.Write("| Lifetime total: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(lifetime_total_calories);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t\t\t|\n");
            Console.Write("| Lifetime average: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(lifetime_average_calories);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t\t|\n");
            Console.Write("| Lifetime highest: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(lifetime_high_calories + " " + lifetime_high_date);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t|\n");
            Console.Write("| Lifetime lowest: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(lifetime_low_calories + " " + lifetime_low_date);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t|\n");

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
            Console.Write("| Last 10-day total: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(tenday_total_calories);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t\t|\n");
            Console.Write("| Last 10-day average: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(tenday_average_calories);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t\t|\n");
            Console.Write("| Last 10-day highest: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(tenday_high_calories + " " + tenday_high_date);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t|\n");
            Console.Write("| Last 10-day lowest: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(tenday_low_calories + " " + tenday_low_date);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\t|\n");
            Console.WriteLine("+---------------------------------------+");
            
            Console.ReadKey();
        }
        static void addMenu(int error_code, MealHistory mealHistory)
        {
            int userSelection = -1;
            Meal temp_meal = new Meal();
            List<MealItems> temp_items = new List<MealItems>();

            while (userSelection != 0)
            {
                errorOutput(error_code);
                appTitle();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   Add Meal\n\n1. Set Date\n2. Set Time\n3. Add Item\n4. Remove Item\n0. Save & Exit\n");
                displayMealMenu(temp_meal);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("#> ");

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                    switch (userSelection)
                    {
                        case 1: temp_meal.Date = addMealDate(ref error_code); break;
                        case 2: temp_meal.Time = addMealTime(ref error_code); break;
                        case 3: temp_meal.mealitems.Add(addMealItem(ref error_code)); break;
                        case 4: removeMealItem(ref error_code, ref temp_meal); break;
                        default: checkMealItems(ref error_code, ref temp_meal, ref userSelection); break;
                    }
                }
                catch
                {
                    error_code = 1;
                    userSelection = -1;
                }
            }
            mealHistory.AddMeal(temp_meal);
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
        static void displayMealMenu(Meal temp_meal)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Meal Date]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(temp_meal.Date);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Meal Time]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(temp_meal.Time);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Meal Item(s)]");
            Console.WriteLine("|Entry#\t| Qty\t| Unit\t| Description\t\t| Calories\t|:");
            Console.ForegroundColor = ConsoleColor.White;

            if (temp_meal.mealitems != null)
            {
                int entry_number = 0;
                foreach (MealItems item in temp_meal.mealitems)
                {
                    string format_qty = item.Quantity;
                    string format_unit = item.UnitMeasurement;
                    string format_desc = item.Description;
                    string format_cal = Convert.ToString(item.Calories);

                    Console.WriteLine(formatOutput(8, "[" + entry_number + "]") + formatOutput(8, format_qty) + formatOutput(8, format_unit)
                        + formatOutput(24, format_desc) + formatOutput(16, format_cal) + formatOutput(8, ""));
                    entry_number++;
                }
            }
            Console.WriteLine("\n");
        }
        static void errorOutput(int error_code)
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
            Console.WriteLine("Caloric Intake v0.5");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("+-----------------+");
        }
        static string formatOutput(int column_width, string column_text)
        {                        
            string text = "| " + column_text;
            text = text.PadRight(column_width);            
            return text.Substring(0, column_width);
        }
        static MealHistory readJSON(string fileJSON)  // Read from existing JSON meal data file
        {
            var options = new JsonSerializerOptions { IncludeFields = true };  // Include fields for serializing objects with objects
            if (File.Exists(fileJSON))
            {
                string mealJSON = File.ReadAllText(fileJSON);
                return JsonSerializer.Deserialize<MealHistory>(mealJSON, options);
            }
            else
            {
                return null;
            }
        }
        static void writeJSON(MealHistory mealHistory, string fileJSON)  // Write to JSON meal data file
        {
            var options = new JsonSerializerOptions { IncludeFields = true };  // Include fields for serializing objects with objects
            string mealJSON = JsonSerializer.Serialize(mealHistory, options);
            File.WriteAllText(fileJSON, mealJSON);
        }
        static string addMealDate(ref int error_code)
        {
            Console.Write("Enter Meal Date [YYYY/MM/DD]: ");
            string meal_date = Console.ReadLine();
            try
            {                
                DateTime mealDate = new DateTime(Convert.ToInt32(meal_date.Substring(0, 4)),
                    Convert.ToInt32(meal_date.Substring(5, 2)),
                    Convert.ToInt32(meal_date.Substring(8, 2)));
                error_code = 0;
                return mealDate.ToString("yyyy/MM/dd");
            }
            catch
            {
                error_code = 2;
                return "";
            }
        }
        static string addMealTime(ref int error_code)
        {
            Console.Write("Enter Meal Time [HH:MM]: ");
            string meal_time = Console.ReadLine();
            try
            {
                DateTime mealTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(meal_time.Substring(0, 2)), Convert.ToInt32(meal_time.Substring(3, 2)), 0);
                error_code = 0;
                return mealTime.ToString("HH:mm");
            }
            catch
            {
                error_code = 2;
                return "";
            }
        }
        static MealItems addMealItem(ref int error_code)
        {
            Console.WriteLine("Entering Meal Item (QTY/UNIT/DESC/CAL)...");
            Console.Write("Enter Quantity: ");
            string item_qty = Console.ReadLine();
            Console.Write("Enter Unit: ");
            string item_unit = Console.ReadLine();
            Console.Write("Enter Description: ");
            string item_desc = Console.ReadLine();
            Console.Write("Enter Calories: ");
            string str_item_cals = Console.ReadLine();

            try
            {                
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
        static void removeMealItem(ref int error_code, ref Meal tempmeal)
        {
            if (tempmeal.mealitems.Count == 0) { error_code = 4; return; }
            Console.Write("Select Meal Item to Remove [Entry #]: ");
            string str_entry_to_remove = Console.ReadLine();

            try
            {
                int int_entry_to_remove = Convert.ToInt32(str_entry_to_remove);
                if (int_entry_to_remove > 0 || int_entry_to_remove < tempmeal.mealitems.Count)
                {
                    tempmeal.mealitems.RemoveAt(int_entry_to_remove);
                    error_code = 0;
                }
            }
            catch
            {
                error_code = 1;
            }
        }
        static void checkMealItems(ref int error_code, ref Meal tempmeal, ref int userSelection)
        {
            if (tempmeal.Date == null || tempmeal.Time == null || tempmeal.mealitems == null)
            {
                error_code = 3;
                userSelection = -1;
            }
            else
            {
                error_code = 0;
                userSelection = 0;
            }
        }
    }

    public class MealHistory
    {
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
    }
    public class Meal
    {
        public List<MealItems> mealitems = new List<MealItems>();
        public string Date { get; set; }
        public string Time { get; set; }
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