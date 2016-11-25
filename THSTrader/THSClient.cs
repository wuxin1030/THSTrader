using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

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
        public string Date { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public double OrderPrice { get; set; }
        public long OrderVol { get; set; }
        public string Time { get; set; }
        public long DealVol { get; set; }
        public double DealPrice { get; set; }
        public string Contract { get; set; }
    }

    public class StockOrder
    {
        public string Date { get; set; }
        public string Contract { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Direction { get; set; }
        public long DealVol { get; set; }
        public double DealPrice { get; set; }
        public double DealValue { get; set; }
        public string DealNO { get; set; }

    }


    public class StockStatistics
    {
        public double Available { get; set; }
        public double StockVol { get; set; }
        public double Asset { get; set; }
    }

    public class THSClient
    {
        const int WM_ID_COPY = 57634;
        const int WM_ID_BUY = 161;
        const int WM_ID_SELL = 162;
        const int WM_ID_CANCEL = 163;
        const int WM_ID_HOLD = 165;
        const int WM_ID_DEAL = 167;
        const int WM_ID_COMMIT = 168;
        const int WM_ID_PREORDER = 172;

        public string strMainWindowTitle = "网上股票交易系统5.0";
        public string strSHAccount = "";
        public string strSZAccount = "";

        public bool allowBuyST = false;

        public int hWnd = 0;
        public int hToolbar = 0;
        public int hFrame = 0;
        public int hAfxWnd = 0;
        public int hLeft = 0;
        public int hLeftWnd = 0;
        public int hLeftTree = 0;

        public int hBuySellWnd = 0;
        public int hBuyCodeEdit = 0;
        public int hBuyPriceEdit = 0;
        public int hBuyAmountEdit = 0;
        public int hBuyStrategyCb = 0;
        public int hBuyBtn = 0;
        public int hBuyNameStatic = 0;
        public int hBuyMaxAmountStatic = 0;
        public int hBuyMarketCB = 0;

        public int hSellCodeEdit = 0;
        public int hSellPriceEdit = 0;
        public int hSellAmountEdit = 0;
        public int hSellStrategyCb = 0;
        public int hSellBtn = 0;
        public int hSellNameStatic = 0;
        public int hSellMaxAmountStatic = 0;
        public int hSellMarketCB = 0;

        public int hAvailableBalance = 0;

        public int hQuoteWnd = 0;
        public int hQuoteB1PriceStatic = 0;
        public int hQuoteB2PriceStatic = 0;
        public int hQuoteB3PriceStatic = 0;
        public int hQuoteB4PriceStatic = 0;
        public int hQuoteB5PriceStatic = 0;
        public int hQuoteS1PriceStatic = 0;
        public int hQuoteS2PriceStatic = 0;
        public int hQuoteS3PriceStatic = 0;
        public int hQuoteS4PriceStatic = 0;
        public int hQuoteS5PriceStatic = 0;
        public int hQuoteNowPriceStatic = 0;
        public int hQuoteMaxPriceStatic = 0;
        public int hQuoteMinPriceStatic = 0;

        public int hBuySellGird = 0;

        public int hWithdrawWnd = 0;
        public int hWithdrawCodeEdit = 0;
        public int hWithdrawQueryBtn = 0;
        public int hWithdrawDealSelectedBtn = 0;
        public int hWithdrawDealAllBtn = 0;
        public int hWithdrawGrid = 0;



        private int pId = 0;

        System.Configuration.Configuration config;

        public THSClient()
        {
            config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            strMainWindowTitle = config.AppSettings.Settings["MainWindowTitle"].Value.ToString();
            strSHAccount = config.AppSettings.Settings["SHAccount"].Value.ToString();
            strSZAccount = config.AppSettings.Settings["SZAccount"].Value.ToString();

            //Init

            StringBuilder stringBuilder = new StringBuilder(256);
            while ((hWnd = Utility.FindWindowEx(0, hWnd, null, strMainWindowTitle)) > 0)
            {
                List<int> hList = Utility.FindWindowRe(hWnd, 0, "ToolbarWindow32", null, 10);
                hToolbar = hList.First();

                hList = Utility.FindWindowRe(hToolbar, 0, "ComboBox", null, 10);
                foreach (int h in hList)
                {
                    string text = Utility.getItemText(h);
                    if (text == strSHAccount || text == strSZAccount)
                    {
                        string currentCaption = "";

                        hFrame = Utility.GetDlgItem(hWnd, 0xE900);
                        hAfxWnd = Utility.GetDlgItem(hFrame, 0xE900);
                        hLeft = Utility.GetDlgItem(hAfxWnd, 0x81);
                        hLeftWnd = Utility.GetDlgItem(hLeft, 0xC8);
                        hLeftTree = Utility.GetDlgItem(hLeftWnd, 0xE81);

                        Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F6, 0);
                        hBuySellWnd = WaitForDialog(hFrame, new string[] { "买入股票[F1]", "卖出股票[F2]" }, ref currentCaption);
                        if (hBuySellWnd <= 0)
                            throw new Exception("无法定位买卖对话框");

                        //hBuySellWnd = hDialogWnd;// Utility.GetDlgItem(hFrame, 0xE901);

                        hBuyCodeEdit = Utility.GetDlgItem(hBuySellWnd, 0x408);
                        hBuyPriceEdit = Utility.GetDlgItem(hBuySellWnd, 0x409);
                        hBuyAmountEdit = Utility.GetDlgItem(hBuySellWnd, 0x40A);
                        hBuyStrategyCb = Utility.GetDlgItem(hBuySellWnd, 0x605);
                        hBuyBtn = Utility.GetDlgItem(hBuySellWnd, 0x3EE);
                        hBuyNameStatic = Utility.GetDlgItem(hBuySellWnd, 0x40C);
                        hBuyMaxAmountStatic = Utility.GetDlgItem(hBuySellWnd, 0x3FA);
                        hBuyMarketCB = Utility.GetDlgItem(hBuySellWnd, 0xD7B);

                        hSellCodeEdit = Utility.GetDlgItem(hBuySellWnd, 0x40B);
                        hSellPriceEdit = Utility.GetDlgItem(hBuySellWnd, 0x422);
                        hSellAmountEdit = Utility.GetDlgItem(hBuySellWnd, 0x40F);
                        hSellStrategyCb = Utility.GetDlgItem(hBuySellWnd, 0x606);
                        hSellBtn = Utility.GetDlgItem(hBuySellWnd, 0x3F0);
                        hSellNameStatic = Utility.GetDlgItem(hBuySellWnd, 0x40D);
                        hSellMaxAmountStatic = Utility.GetDlgItem(hBuySellWnd, 0x3FB);
                        hSellMarketCB = Utility.GetDlgItem(hBuySellWnd, 0xD7D);

                        if (!Utility.getItemText(hBuyBtn).Contains("买入")
                            || !Utility.getItemText(hSellBtn).Contains("卖出"))
                            throw new Exception("无法定位买入卖出按钮");


                        hAvailableBalance = Utility.GetDlgItem(hBuySellWnd, 0x40E);//可用余额

                        hQuoteWnd = FindDialog(hBuySellWnd, new string[] { "涨停", "跌停" }, ref currentCaption);
                        hQuoteB1PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x3FA);
                        hQuoteB2PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x401);
                        hQuoteB3PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x402);
                        hQuoteB4PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x40B);
                        hQuoteB5PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x40C);
                        hQuoteS1PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x3FD);
                        hQuoteS2PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x3Fe);
                        hQuoteS3PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x3FF);
                        hQuoteS4PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x409);
                        hQuoteS5PriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x408);
                        hQuoteNowPriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x400);
                        hQuoteMaxPriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x404);
                        hQuoteMinPriceStatic = Utility.GetDlgItem(hQuoteWnd, 0x405);

                        hBuySellGird = Utility.GetDlgItem(
                            Utility.GetDlgItem(
                                Utility.GetDlgItem(hBuySellWnd, 0x417)
                                , 0xC8)
                            , 0x417);//表格控件嵌套在两层HexinWnd之内

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
            }

            throw new Exception("未找到对应的THS窗口。");
        }
        //public List<StockHolding> GetStockDetail()
        //{
        //    string[] strColumnTitle = new string[1];
        //    string[][] strItems = new string[1][];
        //    ///bool bUpdated = false;

        //    if (GetStockDetail(ref strColumnTitle,
        //        ref strItems))///, ref bUpdated))
        //    {
        //        ///if(bUpdated)
        //        return DecodeStockHolding(strColumnTitle, strItems);

        //        ///return null;
        //    }

        //    return null;
        //}
        //public List<StockCommission> GetCommissionDetail()
        //{
        //    string[] strColumnTitle = new string[1];
        //    string[][] strItems = new string[1][];

        //    if (GetCommissionDetail(ref strColumnTitle,
        //        ref strItems))
        //    {
        //        return DecodeCommission(strColumnTitle, strItems);
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

        //public bool Refresh()
        //{
        //    int hWnd = GetWindowsHandle();

        //    if (hWnd <= 0)
        //        return false;

        //    //F5刷新任何当前对话框
        //    Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F5, 0);

        //    return true;
        //}

        //public bool HeartBeat()
        //{
        //    string a = "", b = "", c = "";
        //    return GetStockStatistics(ref a, ref b, ref c);
        //}
        //public bool GetStockStatistics(ref string strAvail,
        //    ref string strStockVol, ref string strAsset)
        //{
        //    int hWnd = GetWindowsHandle();
        //    if (hWnd <= 0)
        //        return false;

        //    Utility.SendMessage(hWnd, Utility.WM_COMMAND, WM_ID_HOLD, 0);

        //    string strCaption;
        //    StringBuilder stringBuilder = new StringBuilder(256);

        //    string currentCaption = "";
        //    int hClientWnd = FindDialog(hWnd, new string[] { "可用金额" }, ref currentCaption);
        //    if (hClientWnd <= 0)
        //        return false;
        //    Thread.Sleep(500); //找到对话框之后充分等待数据传输

        //    // 可用金额
        //    int hChildWnd = Utility.FindWindowEx(hClientWnd, 0, "Static", null);

        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strCaption = stringBuilder.ToString();

        //    if (strCaption != "可用金额")
        //        return false;

        //    // 股票市值
        //    hChildWnd = Utility.FindWindowEx(hClientWnd, hChildWnd, "Static", null);

        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strCaption = stringBuilder.ToString();

        //    if (strCaption != "股票市值")
        //        return false;

        //    // 总 资 产
        //    hChildWnd = Utility.FindWindowEx(hClientWnd, hChildWnd, "Static", null);

        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strCaption = stringBuilder.ToString();

        //    if (strCaption != "总 资 产")
        //        return false;

        //    hChildWnd = Utility.FindWindowEx(hClientWnd, hChildWnd, "Static", null);
        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strAvail = stringBuilder.ToString();

        //    hChildWnd = Utility.FindWindowEx(hClientWnd, hChildWnd, "Static", null);
        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strStockVol = stringBuilder.ToString();

        //    hChildWnd = Utility.FindWindowEx(hClientWnd, hChildWnd, "Static", null);
        //    Utility.GetWindowText(hChildWnd, stringBuilder, 256);
        //    strAsset = stringBuilder.ToString();

        //    return true;
        //}

        //public string SellStock(string strCode, long num, double price)
        //{
        //    if (num <= 0)
        //        return "出售数量小于等于0,忽略.";

        //    string errCode;
        //    int hWnd = GetWindowsHandle();

        //    if (hWnd <= 0)
        //        return "下单软件已经关闭";

        //    Utility.SendMessage(hWnd, Utility.WM_COMMAND, WM_ID_SELL, 0);

        //    int hAfxMDIWnd = Utility.FindWindowEx(hWnd, 0, "AfxMDIFrame42s", null);

        //    if (hAfxMDIWnd <= 0)
        //        return "内部错误-2001";

        //    string currentCaption = "";
        //    int hDialogWnd = WaitForDialog(hAfxMDIWnd, new string[] { "卖出股票" }, ref currentCaption);
        //    if (hDialogWnd <= 0)
        //        return "内部错误-2002";

        //    int hEdit = 0;
        //    hEdit = Utility.FindWindowEx(hDialogWnd, hEdit, "Edit", null);
        //    Utility.SendMessage(hEdit, Utility.WM_SETTEXT, 0, strCode);

        //    Thread.Sleep(500);

        //    hEdit = Utility.FindWindowEx(hDialogWnd, hEdit, "Edit", null);
        //    WaitEditIsNotEmpty(hEdit);

        //    //获取买5到卖5
        //    //0买一价，正数指定价，-1买五价
        //    if (price > 0)
        //        Utility.SendMessage(hEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"));

        //    hEdit = Utility.FindWindowEx(hDialogWnd, hEdit, "Edit", null);
        //    Utility.SendMessage(hEdit, Utility.WM_SETTEXT, 0, num.ToString());

        //    int hButton = FindButton(hDialogWnd, new string[] { "卖出[S]" });
        //    if (hButton <= 0)
        //        return "内部错误-2003";

        //    Utility.PostMessage(hButton, Utility.WM_CLICK, 0, 0);

        //    int hDialog = WaitForDialog(0, new string[] { "委托确认" }, ref currentCaption);
        //    if (hDialog <= 0)
        //        return "内部错误-2004";

        //    hButton = FindButton(hDialog, new string[] { "是(&Y)" });
        //    if (hButton <= 0)
        //        return "内部错误-2005";

        //    Utility.PostMessage(hButton, Utility.WM_CLICK, 0, 0);

        //    hDialog = WaitForDialog(0, new string[] {
        //        "证券可用数量不足", "委托已成功提交", "当前时间不允许委托", "当前时间不允许停牌委托" }, ref currentCaption); ;

        //    if (hDialog <= 0)
        //        return "内部错误-2006";

        //    errCode = currentCaption;

        //    hButton = FindButton(hDialog, new string[] { "确定", "终止" });
        //    if (hButton <= 0)
        //        return "内部错误-2007";

        //    Utility.PostMessage(hButton, Utility.WM_CLICK, 0, 0);

        //    return errCode;
        //}

        private double GetNearlyQuote(string level)
        {
            level = level.ToLower();

            switch (level)
            {
                case "b5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteB5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("b4");
                    }
                case "b4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteB4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("b3");
                    }
                case "b3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteB3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("b2");
                    }
                case "b2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteB2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("b1");
                    }
                case "b1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteB1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("now");
                    }
                case "s5":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteS5PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("s4");
                    }
                case "s4":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteS4PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("s3");
                    }
                case "s3":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteS3PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("s2");
                    }
                case "s2":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteS2PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("s1");
                    }
                case "s1":
                    try
                    {
                        return double.Parse(Utility.getItemText(hQuoteS1PriceStatic));
                    }
                    catch
                    {
                        return GetNearlyQuote("now");
                    }
                default:
                    return double.Parse(Utility.getItemText(hQuoteNowPriceStatic));
            }
        }
        public string Buy_Money(string code, double money)
        {
            string level = "s5";//"s3";
            string currentCaption = "";
            int msgResult = 0;

            Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F6, 0);
            hBuySellWnd = WaitForDialog(hFrame, new string[] { "买入股票[F1]", "卖出股票[F2]" }, ref currentCaption);
            if (hBuySellWnd <= 0)
                return "ERR:无法定位买卖对话框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法重置输入编辑框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法正确设置代码";

            if (!WaitItemIsNotEmpty(hBuyNameStatic))
                return "ERR:无法获取股票名称";
            string s = Utility.getItemText(hBuyNameStatic).ToLower();
            if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                return "ERR:禁止买进*/s/st个股";

            double price = GetNearlyQuote(level);
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
                if (!WaitItemContains(hBuyMarketCB, "深圳"))
                    return "ERR:无法正确选择市场";
            }
            else
            {
                if (!WaitItemContains(hBuyMarketCB, "上海"))
                    return "ERR:无法正确选择市场";
            }

            if (!WaitWindowEnable(hBuyBtn))
                return "ERR:无法点击按钮";
            Utility.SetActiveWindow(hBuySellWnd);
            Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);
            WaitWindowEnable(hBuyBtn);
            return "";
        }

        public string Buy_Level(string code, long num, string level)
        {
            level = level.ToLower();
            string currentCaption = "";
            int msgResult = 0;

            Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F6, 0);
            hBuySellWnd = WaitForDialog(hFrame, new string[] { "买入股票[F1]", "卖出股票[F2]" }, ref currentCaption);
            if (hBuySellWnd <= 0)
                return "ERR:无法定位买卖对话框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法重置输入编辑框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法正确设置代码";

            if (!WaitItemIsNotEmpty(hBuyNameStatic))
                return "ERR:无法获取股票名称";
            string s = Utility.getItemText(hBuyNameStatic).ToLower();
            if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                return "ERR:禁止买进*/s/st个股";

            double price = GetNearlyQuote(level);
            Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyPriceEdit, price.ToString("F2")))
                return "ERR:无法正确设置价格";

            Utility.SendMessageTimeout(hBuyAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyAmountEdit, num.ToString()))
                return "ERR:无法正确设置数量";

            if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
            {
                if (!WaitItemContains(hBuyMarketCB, "深圳"))
                    return "ERR:无法正确选择市场";
            }
            else
            {
                if (!WaitItemContains(hBuyMarketCB, "上海"))
                    return "ERR:无法正确选择市场";
            }

            if (!WaitWindowEnable(hBuyBtn))
                return "ERR:无法点击按钮";
            Utility.SetActiveWindow(hBuySellWnd);
            Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);
            WaitWindowEnable(hBuyBtn);
            return "";
        }

        public string Buy(string code, long num, double price)
        {
            string currentCaption = "";
            int msgResult = 0;

            Utility.PostMessage(hWnd, Utility.WM_KEYDOWN, Utility.VK_F6, 0);
            hBuySellWnd = WaitForDialog(hFrame, new string[] { "买入股票[F1]", "卖出股票[F2]" }, ref currentCaption);
            if (hBuySellWnd <= 0)
                return "ERR:无法定位买卖对话框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, "", SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法重置输入编辑框";

            Utility.SendMessageTimeout(hBuyCodeEdit, Utility.WM_SETTEXT, 0, code, SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            Utility.SendMessageTimeout(hBuyPriceEdit, Utility.WM_SETTEXT, 0, price.ToString("F2"), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyCodeEdit, code) || !WaitItemIsNotEqual(hQuoteNowPriceStatic, "-"))
                return "ERR:无法正确设置代码";
            if (!WaitItemIsEqual(hBuyPriceEdit, price.ToString("F2")))
                return "ERR:无法正确设置价格";


            if (!WaitItemIsNotEmpty(hBuyNameStatic))
                return "ERR:无法获取股票名称";
            string s = Utility.getItemText(hBuyNameStatic).ToLower();
            if (!allowBuyST && (s.Contains("*") || s.Contains("s")))
                return "ERR:禁止买进*/s/st个股";

            Utility.SendMessageTimeout(hBuyAmountEdit, Utility.WM_SETTEXT, 0, num.ToString(), SendMessageTimeoutFlags.SMTO_BLOCK, Utility.timeout, out msgResult);
            if (!WaitItemIsEqual(hBuyAmountEdit, num.ToString()))
                return "ERR:无法正确设置数量";

            if (code[0] == '0' || code[0] == '3' || code.IndexOf("159") == 0) //sz
            {
                if (!WaitItemContains(hBuyMarketCB, "深圳"))
                    return "ERR:无法正确选择市场";
            }
            else
            {
                if (!WaitItemContains(hBuyMarketCB, "上海"))
                    return "ERR:无法正确选择市场";
            }

            if (!WaitWindowEnable(hBuyBtn))
                return "ERR:无法点击按钮";
            Utility.SetActiveWindow(hBuySellWnd);
            Utility.SendMessage(hBuyBtn, Utility.WM_CLICK, 0, 0);
            WaitWindowEnable(hBuyBtn);
            return "";
        }

        //#region Helper
        //private bool GetStockDetail(ref string[] strColumnHeader,
        //    ref string[][] strItems)///, ref bool bUpdated)
        //{
        //    int hWnd = GetWindowsHandle();

        //    if (hWnd <= 0)
        //        return false;

        //    Utility.SendMessage(hWnd, Utility.WM_COMMAND, WM_ID_HOLD, 0);

        //    int hAfxMDIWnd = Utility.FindWindowEx(hWnd, 0, "AfxMDIFrame42s", null);

        //    if (hAfxMDIWnd <= 0)
        //        return false;

        //    int hDialogWnd = 0;
        //    int hScrollWnd = 0;
        //    int hCVirtualGridCtrlWnd = 0;

        //    string currentCaption = "";
        //    if (WaitForDialog(hAfxMDIWnd, new string[] { "资金余额" }, ref currentCaption) == 0)
        //        return false;
        //    Thread.Sleep(500);//找到对话框之后充分等待数据传输

        //    while ((hDialogWnd = Utility.FindWindowVisibleEx(hAfxMDIWnd, hDialogWnd, "#32770", null)) > 0)
        //    {
        //        while ((hScrollWnd = Utility.FindWindowEx(hDialogWnd, hScrollWnd, "Afx:400000:0", null)) > 0)
        //        {
        //            int hScrollWnd2 = Utility.FindWindowEx(hScrollWnd, 0, "AfxWnd42s", null);

        //            if (hScrollWnd2 <= 0)
        //                return false;

        //            hCVirtualGridCtrlWnd = Utility.FindWindowEx(hScrollWnd2, 0, "CVirtualGridCtrl", null);

        //            if (hCVirtualGridCtrlWnd > 0)
        //                break;
        //        }

        //        if (hCVirtualGridCtrlWnd > 0)
        //            break;
        //    }


        //    if (hCVirtualGridCtrlWnd <= 0)
        //        return false;

        //    ClipboardHelper.Save();

        //    Clipboard.Clear();

        //    Utility.SendMessage(hCVirtualGridCtrlWnd, Utility.WM_COMMAND, WM_ID_COPY, 0);

        //    try
        //    {
        //        IDataObject iData = Clipboard.GetDataObject();
        //        ClipboardHelper.Restore();

        //        if (iData.GetDataPresent(DataFormats.Text))
        //        {
        //            string strShareHold = (string)iData.GetData(DataFormats.UnicodeText);

        //            if (strShareHold.Length < 100)
        //                return false;

        //            ////bUpdated = !strShareHold.Equals(mOldStrShareHold);

        //            string[] strLines = strShareHold.Split(new string[] { "\r\n" },
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
        //            ////mOldStrShareHold = strShareHold;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private bool GetCommissionDetail(ref string[] strColumnHeader,
        //    ref string[][] strItems)
        //{
        //    int hWnd = GetWindowsHandle();

        //    if (hWnd <= 0)
        //        return false;

        //    Utility.SendMessage(hWnd, Utility.WM_COMMAND, WM_ID_COMMIT, 0);

        //    int hAfxMDIWnd = Utility.FindWindowEx(hWnd, 0, "AfxMDIFrame42s", null);

        //    if (hAfxMDIWnd <= 0)
        //        return false;

        //    int hDialogWnd = 0;
        //    int hScrollWnd = 0;
        //    int hCVirtualGridCtrlWnd = 0;

        //    string currentCaption = "";
        //    if (WaitForDialog(hAfxMDIWnd, new string[] { "查询日期" }, ref currentCaption) == 0)
        //        return false;
        //    Thread.Sleep(500);//找到对话框之后充分等待数据传输

        //    while ((hDialogWnd = Utility.FindWindowVisibleEx(hAfxMDIWnd, hDialogWnd, "#32770", null)) > 0)
        //    {
        //        while ((hScrollWnd = Utility.FindWindowEx(hDialogWnd, hScrollWnd, "Afx:400000:0", null)) > 0)
        //        {
        //            int hScrollWnd2 = Utility.FindWindowEx(hScrollWnd, 0, "AfxWnd42s", null);

        //            if (hScrollWnd2 <= 0)
        //                return false;

        //            hCVirtualGridCtrlWnd = Utility.FindWindowEx(hScrollWnd2, 0, "CVirtualGridCtrl", null);

        //            if (hCVirtualGridCtrlWnd > 0)
        //                break;
        //        }

        //        if (hCVirtualGridCtrlWnd > 0)
        //            break;
        //    }


        //    if (hCVirtualGridCtrlWnd <= 0)
        //        return false;

        //    ClipboardHelper.Save();

        //    Clipboard.Clear();

        //    Utility.SendMessage(hCVirtualGridCtrlWnd, Utility.WM_COMMAND, WM_ID_COPY, 0);

        //    try
        //    {
        //        IDataObject iData = Clipboard.GetDataObject();
        //        ClipboardHelper.Restore();

        //        if (iData.GetDataPresent(DataFormats.Text))
        //        {
        //            string strShareHold = (string)iData.GetData(DataFormats.UnicodeText);

        //            if (strShareHold.Length < 100)
        //                return false;

        //            string[] strLines = strShareHold.Split(new string[] { "\r\n" },
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
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //private List<StockHolding> DecodeStockHolding(string[] strColumnHeader,
        //    string[][] strItems)
        //{
        //    List<StockHolding> listStockHoldings = new List<StockHolding>();

        //    for (int i = 0; i < strItems.Length; i++)
        //        listStockHoldings.Add(new StockHolding());

        //    for (int i = 0; i < strColumnHeader.Length; i++)
        //    {
        //        ///StockHolding holding = new StockHolding();

        //        if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Code"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Code = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Name"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Name = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Balance"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Balance = (long)double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_AvaiBalance"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].AvaiBalance = (long)double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Cost"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Cost = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Price"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Price = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_ProfitAndLossRate"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].ProfitAndLossRate = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_ProfitAndLoss"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].ProfitAndLoss = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Holding_Value"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockHoldings[j].Value = double.Parse(strItems[j][i]);
        //        }
        //    }

        //    return listStockHoldings;
        //}
        //private List<StockCommission> DecodeCommission(string[] strColumnHeader,
        //    string[][] strItems)
        //{
        //    List<StockCommission> listStockCommissions = new List<StockCommission>();

        //    for (int i = 0; i < strItems.Length; i++)
        //        listStockCommissions.Add(new StockCommission());

        //    for (int i = 0; i < strColumnHeader.Length; i++)
        //    {
        //        ///StockCommission holding = new StockCommission();

        //        if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Date"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Date = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Code"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Code = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Name"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Name = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Direction"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Direction = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_OrderPrice"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].OrderPrice = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_OrderVol"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].OrderVol = (long)double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Time"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Time = strItems[j][i];
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_DealVol"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].DealVol = (long)double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_DealPrice"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].DealPrice = double.Parse(strItems[j][i]);
        //        }
        //        else if (strColumnHeader[i] == config.AppSettings.Settings["Commission_Contract"].Value.ToString())
        //        {
        //            for (int j = 0; j < strItems.Length; j++)
        //                listStockCommissions[j].Contract = strItems[j][i];
        //        }
        //    }

        //    return listStockCommissions;
        //}
        //public int GetWindowsHandle()
        //{
        //    if (hWnd == 0)
        //    {
        //        StringBuilder stringBuilder = new StringBuilder(256);
        //        while ((hWnd = Utility.FindWindowEx(0, hWnd, null, strMainWindowTitle)) > 0)
        //        {
        //            int hToolbar = Utility.FindWindowEx(hWnd, 0, "ToolbarWindow32", null);
        //            if (hToolbar > 0)
        //            {
        //                int hDialog = 0;
        //                while ((hDialog = Utility.FindWindowEx(hToolbar, hDialog, "#32770", null)) > 0)
        //                {
        //                    int hCombobox = 0;
        //                    while ((hCombobox = Utility.FindWindowEx(hDialog, hCombobox, "ComboBox", null)) > 0)
        //                    {
        //                        Utility.SendMessage(hCombobox, Utility.WM_GETTEXT, 256, stringBuilder);
        //                        string strCaption = stringBuilder.ToString();

        //                        if (strCaption == strSHAccount || strCaption == strSZAccount)
        //                            return hWnd;
        //                    }
        //                }
        //            }
        //        }

        //        return 0;
        //    }
        //    else
        //        return hWnd;

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
                        if (strCaption.Contains(caption))
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

        //private int FindButton(int hDialogWnd, string[] captionArr)
        //{
        //    int hButton = 0;
        //    StringBuilder stringBuilder = new StringBuilder(256);

        //    while ((hButton = Utility.FindWindowEx(hDialogWnd, hButton, "Button", null)) > 0)
        //    {
        //        Utility.GetWindowText(hButton, stringBuilder, 256);

        //        string strCaption = stringBuilder.ToString();

        //        foreach (string caption in captionArr)
        //        {
        //            if (strCaption == caption)
        //                return hButton;
        //        }
        //    }

        //    return 0;
        //}
        //#endregion

    }
}
