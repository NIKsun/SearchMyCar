using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchMyCar
{
    class Program
    {
        static void Main(string[] args)
        {
            MailSender ms = new MailSender("chernik2@gmail.com", "smtp.yandex.ru", "chernuhinnv@yandex.ru", "chernik2", 465);
            ms.SendEmail("Hello world!");
        }
    }
}
