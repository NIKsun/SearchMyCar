using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace SearchMyCar
{
    class Searcher
    {
        public string buildRequest(int price_to)
        {
            return null;
        }
        public string search()
        {
            //Отправляем запрос,где textBox1 - строка с адресом

            System.Net.WebRequest reqGET = System.Net.WebRequest.Create(@"http://auto.ru/cars/chevrolet/lacetti/all/?sort%5Bcreate_date%5D=desc");
            System.Net.WebResponse resp = reqGET.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8);
            string html = sr.ReadToEnd();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(html));
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"/html/body/div[1]/div[3]/article/div[1]/div[1]/div/div[12]");
            foreach (var child in node.ChildNodes)
            {
                if (child.Attributes.Count == 0)
                    continue;
                if (child.Attributes[0].Value == "sales-list-item")
                    Console.WriteLine(child.InnerText);
            }
            return null;
        }
    }
}
