using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SearchMyCar
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Searcher searcher = new Searcher();
            List<Car> cars = new List<Car>();
            int oldCarCount;
            while(true)
            {
                Console.WriteLine("New search {0}", DateTime.Now);
                oldCarCount = cars.Count;
                cars = searcher.search(cars);
                Console.WriteLine("Search sucsessfully. New cars - {0}", cars.Count - oldCarCount);
                Thread.Sleep(1000 * 60 * 5);
            }
        }
    }
}
