using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Synthesis;

namespace InventorySystem
{
    class Program
    {
        // File path of inventory
        public static string filepath = "..\\Inventory.txt";
        // Filepath of customer's cart
        public static string filepath_c = "..\\Cart.txt";
        public static string add_to_cart;
        public static string name, contact;
        public static int total = 0;
        public static string dline = "=====================================================================================";
        public static string sline = "-------------------------------------------------------------------------------------";
        public static SpeechSynthesizer s = new SpeechSynthesizer();

        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            FileStream fs_c = new FileStream(filepath_c, FileMode.Create, FileAccess.Write);
            fs_c.Close();

            // Configure the audio output.
            s.SetOutputToDefaultAudioDevice();

            Console.ForegroundColor = ConsoleColor.DarkGray;

            string heading = "Shopping Cart & Inventory System";

            Console.WriteLine(dline);
            Console.WriteLine("\t\t\t"+ heading);
            Console.WriteLine(sline);

            s.Speak("Shopping Cart & Inventory System");

            Console.WriteLine("Select your category: ");

            Console.WriteLine("\n\t[1]Retailer");
            Console.WriteLine("\t[2]Customer\n");
            Console.WriteLine(dline);

            //s.Speak("Select your category: ");

            Console.Write("\nChoice: ");

            string opt = Console.ReadLine();
            if (opt == "1")
            {                
                Retailer();
            }
            else if (opt == "2")
            {
                Customer();
            }
            else
            {
                Console.WriteLine("\nInvalid Option Entered!");
                s.Speak("Invalid Option Entered");
            }

        }
        //Retailer
        static void Retailer()
        {
          
           
            s.SetOutputToDefaultAudioDevice();

            Console.ForegroundColor = ConsoleColor.Blue;
            
            Console.Clear();

            Console.WriteLine(dline);
            Console.WriteLine("\t\t\tWelcome to the Inventory System!");
            Console.WriteLine(sline);

            s.Speak("Welcome to the Inventory System! ");

            Console.Write("\n\t[1] View items in Inventory");
            Console.Write("\n\t[2] Add items to Inventory\n\n");
            Console.Write(dline);
            Console.Write("\n\nChoice: ");
                        
            string opt = Console.ReadLine();
            Console.Clear();
            switch (opt)
            {
                case "1":
                    ViewItems();
                    Console.WriteLine("\nPress \"Enter\" to return to main menu. ");
                    s.Speak("Press Enter to return to main menu");
                    Console.ReadKey();
                    Main();
                    break;

                case "2":
                    AddItems();
                    break;

                default:
                    Console.WriteLine("Invalid Option!");
                    s.Speak("Invalid Option");
                    break;
            }

        }
        //Retailer - To add items to inventory
        static void AddItems()
        {
            Console.Write("\nEnter the item do you want to add: ");
            s.Speak("Enter the item do you want to add ");
            string item = Console.ReadLine();

            Console.Write("Enter price of one unit of entered item: ");
            s.Speak("Enter price of one unit of entered item:");
            int price = Convert.ToInt32(Console.ReadLine());

            FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
            StreamWriter write = new StreamWriter(fs);

            write.Write(string.Format("{0},", item.ToUpper()));
            write.WriteLine(string.Format("{0}", price));

            write.Close();
            fs.Close();

            Console.WriteLine("\nSaved!\n");
            s.Speak("Saved");

            Console.Write("\nDo you want to add more items?(Y/N): ");
            s.Speak("Do you want to add more items?");
            char more = Convert.ToChar(Console.ReadLine());

            if (more == 'y' || more == 'Y')
            {
                AddItems();
            }
            else
            {
                Console.WriteLine("\nOkay!");
                Console.ReadKey();
                Main();
                s.Speak("Okay");
            }
        }

