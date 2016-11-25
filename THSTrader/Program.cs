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
                for (int i = 0; i < 10; i++)
                {
                    string xx = client.Buy("601398", 100, 4.01);
                    if (xx != "")
                        ;
                    xx = client.Buy("000002", 300, 24.50);
                    if (xx != "")
                        ;
                    xx = client.Buy("601328", 200, 5.27);
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
        static THSClient client = new THSClient();
        public THSModule()
        {
            Get["/"] = r =>
            {
                client.hWnd++;
                return Response.AsJson(new { result = true, message = client.hWnd });
            };
        }
    }
}
