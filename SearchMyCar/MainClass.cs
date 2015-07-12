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
            Searcher searcher = new Searcher();
            MailSender mailSender = new MailSender("chernik2@gmail.com", "smtp.yandex.ru","chernuhinnv@yandex.ru", "chernik2", 465);

            List<Car> cars = new List<Car>();
            int oldCarCount, numberOfNewCars;
            while(true)
            {
                Console.WriteLine("New search {0}", DateTime.Now);
                oldCarCount = cars.Count;
                cars = searcher.search(cars);
                numberOfNewCars = cars.Count - oldCarCount;
                Console.WriteLine("Search sucsessfully. New cars - {0}", numberOfNewCars);
                string message = "You handle ";

                int lastIndex = cars.Count - 1;
                for (int i = 0; i < numberOfNewCars; i++)
                {
                    Car buffer = cars[lastIndex-i];

                    message += (i+1).ToString() + " new cars\r\n";
                    message += "Характеристики тачки:\r\n";
                    message += buffer.GetMessage();
                }

                if (numberOfNewCars != 0)
                {
                    mailSender.SendEmail(message);
                }
                Thread.Sleep(1000 * 60 * 5);
            }
        }
    }
}
