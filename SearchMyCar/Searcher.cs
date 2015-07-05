using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace SearchMyCar
{
    class Searcher
    {
        public string search()
        {
            //Отправляем запрос,где textBox1 - строка с адресом

            System.Net.WebRequest reqGET = System.Net.WebRequest.Create(@"https://auto.yandex.ru/offers?price_to=200000&sort_offers=cr_date-desc&in_stock=true&year_from=2008&mark=chevrolet&climate=1&airbag=1");
            System.Net.WebResponse resp = reqGET.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8);
            string html = sr.ReadToEnd();
            var file = File.Open(@"E:\1.html", FileMode.Create);
            file.Write(Encoding.Unicode.GetBytes(html), 0, html.Length);
            file.Close();
            HtmlDocument doc = new HtmlDocument();           

            doc.OptionFixNestedTags = true; //Опционально, если требуется

            doc.Load(new StringReader(html));
            HtmlNode node = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/table/tr/td[1]/div/div/div[2]/div[1]/div/div/table/tr[4]/td[4]/div[2]/text()");   
            return node.OuterHtml;
        }
    }
}
