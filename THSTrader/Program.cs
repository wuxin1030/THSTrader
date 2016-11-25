using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using Nancy;
using System.Diagnostics;
using System.Threading;

namespace THSTrader
{
    class Program : NancyModule
    {
        static THSClient client = new THSClient();

        static void Main(string[] args)
        {
            string ip = "localhost";
            string port = "9000";
            string uri = string.Format(@"http://{0}:{1}", ip, port);

            using (var host = new NancyHost(new Uri(uri)))
            {
                //int hwnd = client.GetWindowsHandle();
                //int pid = -1;
                //Utility.GetWindowThreadProcessId(hwnd, ref pid);

                //client.Buy("511880", 100, 1);
                //Thread.Sleep(2000);
                uint t1 = Utility.GetTickCount();
                for (int i = 0; i < 4; i++)
                {
                    string xx = "";
                    xx = client.Buy_Money("000725", 1000);
                    if (xx != "")
                        ;
                    xx = client.Buy_Level("600401", 100, "b5");
                    if (xx != "")
                        ;
                    xx = client.Sell("601398", 1, 4.5);
                    if (xx != "")
                        ;
                    xx = client.Buy("000725", 100, 2.78);
                    if (xx != "")
                        ;
                    xx = client.Sell_Level("000002", 2, "b5");
                    if (xx != "")
                        ;
                    xx = client.Sell_Percent("510300", 0.1);
                    if (xx != "")
                        ;
                }
                uint t2 = Utility.GetTickCount();
                uint t3 = t2 - t1;

                host.Start();
                Console.WriteLine("Running on " + uri);

                string ipt = "";
                while (ipt.Trim().ToLower() != "exit")
                {
                    ipt = Console.ReadLine();
                }
            }
        }
    }

    public class THSModule : NancyModule
    {
        static private int i = 0;
        //static THSClient client = new THSClient();
        public THSModule()
        {
            Get["/"] = r =>
            {
                //client.hWnd++;
                return Response.AsJson(new { result = true, message = 111 });
            };
        }
    }
}
