using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchMyCar
{
    class HandlerRequest
    {
        private string IPaddress;
        private string port;
        public HandlerRequest(string IPaddress, string port)
        {
            this.IPaddress = IPaddress;
            this.port = port;
        }
        static void HandleRequest(object socket)
        {
            Socket connection = (Socket)socket;
            byte[] bytes = new byte[4096];
            int bytesRec;
            try
            {
                bytesRec = connection.Receive(bytes);
                string request = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                Searcher searcher = new Searcher(request);
                List<Car> cars = new List<Car>();
                searcher.Search(cars);

                string reply = null;
                /*foreach (var car in cars)
                {
                    reply += car.AndroidMessage() + "  ";
                } */
                reply = cars.First().AndroidMessage();
                byte[] msg = Encoding.UTF8.GetBytes(reply);
                connection.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }
        }

        public void start()
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(IPaddress);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(port));
                Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sListener.Bind(ipEndPoint);
                sListener.Listen(1024);
                Console.WriteLine("Прослушиваем порт {0} по адресу {1}", ipEndPoint.Port, ipEndPoint.Address.ToString());
                Thread[] threads = new Thread[64];
                for (int i = 0; i < 64; i++)
                    threads[i] = new Thread(HandleRequest);
                while (true)
                {
                    Socket connection = sListener.Accept();
                    for (int i = 0; i < 64; i++)
                    {
                        if (threads[i].ThreadState == ThreadState.Unstarted)
                        {
                            threads[i].Start(connection);
                            break;
                        }
                        if (threads[i].ThreadState == ThreadState.Stopped)
                            threads[i] = new Thread(HandleRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
