using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace LINQ_Examples
{
    class Program
    {
        

    

        static List<Customer> customers = new List<Customer>
        {          
            new Customer {ID = "LD2961", First = "Cailin", Last = "Alford", State = "UT", Price = 930.00, Purchases = new string[] {"Panel 625", "Panel 200"}},
            new Customer {ID = "LD8403", First = "Theodore", Last = "Brock", State = "AR", Price = 2100.00, Purchases = new string[] {"12V Li"}},
            new Customer {ID = "LD7261", First = "Jerry", Last = "Gill", State = "UT", Price = 585.80, Purchases = new string[] {"Bulb 23W", "Panel 625"}},
            new Customer {ID = "LD9918", First = "Owens", Last = "Howell", State = "UT", Price = 512.00, Purchases = new string[] {"Panel 200", "Panel 180"}},
            new Customer {ID = "LD9332", First = "Adena", Last = "Jenkins", State = "OR", Price = 2267.80, Purchases = new string[] {"Bulb 23W", "12V Li", "Panel 180"}},
            new Customer {ID = "LD8300", First = "Medge", Last = "Ratliff", State = "GA", Price = 1034.00, Purchases = new string[] {"Panel 625"}},
            new Customer {ID = "LD8776", First = "Sydney", Last = "Bartlett", State = "OR", Price = 2105.00, Purchases = new string[] {"12V Li", "AA NiMH"}},
            new Customer {ID = "LD1572", First = "Malik", Last = "Faulkner", State = "MI", Price = 167.80, Purchases = new string[] {"Bulb 23W", "Panel 180"}},
            new Customer {ID = "LD1042", First = "Serena", Last = "Malone", State = "UT", Price = 512.00, Purchases = new string[] {"Panel 180", "Panel 200"}},
            new Customer {ID = "LD9674", First = "Hadley", Last = "Sosa", State = "OR", Price = 590.20, Purchases = new string[] {"Panel 625", "Bulb 23W", "Bulb 9W"}},
            new Customer {ID = "LD7414", First = "Nash", Last = "Vasquez", State = "OR", Price = 10.20, Purchases = new string[] {"Bulb 23W", "Bulb 9W"}},
            new Customer {ID = "LD5537", First = "Joshua", Last = "Delaney", State = "UT", Price = 350.00, Purchases = new string[] {"Panel 200"}}
                        
        };

        


        static void Main(string[] args)
        {

            //your code here
            CreateXML();

            Console.WriteLine("LINQ XML Query");

            QueryXML();

            Console.ReadKey();
        }

        private static void CreateXML()
        {
            var document = new XDocument();

            var comment = new XComment("All the customers who spent less than $1000 and are in Utah");
            document.Add(comment);

            var rootElement = new XElement("Customers",
                from customer in customers
                where customer.Price < 1000 && customer.State == "UT"
                select new XElement("Customer",
                new XAttribute("ID", customer.ID),
                new XElement("First_Name", customer.First),
                new XElement("Last_Name", customer.Last),
                new XElement("State", customer.State),
                new XElement("Ticket_Price", customer.Price),
                new XElement("Items",
                from item in customer.Purchases
                select new XElement("Item", item)))
                );

            document.Add(rootElement);
            document.Save("../../../CustomerLinqQuery.xml");
        }

        private static void QueryXML()
        {
            var document = XDocument.Load("../../../CustomerLinqQuery.xml");

            try
            {
                var query = from element in document.Element("Customers").Elements("Customer")
                            select new
                            {
                                FirstName = element.Element("First_Name").Value,
                                LastName = element.Element("Last_Name").Value,
                                ID = element.Attribute("ID").Value,
                                State = element.Element("State").Value,
                                TicketPrice = element.Element("Ticket_Price").Value,
                                Items = element.Element("Items").Elements()
                            };

                foreach (var customer in query)
                {
                    Console.WriteLine($"Customer {customer.FirstName} {customer.LastName} fits the query and is in the xml document");
                    Console.WriteLine($"\tID:{customer.ID}, State:{customer.State}, Ticket Price: {customer.TicketPrice}");
                    Console.Write($"\tItems Purchased - (");
                    foreach (var item in customer.Items)
                    {
                        Console.Write($"{item.Value}, ");
                    }
                    Console.Write(")\n");
                }
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
