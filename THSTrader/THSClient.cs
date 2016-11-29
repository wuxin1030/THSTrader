using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using System.Windows.Forms;

namespace THSTrader
{
    public class StockHolding
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public long Balance { get; set; }
        public long AvaiBalance { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public double ProfitAndLossRate { get; set; }
        public double ProfitAndLoss { get; set; }
        public double Value { get; set; }
    }

    public class StockCommission
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public double OrderPrice { get; set; }
        public long OrderVol { get; set; }
        public string Time { get; set; }
        public long DealVol { get; set; }
        public long CancelVol { get; set; }
        public double DealPrice { get; set; }
        public string Contract { get; set; }
    }

    //public class StockOrder
    //{
    //    public string Contract { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public string Direction { get; set; }
    //    public long DealVol { get; set; }
    //    public double DealPrice { get; set; }
    //    public double DealValue { get; set; }
    //    public string DealNO { get; set; }
    //}


    //public class StockStatistics
    //{
    //    public double Available { get; set; }
    //    public double StockVol { get; set; }
    //    public double Asset { get; set; }
    //}

    public class THSClient
    {
        static private object locker = new object();

        const int WM_ID_COPY = 57634;
        const int WM_ID_BUY = 161;
        const int WM_ID_SELL = 162;
        const int WM_ID_CANCEL = 163;
        const int WM_ID_HOLD = 165;
        const int WM_ID_ORDER = 167;
        const int WM_ID_COMMIT = 168;
        const int WM_ID_PREORDER = 172;

        public string strMainWindowTitle = "网上股票交易系统5.0";
        public string strSHAccount = "";
        public string strSZAccount = "";

        public bool allowBuyST = false;

        public int hWnd = 0;
        public int hToolbar = 0;
        public int hToolbarMarket = 0;
        public int hToolbarAccount = 0;
        public int hFrame = 0;
        public int hAfxWnd = 0;
        public int hLeft = 0;
        public int hLeftWnd = 0;
        public int hLeftTree = 0;

        public int hBuyWnd = 0;
        public int hBuyCodeEdit = 0;
        public int hBuyPriceEdit = 0;
        public int hBuyAmountEdit = 0;
        public int hBuyBtn = 0;
        public int hBuyNameStatic = 0;
        public int hBuyMaxAmountStatic = 0;
        public int hBQuoteWnd = 0;
        public int hBQuoteB1PriceStatic = 0;
        public int hBQuoteB2PriceStatic = 0;
        public int hBQuoteB3PriceStatic = 0;
        public int hBQuoteB4PriceStatic = 0;
        public int hBQuoteB5PriceStatic = 0;
        public int hBQuoteS1PriceStatic = 0;
        public int hBQuoteS2PriceStatic = 0;
        public int hBQuoteS3PriceStatic = 0;
        public int hBQuoteS4PriceStatic = 0;
        public int hBQuoteS5PriceStatic = 0;
        public int hBQuoteNowPriceStatic = 0;
        public int hBQuoteMaxPriceStatic = 0;
        public int hBQuoteMinPriceStatic = 0;


        public int hSellWnd = 0;
        public int hSellCodeEdit = 0;
        public int hSellPriceEdit = 0;
        public int hSellAmountEdit = 0;
        public int hSellBtn = 0;
        public int hSellNameStatic = 0;
        public int hSellMaxAmountStatic = 0;
        public int hSQuoteWnd = 0;
        public int hSQuoteB1PriceStatic = 0;
        public int hSQuoteB2PriceStatic = 0;
        public int hSQuoteB3PriceStatic = 0;
        public int hSQuoteB4PriceStatic = 0;
        public int hSQuoteB5PriceStatic = 0;
        public int hSQuoteS1PriceStatic = 0;
        public int hSQuoteS2PriceStatic = 0;
        public int hSQuoteS3PriceStatic = 0;
        public int hSQuoteS4PriceStatic = 0;
        public int hSQuoteS5PriceStatic = 0;
        public int hSQuoteNowPriceStatic = 0;
        public int hSQuoteMaxPriceStatic = 0;
        public int hSQuoteMinPriceStatic = 0;

        public int hHoldWnd = 0;
        public int hAvailable = 0;
        public int hStockValue = 0;
        public int hAsset = 0;
        public int hHoldGrid = 0;

        public int hOrderWnd = 0;
        public int hOrderGrid = 0;
        public int hCommitWnd = 0;
        public int hCommitGrid = 0;

        public int hWithdrawWnd = 0;
        public int hWithdrawCodeEdit = 0;
        public int hWithdrawQueryBtn = 0;
        public int hWithdrawDealSelectedBtn = 0;
        public int hWithdrawDealAllBtn = 0;
        public int hWithdrawGrid = 0;

        System.Configuration.Configuration config;

        public THSClient()
        {
            config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            strMainWindowTitle = config.AppSettings.Settings["MainWindowTitle"].Value.ToString();
            strSHAccount = config.AppSettings.Settings["SHAccount"].Value.ToString();
            strSZAccount = config.AppSettings.Settings["SZAccount"].Value.ToString();

            StringBuilder stringBuilder = new StringBuilder(256);
            while ((hWnd = Utility.FindWindowEx(0, hWnd, null, strMainWindowTitle)) > 0)
            {
                List<int> hList = Utility.FindWindowRe(hWnd, 0, "ToolbarWindow32", null, 10);
                hToolbar = hList.First();
                hToolbarMarket = Utility.GetDlgItem(Utility.FindWindowEx(hToolbar, 0, "#32770", null), 0x3EB);
                hToolbarAccount = Utility.GetDlgItem(Utility.FindWindowEx(hToolbar, 0, "#32770", null), 0x3EC);

                string text = Utility.getItemText(hToolbarAccount);
                if (text == strSHAccount || text == strSZAccount)
                {
                    int outResult = 0;
                    string currentCaption = "";

                    hFrame = Utility.GetDlgItem(hWnd, 0xE900);
                    hAfxWnd = Utility.GetDlgItem(hFrame, 0xE900);
                    hLeft = Utility.GetDlgItem(hAfxWnd, 0x81);
                    hLeftWnd = Utility.GetDlgItem(hLeft, 0xC8);
                    hLeftTree = Utility.GetDlgItem(hLeftWnd, 0x81);

                    Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F3, 0);
                    hWithdrawWnd = WaitForDialog(hFrame, new string[] { "在委托记录上用鼠标双击或回车即可撤单" }, ref currentCaption);
                    if (hWithdrawWnd <= 0)
                        throw new Exception("无法定位撤单对话框");

                    hWithdrawCodeEdit = Utility.GetDlgItem(hWithdrawWnd, 0xD14);
                    hWithdrawQueryBtn = Utility.GetDlgItem(hWithdrawWnd, 0xD15);
                    hWithdrawDealSelectedBtn = Utility.GetDlgItem(hWithdrawWnd, 0x44B);
                    hWithdrawDealAllBtn = Utility.GetDlgItem(hWithdrawWnd, 0x7531);

                    if (!Utility.getItemText(hWithdrawQueryBtn).Contains("查询代码")
                        || !Utility.getItemText(hWithdrawDealSelectedBtn).Contains("撤单")
                        || !Utility.getItemText(hWithdrawDealAllBtn).Contains("全撤"))
                        throw new Exception("无法定位撤单按钮");

                    hWithdrawGrid = Utility.GetDlgItem(
                        Utility.GetDlgItem(
                            Utility.GetDlgItem(hWithdrawWnd, 0x417)
                            , 0xC8)
                        , 0x417);//表格控件嵌套在两层HexinWnd之内

                    return;
                }
            }

            throw new Exception("未找到对应的THS窗口。");
        }

        public List<StockHolding> GetHoldingDetail()
        {
            string[] strColumnTitle = new string[1];
            string[][] strItems = new string[1][];

            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                if (GetStockDetail(ref strColumnTitle, ref strItems))
                {
                    return DecodeStockHolding(strColumnTitle, strItems);
                }

                Thread.Sleep(100);
            }

            return null;
        }
        public List<StockCommission> GetCommissionDetail()
        {
            string[] strColumnTitle = new string[1];
            string[][] strItems = new string[1][];

            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                if (GetCommissionDetail(ref strColumnTitle, ref strItems))
                {
                    return DecodeCommission(strColumnTitle, strItems);
                }

                Thread.Sleep(100);
            }

            return null;
        }
        //public List<StockOrder> GetOrderDetail()
        //{
        //    string[] strColumnTitle = new string[1];
        //    string[][] strItems = new string[1][];

        //    uint oldTick = Utility.GetTickCount();

        //    while (Utility.GetTickCount() - oldTick < Utility.timeout)
        //    {
        //        if (GetOrderDetail(ref strColumnTitle, ref strItems))
        //        {
        //            return DecodeOrder(strColumnTitle, strItems);
        //        }

        //        Thread.Sleep(100);
        //    }

        //    return null;
        //}

        //public bool BatchSellStock(int nPercent)
        //{
        //    List<StockHolding> listStockHoldings = GetStockDetail();

        //    if (listStockHoldings != null)
        //    {
        //        foreach (StockHolding holding in listStockHoldings)
        //        {
        //            long num = holding.Balance * nPercent / 100;

        //            num = Math.Min(num, holding.AvaiBalance);

        //            SellStock(holding.Code, num, -1);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("无法获得当前的持仓信息");
        //    }

        //    return true;
        //}


        public bool GetAccountStat(ref double available,
            ref double stockValue, ref double asset)
        {
            string currentCaption = "";
            int outResult = 0;

            lock (locker)
            {
                Utility.SendMessageTimeout(hWnd, Utility.WM_COMMAND, WM_ID_HOLD, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResult);
                hHoldWnd = WaitForDialog(hFrame, new string[] { "可用金额" }, ref currentCaption);
                if (hHoldWnd <= 0)
                    throw new Exception("无法定位持仓对话框");

                hAvailable = Utility.GetDlgItem(hHoldWnd, 0x3F8);
                hStockValue = Utility.GetDlgItem(hHoldWnd, 0x3F6);
                hAsset = Utility.GetDlgItem(hHoldWnd, 0x3F7);
                //hHoldGrid = Utility.GetDlgItem(
                //    Utility.GetDlgItem(
                //        Utility.GetDlgItem(hHoldWnd, 0x417)
                //        , 0xC8)
                //    , 0x417);//表格控件嵌套在两层HexinWnd之内

                if (!WaitItemIsNotEmpty(hAvailable))
                    throw new Exception("无法读取可用金额数据");

                try
                {
                    available = double.Parse(Utility.getItemText(hAvailable));
                    stockValue = double.Parse(Utility.getItemText(hStockValue));
                    asset = double.Parse(Utility.getItemText(hAsset));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        private double GetNearlyBQuote(string level)
        {
            level = level.ToLower();

            switch (level)
            {
                case "b5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteB5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("b4");
                    }
                case "b4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteB4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("b3");
                    }
                case "b3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteB3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("b2");
                    }
                case "b2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteB2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("b1");
                    }
                case "b1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteB1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("now");
                    }
                case "s5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteS5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("s4");
                    }
                case "s4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteS4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("s3");
                    }
                case "s3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteS3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("s2");
                    }
                case "s2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteS2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("s1");
                    }
                case "s1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hBQuoteS1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyBQuote("now");
                    }
                default:
                    return double.Parse(Utility.getItemText(hBQuoteNowPriceStatic));
            }
        }
        private double GetNearlySQuote(string level)
        {
            level = level.ToLower();

            switch (level)
            {
                case "b5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteB5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("b4");
                    }
                case "b4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteB4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("b3");
                    }
                case "b3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteB3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("b2");
                    }
                case "b2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteB2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("b1");
                    }
                case "b1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteB1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("now");
                    }
                case "s5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteS5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("s4");
                    }
                case "s4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteS4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("s3");
                    }
                case "s3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteS3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("s2");
                    }
                case "s2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteS2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("s1");
                    }
                case "s1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hSQuoteS1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlySQuote("now");
                    }
                default:
                    return double.Parse(Utility.getItemText(hSQuoteNowPriceStatic));
            }
        }

        private void LoadBuyControl()
        {
            string currentCaption = "";

            Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F1, 0);
            hBuyWnd = WaitForDialog(hFrame, new string[] { "买入股票" }, ref currentCaption);
            if (hBuyWnd <= 0)
                throw new Exception("无法定位买入对话框");

            hBuyCodeEdit = Utility.GetDlgItem(hBuyWnd, 0x408);
            hBuyPriceEdit = Utility.GetDlgItem(hBuyWnd, 0x409);
            hBuyAmountEdit = Utility.GetDlgItem(hBuyWnd, 0x40A);
            hBuyBtn = Utility.GetDlgItem(hBuyWnd, 0x3EE);
            hBuyNameStatic = Utility.GetDlgItem(hBuyWnd, 0x40C);
            hBuyMaxAmountStatic = Utility.GetDlgItem(hBuyWnd, 0x3FA);
            hBQuoteWnd = FindDialog(hBuyWnd, new string[] { "涨停", "跌停" }, ref currentCaption);
            hBQuoteB1PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x3FA);
            hBQuoteB2PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x401);
            hBQuoteB3PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x402);
            hBQuoteB4PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x40B);
            hBQuoteB5PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x40C);
            hBQuoteS1PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x3FD);
            hBQuoteS2PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x3Fe);
            hBQuoteS3PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x3FF);
            hBQuoteS4PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x409);
            hBQuoteS5PriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x408);
            hBQuoteNowPriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x400);
            hBQuoteMaxPriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x404);
            hBQuoteMinPriceStatic = Utility.GetDlgItem(hBQuoteWnd, 0x405);
        }
        public string Buy_Money(string code, double money)
        {
            string level = "s5";//"s3";
            
            int msgResult = 0;

            lock (locker)
            {
                LoadBuyControl();
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";

                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";

                if (!WaitItemIsNotEmpty(hBuyNameStatic))
                    return "ERR:无法获取股票名称";
                string s = Utility.getItemText(hBuyNameStatic).ToLower();
                if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                    return "ERR:禁止买进*/s/st个股";

                double price = GetNearlyBQuote(level);
                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";

                long num = (long)Math.Round(money / price);
                num = num / 100 * 100;
                Utility.SendMessageTimeout(hBuyAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hBuyBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hBuyWnd);
                Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hBuyBtn);
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }
        public string Buy_Level(string code, long num, string level)
        {
            level = level.ToLower();
            int msgResult = 0;

            lock (locker)
            {
                LoadBuyControl();
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";

                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";

                if (!WaitItemIsNotEmpty(hBuyNameStatic))
                    return "ERR:无法获取股票名称";
                string s = Utility.getItemText(hBuyNameStatic).ToLower();
                if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                    return "ERR:禁止买进*/s/st个股";

                double price = GetNearlyBQuote(level);
                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";

                num = num / 100 * 100;
                Utility.SendMessageTimeout(hBuyAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hBuyBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hBuyWnd);
                Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hBuyBtn);
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }
        public string Buy(string code, long num, double price)
        {
            int msgResult = 0;

            lock (locker)
            {
                LoadBuyControl();
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";


                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hBQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";
                if (!WaitItemIsEqual(hBuyPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";


                if (!WaitItemIsNotEmpty(hBuyNameStatic))
                    return "ERR:无法获取股票名称";
                string s = Utility.getItemText(hBuyNameStatic).ToLower();
                if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                    return "ERR:禁止买进*/s/st个股";

                num = num / 100 * 100;
                Utility.SendMessageTimeout(hBuyAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hBuyAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hBuyBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hBuyWnd);
                Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hBuyBtn);
                Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }

        private void LoadSellControl()
        {
            string currentCaption = "";

            Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F2, 0);
            hSellWnd = WaitForDialog(hFrame, new string[] { "卖出股票" }, ref currentCaption);
            if (hSellWnd <= 0)
                throw new Exception("无法定位卖出对话框");

            hSellCodeEdit = Utility.GetDlgItem(hSellWnd, 0x408);
            hSellPriceEdit = Utility.GetDlgItem(hSellWnd, 0x409);
            hSellAmountEdit = Utility.GetDlgItem(hSellWnd, 0x40A);
            hSellBtn = Utility.GetDlgItem(hSellWnd, 0x3EE);
            hSellNameStatic = Utility.GetDlgItem(hSellWnd, 0x40C);
            hSellMaxAmountStatic = Utility.GetDlgItem(hSellWnd, 0x40E);
            hSQuoteWnd = FindDialog(hSellWnd, new string[] { "涨停", "跌停" }, ref currentCaption);
            hSQuoteB1PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x3FA);
            hSQuoteB2PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x401);
            hSQuoteB3PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x402);
            hSQuoteB4PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x40B);
            hSQuoteB5PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x40C);
            hSQuoteS1PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x3FD);
            hSQuoteS2PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x3Fe);
            hSQuoteS3PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x3FF);
            hSQuoteS4PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x409);
            hSQuoteS5PriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x408);
            hSQuoteNowPriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x400);
            hSQuoteMaxPriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x404);
            hSQuoteMinPriceStatic = Utility.GetDlgItem(hSQuoteWnd, 0x405);
        }
        public string Sell_Percent(string code, double percent)
        {
            string level = "b5";
            int msgResult = 0;

            lock (locker)
            {
                LoadSellControl();
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "11", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";


                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellCodeEdit, code) || !WaitItemIsNotEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";

                double price = GetNearlySQuote(level);
                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";

                long maxAmount = 0;
                maxAmount = long.Parse(Utility.getItemText(hSellMaxAmountStatic));
                long num = (long)Math.Floor((double)maxAmount * percent);
                Utility.SendMessageTimeout(hSellAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hSellBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hSellWnd);
                Utility.SendMessage(hSellBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hSellBtn);
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }
        public string Sell_Level(string code, long num, string level)
        {
            level = level.ToLower();
            int msgResult = 0;

            lock (locker)
            {
                LoadSellControl();
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "11", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";


                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellCodeEdit, code) || !WaitItemIsNotEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";

                double price = GetNearlySQuote(level);
                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";

                long maxAmount = 0;
                maxAmount = long.Parse(Utility.getItemText(hSellMaxAmountStatic));
                num = Math.Min(num, maxAmount);
                Utility.SendMessageTimeout(hSellAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hSellBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hSellWnd);
                Utility.SendMessage(hSellBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hSellBtn);
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }
        public string Sell(string code, long num, double price)
        {
            int msgResult = 0;

            lock (locker)
            {
                LoadSellControl();
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "11", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);

                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法重置输入编辑框";


                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                Utility.SendMessageTimeout(hSellPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellCodeEdit, code) || !WaitItemIsNotEqual(hSQuoteNowPriceStatic, "-"))
                    return "ERR:无法正确设置代码";
                if (!WaitItemIsEqual(hSellPriceEdit, price.ToString("F2")))
                    return "ERR:无法正确设置价格";

                long maxAmount = 0;
                maxAmount = long.Parse(Utility.getItemText(hSellMaxAmountStatic));
                num = Math.Min(num, maxAmount);
                Utility.SendMessageTimeout(hSellAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                if (!WaitItemIsEqual(hSellAmountEdit, num.ToString()))
                    return "ERR:无法正确设置数量";

                if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
                {
                    if (!WaitItemContains(hToolbarMarket, "深圳"))
                        return "ERR:无法正确选择市场";
                }
                else
                {
                    if (!WaitItemContains(hToolbarMarket, "上海"))
                        return "ERR:无法正确选择市场";
                }

                if (!WaitWindowEnable(hSellBtn))
                    return "ERR:无法点击按钮";
                Utility.SetActiveWindow(hSellWnd);
                Utility.SendMessage(hSellBtn, Utility.WM_CLICK, 0, 0);

                WaitWindowEnable(hSellBtn);
                Utility.SendMessageTimeout(hSellCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
                return "";
            }
        }

        #region Helper
        private bool GetStockDetail(ref string[] strColumnHeader,
            ref string[][] strItems)
        {
            string currentCaption = "";
            int outResult = 0;

            lock (locker)
            {
                try
                {
                    Utility.SendMessageTimeout(hWnd, Utility.WM_COMMAND, WM_ID_HOLD, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResult);
                    hHoldWnd = WaitForDialog(hFrame, new string[] { "可用金额" }, ref currentCaption);
                    if (hHoldWnd <= 0)
                        throw new Exception("无法定位持仓对话框");

                    hHoldGrid = Utility.GetDlgItem(
                        Utility.GetDlgItem(
                            Utility.GetDlgItem(hHoldWnd, 0x417)
                            , 0xC8)
                        , 0x417);//表格控件嵌套在两层HexinWnd之内

                    ClipboardHelper.Save();
                    Clipboard.Clear();
                    int outResule = 0;
                    Utility.SendMessageTimeout(hHoldGrid, (uint)Utility.WM_COMMAND, WM_ID_COPY, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResule);
                    string cbText = Clipboard.GetText(TextDataFormat.UnicodeText);
                    ClipboardHelper.Restore();

                    string[] strLines = cbText.Split(new string[] { "\r\n" },
                        StringSplitOptions.RemoveEmptyEntries);

                    strColumnHeader = strLines[0].Split(new string[] { "\t" },
                        StringSplitOptions.RemoveEmptyEntries);

                    string[][] items = new string[strLines.Length - 1][];

                    for (int i = 1; i < strLines.Length; i++)
                    {
                        items[i - 1] = strLines[i].Split(new string[] { "\t" },
                            StringSplitOptions.RemoveEmptyEntries);
                    }

                    strItems = items;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        private bool GetCommissionDetail(ref string[] strColumnHeader,
            ref string[][] strItems)
        {
            string currentCaption = "";
            int outResult = 0;

            lock (locker)
            {
                try
                {
                    Utility.SendMessageTimeout(hWnd, Utility.WM_COMMAND, WM_ID_COMMIT, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResult);
                    hCommitWnd = WaitForDialog(hFrame, new string[] { "合同编号" }, ref currentCaption);
                    if (hCommitWnd <= 0)
                        throw new Exception("无法定位当日委托对话框");

                    hCommitGrid = Utility.GetDlgItem(
                        Utility.GetDlgItem(
                            Utility.GetDlgItem(hCommitWnd, 0x417)
                            , 0xC8)
                        , 0x417);//表格控件嵌套在两层HexinWnd之内

                    ClipboardHelper.Save();
                    Clipboard.Clear();
                    int outResule = 0;
                    Utility.SendMessageTimeout(hCommitGrid, (uint)Utility.WM_COMMAND, WM_ID_COPY, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResule);
                    string cbText = Clipboard.GetText(TextDataFormat.UnicodeText);
                    ClipboardHelper.Restore();

                    string[] strLines = cbText.Split(new string[] { "\r\n" },
                        StringSplitOptions.RemoveEmptyEntries);

                    strColumnHeader = strLines[0].Split(new string[] { "\t" },
                        StringSplitOptions.RemoveEmptyEntries);

                    string[][] items = new string[strLines.Length - 1][];

                    for (int i = 1; i < strLines.Length; i++)
                    {
                        items[i - 1] = strLines[i].Split(new string[] { "\t" },
                            StringSplitOptions.RemoveEmptyEntries);
                    }

                    strItems = items;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        //private bool GetOrderDetail(ref string[] strColumnHeader,
        //    ref string[][] strItems)
        //{
        //    string currentCaption = "";
        //    int outResult = 0;

        //    lock (locker)
        //    {
        //        try
        //        {
        //            Utility.SendMessageTimeout(hWnd, Utility.WM_COMMAND, WM_ID_ORDER, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResult);
        //            hOrderWnd = WaitForDialog(hFrame, new string[] { "合同编号" }, ref currentCaption);
        //            if (hOrderWnd <= 0)
        //                throw new Exception("无法定位当日成交对话框");

        //            hOrderGrid = Utility.GetDlgItem(
        //                Utility.GetDlgItem(
        //                    Utility.GetDlgItem(hOrderWnd, 0x417)
        //                    , 0xC8)
        //                , 0x417);//表格控件嵌套在两层HexinWnd之内

        //            ClipboardHelper.Save();
        //            Clipboard.Clear();
        //            int outResule = 0;
        //            Utility.SendMessageTimeout(hOrderGrid, (uint)Utility.WM_COMMAND, WM_ID_COPY, 0, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out outResule);
        //            string cbText = Clipboard.GetText(TextDataFormat.UnicodeText);
        //            ClipboardHelper.Restore();

        //            string[] strLines = cbText.Split(new string[] { "\r\n" },
        //                StringSplitOptions.RemoveEmptyEntries);

        //            strColumnHeader = strLines[0].Split(new string[] { "\t" },
        //                StringSplitOptions.RemoveEmptyEntries);

        //            string[][] items = new string[strLines.Length - 1][];

        //            for (int i = 1; i < strLines.Length; i++)
        //            {
        //                items[i - 1] = strLines[i].Split(new string[] { "\t" },
        //                    StringSplitOptions.RemoveEmptyEntries);
        //            }

        //            strItems = items;
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}
        
        private List<StockHolding> DecodeStockHolding(string[] strColumnHeader,
            string[][] strItems)
        {
            List<StockHolding> listStockHoldings = new List<StockHolding>();

            for (int i = 0; i < strItems.Length; i++)
                listStockHoldings.Add(new StockHolding());

            for (int i = 0; i < strColumnHeader.Length; i++)
            {
                if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Code"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Code = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Name"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Name = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Balance"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Balance = (long)double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_AvaiBalance"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].AvaiBalance = (long)double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Cost"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Cost = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Price"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Price = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_ProfitAndLossRate"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].ProfitAndLossRate = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_ProfitAndLoss"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].ProfitAndLoss = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Value"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockHoldings[j].Value = double.Parse(strItems[j][i]);
                }
            }

            return listStockHoldings;
        }
        private List<StockCommission> DecodeCommission(string[] strColumnHeader,
            string[][] strItems)
        {
            List<StockCommission> listStockCommissions = new List<StockCommission>();

            for (int i = 0; i < strItems.Length; i++)
                listStockCommissions.Add(new StockCommission());

            for (int i = 0; i < strColumnHeader.Length; i++)
            {
                if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Time"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].Time = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Code"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].Code = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Name"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].Name = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Direction"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].Direction = strItems[j][i];
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_OrderPrice"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].OrderPrice = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_OrderVol"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].OrderVol = (long)double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_DealVol"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].DealVol = (long)double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_CancelVol"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].CancelVol = (long)double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_DealPrice"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].DealPrice = double.Parse(strItems[j][i]);
                }
                else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Contract"].Value.ToString())
                {
                    for (int j = 0; j < strItems.Length; j++)
                        listStockCommissions[j].Contract = strItems[j][i];
                }
            }

            return listStockCommissions;
        }
        //private List<StockOrder> DecodeOrder(string[] strColumnHeader,
        //    string[][] strItems)
        //{
        //    List<StockOrder> listStockOrders = new List<StockOrder>();

        //    for (int i = 0; i < strItems.Length; i++)
        //        listStockOrders.Add(new StockOrder());

        //    for (int i = 0; i < strColumnHeader.Length; i++)
        //    {
        //        if (strColumnHeader[i] == config.AppSettings.Settings["Order_Contract"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].Contract = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_Code"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].Code = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_Name"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].Name = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_Direction"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].Direction = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_DealVol"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].DealVol = (long)double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_DealPrice"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].DealPrice = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_DealValue"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].DealValue = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Order_DealNO"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockOrders[j].DealNO = strItems[j][i];
        //        }
        //    }

        //    return listStockOrders;
        //}

        private bool WaitWindowEnable(int hWnd)
        {
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                if (Utility.IsWindowEnabled(hWnd))
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemIsNotEmpty(int hItem)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();
                if (strCaption.Trim() != "")
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemIsEmpty(int hItem)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();

                if (strCaption.Trim() == "")
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemIsEqual(int hItem, string ipt)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();

                if (strCaption.Trim() == ipt)
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemIsNotEqual(int hItem, string ipt)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();

                if (strCaption.Trim() != ipt)
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemContains(int hItem, string ipt)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();

                if (strCaption.Trim().Contains(ipt))
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private bool WaitItemDontContain(int hItem, string ipt)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                Utility.SendMessage(hItem, Utility.WM_GETTEXT, 256, stringBuilder);

                string strCaption = stringBuilder.ToString();

                if (!strCaption.Trim().Contains(ipt))
                    return true;

                Thread.Sleep(100);
            }

            return false;
        }
        private int FindDialog(int hParent, string[] captionArr, ref string currentCaption)
        {
            int hDialog = 0;
            StringBuilder stringBuilder = new StringBuilder(256);

            while ((hDialog = Utility.FindWindowEx(hParent, hDialog, "#32770", null)) > 0)
            {
                int hStatic = 0;
                while ((hStatic = Utility.FindWindowEx(hDialog, hStatic, "Static", null)) > 0)
                {
                    Utility.SendMessage(hStatic, Utility.WM_GETTEXT, 256, stringBuilder);
                    string strCaption = stringBuilder.ToString();

                    foreach (string caption in captionArr)
                    {
                        if (strCaption == caption)
                        {
                            currentCaption = caption;
                            return hDialog;
                        }
                    }
                }
            }

            return 0;
        }
        private int WaitForDialog(int hParent, string[] captionArr, ref string currentCaption)
        {
            uint oldTick = Utility.GetTickCount();

            while (Utility.GetTickCount() - oldTick < Utility.timeout)
            {
                int hDialog = 0;

                if ((hDialog = FindDialog(hParent, captionArr, ref currentCaption)) > 0)
                    return hDialog;

                Thread.Sleep(100);
            }

            return 0;
        }
        #endregion

    }
}
