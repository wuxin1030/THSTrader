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

        [STAThread]
        static void Main(string[] args)
        {
            string ip = "localhost";
            string port = "9000";
            string uri = string.Format(@"http://{0}:{1}", ip, port);

            using (var host = new NancyHost(new Uri(uri)))
            {

                double a = 0, b = 0, c = 0;
                client.GetAccountStat(ref a, ref b, ref c);

                var xxx1 = client.GetHoldingDetail();
                var xxx2 = client.GetCommissionDetail();

                string xxs = client.Cancel("", "all");
                uint t1 = Utility.GetTickCount();
                for (int i = 0; i < 1; i++)
                {
                    string xx = "";
                    xx = client.Buy_Money("000725", 1000);
                    if (xx != "")
                        ;
                    xx = client.Buy_Level("600401", 100, "b5");
                    if (xx != "")
                        ;
                    xx = client.Sell("601398", 1, 4.56);
                    if (xx != "")
                        ;
                    xx = client.Buy("000725", 100, 2.78);
                    if (xx != "")
                        ;
                    xx = client.Sell_Level("000002", 2, "b5");
                    if (xx != "")
                        ;
                    xx = client.Sell_Percent("159915", 0.1);
                    if (xx != "")
                        ;
                }
                uint t2 = Utility.GetTickCount();
                uint t3 = t2 - t1;

                client.GetAccountStat(ref a, ref b, ref c);


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
