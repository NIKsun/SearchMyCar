using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchMyCar
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Searcher s = new Searcher();
            Console.Write(s.search());
        }
    }
}
