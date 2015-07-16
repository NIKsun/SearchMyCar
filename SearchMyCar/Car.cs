using System;
using HtmlAgilityPack;

namespace SearchMyCar
{
    class Car
    {
        public int carId;

        string img;
        string href;
        string city;
        string message;
        int price;
        int mileage;
        int year;
        DateTime date_create;
        DateTime date_update;

        public string AndroidMessage()
        {
            string result = this.img + "@@@";
            result += this.message;
            result += "<br>" + this.date_create.ToString() + " => " + this.date_update.ToString();
            result += "<br>Цена: " + this.price;
            result += "<br>Год: " + this.year;
            result += "<br>Пробег: " + this.mileage;
            result += "<br>Город: " + this.city + "</td>";
            return result;
        }
        public string GetMessage()
        {
            if (img.Length == 81)
                img = img.Insert(img.LastIndexOf('/'), "/");
            string result = "<td><a     href=" + this.href + "><img src=" + this.img + "></td><td></a>";
            result += this.message;
            result += "<br>" + this.date_create.ToString() + " => " + this.date_update.ToString();
            result += "<br>Цена: " + this.price;
            result += "<br>Год: " + this.year;
            result += "<br>Пробег: " + this.mileage;
            result += "<br>Город: " + this.city + "</td>";
            return result;
        }
        public void ParseAttrs(string[] Attrs)
        {
            foreach (var attr in Attrs)
            {
                string name = attr.Split(':')[0];
                string val = attr.Split(':')[1];
                if (name.IndexOf("card_id") != -1)
                    this.carId = Int32.Parse(val.Substring(1, val.Length - 2));
                if (name.IndexOf("card_price") != -1)
                    this.price = Int32.Parse(val);
                if (name.IndexOf("card_run") != -1)
                    this.mileage = Int32.Parse(val);
                if (name.IndexOf("card_year") != -1)
                    this.year = Int32.Parse(val.Substring(1, val.Length - 2));
                if (name.IndexOf("card_date_created") != -1)
                {
                    val += ":" + attr.Split(':')[2];
                    this.date_create = DateTime.Parse(val.Substring(1));

                }
                if (name.IndexOf("card_date_updated") != -1)
                {
                    val += ":" + attr.Split(':')[2];
                    this.date_update = DateTime.Parse(val.Substring(1));
                }
            }
        }
        public void ParseChilds(HtmlNodeCollection childs)
        {
            foreach (var child in childs)
            {
                if (child.Attributes[0].Value.IndexOf("images") != -1)
                {
                    this.href = child.FirstChild.Attributes[1].Value;

                    if (child.FirstChild.FirstChild.Attributes[0].Name == "src")
                        this.img = child.FirstChild.FirstChild.Attributes[0].Value;
                    else
                        this.img = child.FirstChild.FirstChild.Attributes[2].Value;

                    if (this.img == @"/i/all7/img/no-photo-thumb.png")
                        this.img = "http://auto.ru/i/all7/img/no-photo-thumb.png width=\"131\" height=\"98\"";
                }
                if (child.Attributes[0].Value.IndexOf("cell_mark_id") != -1)
                {
                    this.message = child.InnerText;
                }
                if (child.Attributes[0].Value.IndexOf("cell_poi_id") != -1)
                {
                    this.city = child.FirstChild.InnerText;
                }
            }
        }
    }
}
