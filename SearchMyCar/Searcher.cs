using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace SearchMyCar
{
    

    class Searcher
    {
        string _requset;
        public Searcher(string requset)
        {
            this._requset = requset;
        }
        Car HandleCar(HtmlNode car)
        {
            Car result = new Car();
            result.ParseAttrs(car.FirstChild.FirstChild.Attributes[2].Value.Split(','));
            result.ParseChilds(car.FirstChild.FirstChild.ChildNodes);            
            return result;
        }

        private List<Car> ParseAutoRu(HtmlDocument doc, List<Car> cars)
        {

            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"/html/body/div[1]/div[3]/article/div[1]/div[1]/div/div[1]");
            int i = 1;
            while (node.Attributes == null || node.Attributes[0].Value != "widget widget_theme_white sales-list")
                node = doc.DocumentNode.SelectSingleNode(@"/html/body/div[1]/div[3]/article/div[1]/div[1]/div/div[" + (++i).ToString() + "]");

            bool isNewCar;
            foreach (HtmlNode child in node.ChildNodes)
            {
                if (child.Attributes.Count == 0)
                    continue;
                if (child.Attributes[0].Value == "sales-list-item")
                {
                    isNewCar = true;
                    Car buffer = HandleCar(child);
                    foreach (var car in cars)
                    {
                        if (car.carId == buffer.carId)
                        {
                            isNewCar = false;
                            break;
                        }
                    }
                    if (isNewCar)
                        cars.Add(buffer);
                }
            }
            return cars;
        }
        private string GetHtml()
        {
            System.Net.WebRequest reqGET = System.Net.WebRequest.Create(this._requset);
            System.Net.WebResponse resp = reqGET.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8);
            string html = sr.ReadToEnd();
            if (stream != null)
                stream.Close();
            else
                return null;
            return html;
        }
        public List<Car> Search(int lastCarID)
        {
            List<Car> result = new List<Car>();
            string html = GetHtml();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(html));
            result = ParseAutoRu(doc, result);
            int i = result.Count;
            while (i != -1 && result[i].carId != lastCarID)
                i--;
            if (i != 0)
                result.RemoveRange(i, result.Count - i);
            return result;
        }
        public List<Car> Search(List<Car> cars)
        {
            string html = GetHtml();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(html));
            return ParseAutoRu(doc, cars);
        }
    }
}
