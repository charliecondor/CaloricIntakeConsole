using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CaloricIntakeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MealHistory mealHistory = new MealHistory();            

            Console.Title = "Caloric Intake Console App";
            Console.Clear();

            int userSelection = -1;
            int errorCode = 0;
                      
            while(userSelection != 0)
            {
                mainMenu(errorCode);

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    errorCode = 1;
                    userSelection = -1;
                }
                
                if (userSelection == 1)
                {
                    errorCode = 0;
                    addMenu(errorCode, mealHistory);                  
                }
                else if(userSelection == 2)
                {
                    errorCode = 0;
                    Console.Clear();
                    Console.WriteLine("Edit Meal Menu");
                    Console.ReadKey();
                }
                else if(userSelection == 3)
                {
                    errorCode = 0;
                    Console.Clear();
                    Console.WriteLine("View History Menu");
                    Console.ReadKey();
                }
                else
                {
                    errorCode = 1;
                }
            }
        }

        static void mainMenu(int error_code)
        {
            Console.Clear();
            errorOutput(error_code);
            appTitle();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n   Main Menu\n\n1. Add Meal\n2. Edit Meal\n3. View History\n0. Exit\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("#> ");            
        }

        static void addMenu(int error_code, MealHistory mealHistory)
        {
            int userSelection = -1;
            Meal temp_meal = new Meal();
            List<MealItems> temp_items = new List<MealItems>();

            while (userSelection != 0)
            {
                Console.Clear();
                errorOutput(error_code);
                appTitle();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   Add Meal\n\n1. Set Date\n2. Set Time\n3. Add Item\n4. Remove Item\n0. Save & Exit\n");                
                displayMeal(temp_meal);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("#> ");

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    error_code = 1;
                    userSelection = -1;
                }

                if (userSelection == 1)
                {
                    Console.Write("Enter Meal Date [YYYY/MM/DD]: ");
                    string meal_date = Console.ReadLine();
                    char[] meal_date_char_codes = meal_date.ToCharArray();
                                         
                    if (meal_date_char_codes.Length != 10) { error_code = 2; }
                    else if (meal_date_char_codes[0] < 48 || meal_date_char_codes[0] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[1] < 48 || meal_date_char_codes[1] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[2] < 48 || meal_date_char_codes[2] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[3] < 48 || meal_date_char_codes[3] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[4] != 47) { error_code = 2; }
                    else if (meal_date_char_codes[5] < 48 || meal_date_char_codes[5] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[6] < 48 || meal_date_char_codes[6] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[7] != 47) { error_code = 2; }
                    else if (meal_date_char_codes[8] < 48 || meal_date_char_codes[8] > 57) { error_code = 2; }
                    else if (meal_date_char_codes[9] < 48 || meal_date_char_codes[9] > 57) { error_code = 2; }
                    else
                    {
                        error_code = 0;
                        temp_meal.Date = meal_date;
                    }
                }                                    
                else if (userSelection == 2)
                {
                    Console.Write("Enter Meal Time [HH:MM]: ");
                    string meal_time = Console.ReadLine();
                    char[] meal_time_char_codes = meal_time.ToCharArray();

                    if (meal_time_char_codes.Length != 5) { error_code = 2; }
                    else if (meal_time_char_codes[0] < 48 || meal_time_char_codes[0] > 50) { error_code = 2; }
                    else if (meal_time_char_codes[1] < 48 || meal_time_char_codes[1] > 57) { error_code = 2; }
                    else if (meal_time_char_codes[2] != 58) { error_code = 2; }
                    else if (meal_time_char_codes[3] < 48 || meal_time_char_codes[3] > 53) { error_code = 2; }
                    else if (meal_time_char_codes[4] < 48 || meal_time_char_codes[4] > 57) { error_code = 2; }
                    else
                    {
                        error_code = 0;
                        temp_meal.Time = meal_time;
                    }                    
                }
                else if (userSelection == 3)
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
                    int int_item_cals = new int();

                    try
                    {
                        int_item_cals = Convert.ToInt32(str_item_cals);
                        error_code = 0;
                        temp_items.Add(new MealItems { Quantity = item_qty, UnitMeasurement = item_unit, Description = item_desc, Calories = int_item_cals });
                        temp_meal.mealitems = temp_items;
                    }
                    catch
                    {
                        error_code = 1;
                        userSelection = -1;
                    }                    
                }
                else if (userSelection == 4)
                {
                    if (temp_meal.mealitems == null || temp_meal.mealitems.Count == 0)
                    {
                        error_code = 4;
                    }
                    else
                    {
                        Console.Write("Select Meal Item to Remove [Entry #]: ");
                        string str_entry_to_remove = Console.ReadLine();
                        int int_entry_to_remove = new int();

                        try
                        {
                            int_entry_to_remove = Convert.ToInt32(str_entry_to_remove);
                            
                            if (int_entry_to_remove < 0 || int_entry_to_remove > temp_meal.mealitems.Count)
                            {
                                error_code = 1;
                                userSelection = -1;
                            }
                            else
                            {
                                temp_meal.mealitems.RemoveAt(int_entry_to_remove);
                                error_code = 0;
                            }                            
                        }
                        catch
                        {
                            error_code = 1;
                            userSelection = -1;
                        }
                    }                    
                }
                else if (userSelection == 0)
                {
                    if (temp_meal.Date == null)
                    {
                        error_code = 3;
                        userSelection = -1;
                    }
                    else if (temp_meal.Time == null)
                    {
                        error_code = 3;
                        userSelection = -1;
                    }
                    else if (temp_meal.mealitems == null)
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
            mealHistory.meals = new List<Meal> { temp_meal };
        }

        static void displayMeal(Meal temp_meal)
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
                    Console.WriteLine(formatOutput(8, "[" + entry_number + "]") + formatOutput(8, item.Quantity) + formatOutput(8, item.UnitMeasurement) 
                        + formatOutput(24, item.Description) + formatOutput(16, Convert.ToString(item.Calories)) + formatOutput(8, ""));
                    entry_number++;
                }
            }            
            Console.WriteLine("\n");
        }

        static void errorOutput(int error_code)
        {
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
            Console.WriteLine("Caloric Intake v0.4");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("+-----------------+");
        }

        static string formatOutput(int column_width, string column_text)
        {
            string formatted_text = "| ";
            string padded_text = "";
            int formatted_padding = 2;
            int padded_format = column_width - formatted_padding;

            if (column_text.Length < padded_format)
            {
                padded_text = formatted_text + column_text;
                
                int empty_space = 2 + column_text.Length;
                while (empty_space < column_width)
                {
                    padded_text += " ";
                    empty_space++;
                }
            }

            return padded_text;
        }
    }    

    public class MealHistory
    {
        public List<Meal> meals { get; set; }
    }

    public class Meal
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public List<MealItems> mealitems { get; set; }
    }

    public class MealItems
    {
        public string Quantity { get; set; }
        public string UnitMeasurement { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
    }
}