        //Retailer - To view Items available in Inventory
        static void ViewItems()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs);

            Console.WriteLine("========================================");
            Console.WriteLine("{0,16}{1,16}", "ITEM", "PRICE");
            Console.WriteLine("----------------------------------------");

            string str = read.ReadLine();
            
            read.ReadLine();                        

            while (!read.EndOfStream)
            {
                string line = read.ReadLine();
                string[] values = line.Split(',');
                Console.Write(string.Format("{0, 16}", values[0]));
                Console.WriteLine(string.Format("{0, 16}", values[1]));
            }

            read.Close();
            fs.Close();
            Console.WriteLine("========================================");
        }

        //Customer
        static void Customer()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("\t\tDetails");
            Console.WriteLine("----------------------------------------");

            Console.Write("\nEnter your name: ");
            s.Speak("Please enter your name");
            name = Console.ReadLine();
            

            Console.Write("Enter your contact number: ");
            s.Speak("Please enter your contact number");
            contact = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("========================================");

            Console.WriteLine("\nFollowing is the list of items along \nwith per unit price: \n");
            s.Speak("Following is the list of available items:");

            ViewItems();
          
            Console.WriteLine();

            Console.WriteLine("Enter the name of items you want to add to cart: \n(Enter \"0\" to get receipt)\n");
            s.Speak("Enter the name of items you want to add to cart");

            while (add_to_cart != "0")
            {
              
                add_to_cart = Console.ReadLine();

                AddToCart();

            }
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;

            Receipt();

            Console.ReadKey();

            Main();

        }

        //Customer - to add Items in Customer's Cart
        static void AddToCart()
        {

            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs);
            bool found = false;

            string str = read.ReadLine();

            while (!read.EndOfStream)
            {

                string[] values = str.Split(',');
                if (add_to_cart.ToUpper() == values[0])
                {
                    
                    FileStream fs_c = new FileStream(filepath_c, FileMode.Append, FileAccess.Write);
                    StreamWriter write_c = new StreamWriter(fs_c);
                                       
                    write_c.Write(string.Format("{0},", values[0]));
                    write_c.WriteLine(string.Format("{0}", values[1]));
                    total += int.Parse(values[1]);

                    write_c.Close();
                    found = true;

                    break;
                }
                str = read.ReadLine();
            }

            if (found == false && add_to_cart.ToUpper() != "0")
            {
                Buzzer();
                Console.WriteLine("Sorry, item is not available!");
                s.Speak("Sorry, item is not available");
               
            }

            read.Close();
            fs.Close();

        }
        //Customer - Generate customer's receipt
        static void Receipt()
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.White;


            string seperators = "-----------------------------------------------------------";
            string heading = "RECEIPT";

            Console.WriteLine("===========================================================");
            Console.WriteLine("\t\t\t" + heading);
            Console.WriteLine(seperators);

            Random random = new Random();
            int randomBetween100And500 = random.Next(1000, 5000);
            string sp = "Muhammad Hamza";

         
            Console.Write("Customer Name: " + name);
            Console.WriteLine(string.Format("{0, 34}", ("Contact Number: " + contact)));

           
            Console.Write("Sales Person: " + sp);
            Console.WriteLine(string.Format("{0, 30}", ("Receipt Number: " + randomBetween100And500)));

            Console.WriteLine(seperators);
           
            Console.WriteLine("{0,16}{1,28}", "ITEM", "PRICE");
            Console.WriteLine();

            FileStream fs_c = new FileStream(filepath_c, FileMode.Open, FileAccess.Read);
            StreamReader read_c = new StreamReader(fs_c);

          

            string str = read_c.ReadLine();
            while (str != null)
            {
                
                string[] values = str.Split(',');

                Console.Write(string.Format("{0, 16}", values[0]));
                Console.WriteLine(string.Format("{0, 28}", values[1]));
                
                str = read_c.ReadLine();
            }

            Console.WriteLine();

            Console.WriteLine("===========================================================");
            Console.WriteLine("{0,16}{1,28}", "TOTAL: ", total);
            string tot = Convert.ToString(total);
            Console.WriteLine("===========================================================");
           
            s.Speak("Your Total is" + total);
            Console.ReadLine();
            read_c.Close();
            fs_c.Close();
        }


        //Customer - Beep if unavialable Item is put in the Cart by Customer
        static void Buzzer()
        {
            var soundPlayer = new System.Media.SoundPlayer();
            soundPlayer.SoundLocation = "..\\buzzer.wav";
            soundPlayer.PlaySync();
        }      
    }
}

