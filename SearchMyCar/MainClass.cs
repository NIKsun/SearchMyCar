using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SearchMyCar
{
    class MainClass
    {
        static void Main(string[] args)
        {
            MailSender mailSender = new MailSender("chernik2@gmail.com", "smtp.yandex.ru","chernuhinnv@yandex.ru", "chernik2", 465);

            List<Car> cars = new List<Car>();
            List<Searcher> crawlers = new List<Searcher>();

            string request;
            while ((request = Console.ReadLine()) != "")
                crawlers.Add(new Searcher(request));

            int oldCarCount, numberOfNewCars;
            while(true)
            {
                Console.WriteLine("New search {0}", DateTime.Now);
                oldCarCount = cars.Count;
                foreach(var crawler in crawlers)
                    cars = crawler.search(cars);
                numberOfNewCars = cars.Count - oldCarCount;
                Console.WriteLine("Search sucsessfully. New cars - {0}", numberOfNewCars);
                string message = "<html><table border=2>";

                for (int i = 0; i < numberOfNewCars; i++)
                {
                    Car buffer = cars[cars.Count - numberOfNewCars + i];
                    message += "<tr>";
                    message += buffer.GetMessage();
                    message += "</tr>";
                }
                message += "</table></html>";

                if (numberOfNewCars != 0)
                {
                    mailSender.SendEmail(message);
                }
                Thread.Sleep(1000 * 60 * 5);
            }
        }
    }
}
