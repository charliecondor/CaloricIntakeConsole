using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloricIntakeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MealHistory mealHistory = new MealHistory();            

            Console.Title = "Caloric Intake Console App";
            Console.ForegroundColor = ConsoleColor.Blue;
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
                    addMenu(errorCode);                  
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Caloric Intake v0.1");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("+-----------------+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n   Main Menu\n\n1. Add Meal\n2. Edit Meal\n3. View History\n0. Exit\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("#> ");            
        }

        static void addMenu(int error_code)
        {
            string userInput = null;
            int userSelection = -1;            

            while (userSelection != 0)
            {
                Console.Clear();
                errorOutput(error_code);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Caloric Intake v0.1");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("+-----------------+");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   Add Meal\n\n1. Set Date\n2. Set Time\n3. Add Item\n4. Remove Item\n0. Save & Exit\n");                
                displayMeal();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("#> ");

                try
                {
                    userSelection = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    error_code = 1;
                    userSelection = 0;
                }
            }
        }

        static void displayMeal()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Meal Date]: ");
            Console.WriteLine("[Meal Time]: ");
            Console.WriteLine("[Meal Item(s)]");
            Console.WriteLine("[Quantity | Unit | Description | Calories]");
            Console.WriteLine("\n");
        }

        static void errorOutput(int error_code)
        {
            if(error_code == 0)
            {
                // No Error
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Status: No Errors]\n");
            }
            if(error_code == 1)
            {
                // Error
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Status: Invalid Input]\n");
            }
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
