﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public string GetMessage()
        {
            if (img.Length == 81)
                img = img.Insert(img.LastIndexOf('/'), "/");
            string result = "<td><a     href=" + this.href + "><img src=" + this.img + "></td><td></a>";
            result += this.message; 
            result += "<br>" + this.date_create.ToString() + " => " + this.date_update.ToString();
            result += "<br>Цена: " + this.price;
            result += "<br>Год: " + this.year;
            result += "<br>Пробег: " + this.mileage ;
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

    class Searcher
    {
        public string buildRequest(int price_to)
        {
            return null;
        }
        Car HandleCar(HtmlNode car)
        {
            Car result = new Car();
            result.ParseAttrs(car.FirstChild.FirstChild.Attributes[2].Value.Split(','));
            result.ParseChilds(car.FirstChild.FirstChild.ChildNodes);            
            return result;
        }
        public List<Car> search(List<Car> cars)
        {
            //Отправляем запрос,где textBox1 - строка с адресом

            System.Net.WebRequest reqGET = System.Net.WebRequest.Create(@"http://auto.ru/cars/vaz/2114/group-hatchback_5d/all/?search%5Bsection_id%5D=0&search%5Bmark%5D%5B0%5D=288&search%5Bmark-folder%5D%5B0%5D=288-3899&search%5Bsalon_id%5D=&search%5Byear%5D%5Bmin%5D=&search%5Byear%5D%5Bmax%5D=&search%5Bprice%5D%5Bmin%5D=&search%5Bprice%5D%5Bmax%5D=&search%5Bengine_volume%5D%5Bmin%5D=&search%5Bengine_volume%5D%5Bmax%5D=&search%5Brun%5D%5Bmax%5D=&search%5Bengine_power%5D%5Bmin%5D=&search%5Bengine_power%5D%5Bmax%5D=&search%5Bcustom%5D=1&search%5Bacceleration%5D%5Bmin%5D=&search%5Bacceleration%5D%5Bmax%5D=&search%5Bstate%5D=1&search%5Bgeo_region%5D=&search%5Bgeo_city%5D=&search%5Bgeo_country%5D=&search%5Bgeo_similar_cities%5D=&search%5Bperiod%5D=0&show_sales=1&sort%5Bcreate_date%5D=desc");
            System.Net.WebResponse resp = reqGET.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8);            
            string html = sr.ReadToEnd();
            stream.Close();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(new StringReader(html));
            
            int i = 1;
            HtmlNode node = doc.DocumentNode.SelectSingleNode(@"/html/body/div[1]/div[3]/article/div[1]/div[1]/div/div[" + i.ToString() + "]");

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
                    foreach(var car in cars)
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
    }
}
