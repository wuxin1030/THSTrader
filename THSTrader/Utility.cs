using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace THSTrader
{
    public enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL = 0x0,
        SMTO_BLOCK = 0x1,
        SMTO_ABORTIFHUNG = 0x2,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
        SMTO_ERRORONEXIT = 0x20
    }

    public class Utility
    {
        #region const
        public const int timeout = 20000;

        //Windows 使用的256个虚拟键码
        public const int VK_LBUTTON = 0x1;
        public const int VK_RBUTTON = 0x2;
        public const int VK_CANCEL = 0x3;
        public const int VK_MBUTTON = 0x4;
        public const int VK_BACK = 0x8;
        public const int VK_TAB = 0x9;
        public const int VK_CLEAR = 0xC;
        public const int VK_RETURN = 0xD;
        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_MENU = 0x12;
        public const int VK_PAUSE = 0x13;
        public const int VK_CAPITAL = 0x14;
        public const int VK_ESCAPE = 0x1B;
        public const int VK_SPACE = 0x20;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_Select = 0x29;
        public const int VK_PRINT = 0x2A;
        public const int VK_EXECUTE = 0x2B;
        public const int VK_SNAPSHOT = 0x2C;
        public const int VK_Insert = 0x2D;
        public const int VK_Delete = 0x2E;
        public const int VK_HELP = 0x2F;
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;
        public const int VK_A = 0x41;
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;
        public const int VK_D = 0x44;
        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;
        public const int VK_S = 0x53;
        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;
        public const int VK_W = 0x57;
        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;
        public const int VK_STARTKEY = 0x5B;
        public const int VK_CONTEXTKEY = 0x5D;
        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;
        public const int VK_MULTIPLY = 0x6A;
        public const int VK_ADD = 0x6B;
        public const int VK_SEPARATOR = 0x6C;
        public const int VK_SUBTRACT = 0x6D;
        public const int VK_DECIMAL = 0x6E;
        public const int VK_DIVIDE = 0x6F;
        public const int VK_F1 = 0x70;
        public const int VK_F2 = 0x71;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;
        public const int VK_F13 = 0x7C;
        public const int VK_F14 = 0x7D;
        public const int VK_F15 = 0x7E;
        public const int VK_F16 = 0x7F;
        public const int VK_F17 = 0x80;
        public const int VK_F18 = 0x81;
        public const int VK_F19 = 0x82;
        public const int VK_F20 = 0x83;
        public const int VK_F21 = 0x84;
        public const int VK_F22 = 0x85;
        public const int VK_F23 = 0x86;
        public const int VK_F24 = 0x87;
        public const int VK_NUMLOCK = 0x90;
        public const int VK_OEM_SCROLL = 0x91;
        public const int VK_OEM_1 = 0xBA;
        public const int VK_OEM_PLUS = 0xBB;
        public const int VK_OEM_COMMA = 0xBC;
        public const int VK_OEM_MINUS = 0xBD;
        public const int VK_OEM_PERIOD = 0xBE;
        public const int VK_OEM_2 = 0xBF;
        public const int VK_OEM_3 = 0xC0;
        public const int VK_OEM_4 = 0xDB;
        public const int VK_OEM_5 = 0xDC;
        public const int VK_OEM_6 = 0xDD;
        public const int VK_OEM_7 = 0xDE;
        public const int VK_OEM_8 = 0xDF;
        public const int VK_ICO_F17 = 0xE0;
        public const int VK_ICO_F18 = 0xE1;
        public const int VK_OEM102 = 0xE2;
        public const int VK_ICO_HELP = 0xE3;
        public const int VK_ICO_00 = 0xE4;
        public const int VK_ICO_CLEAR = 0xE6;
        public const int VK_OEM_RESET = 0xE9;
        public const int VK_OEM_JUMP = 0xEA;
        public const int VK_OEM_PA1 = 0xEB;
        public const int VK_OEM_PA2 = 0xEC;
        public const int VK_OEM_PA3 = 0xED;
        public const int VK_OEM_WSCTRL = 0xEE;
        public const int VK_OEM_CUSEL = 0xEF;
        public const int VK_OEM_ATTN = 0xF0;
        public const int VK_OEM_FINNISH = 0xF1;
        public const int VK_OEM_COPY = 0xF2;
        public const int VK_OEM_AUTO = 0xF3;
        public const int VK_OEM_ENLW = 0xF4;
        public const int VK_OEM_BACKTAB = 0xF5;
        public const int VK_ATTN = 0xF6;
        public const int VK_CRSEL = 0xF7;
        public const int VK_EXSEL = 0xF8;
        public const int VK_EREOF = 0xF9;
        public const int VK_PLAY = 0xFA;
        public const int VK_ZOOM = 0xFB;
        public const int VK_NONAME = 0xFC;
        public const int VK_PA1 = 0xFD;
        public const int VK_OEM_CLEAR = 0xFE;


        //创建一个窗口   
        public const int WM_CREATE = 0x01;

        //当一个窗口被破坏时发送   
        public const int WM_DESTROY = 0x02;

        //移动一个窗口   
        public const int WM_MOVE = 0x03;

        //改变一个窗口的大小   
        public const int WM_SIZE = 0x05;

        //一个窗口被激活或失去激活状态   
        public const int WM_ACTIVATE = 0x06;

        //一个窗口获得焦点   
        public const int WM_SETFOCUS = 0x07;

        //一个窗口失去焦点   
        public const int WM_KILLFOCUS = 0x08;

        //一个窗口改变成Enable状态   
        public const int WM_ENABLE = 0x0A;

        //设置窗口是否能重画   
        public const int WM_SETREDRAW = 0x0B;

        //应用程序发送此消息来设置一个窗口的文本   
        public const int WM_SETTEXT = 0x0C;

        //应用程序发送此消息来复制对应窗口的文本到缓冲区   
        public const int WM_GETTEXT = 0x0D;

        //得到与一个窗口有关的文本的长度（不包含空字符）   
        public const int WM_GETTEXTLENGTH = 0x0E;

        //要求一个窗口重画自己   
        public const int WM_PAINT = 0x0F;

        //当一个窗口或应用程序要关闭时发送一个信号   
        public const int WM_CLOSE = 0x10;

        //当用户选择结束对话框或程序自己调用ExitWindows函数   
        public const int WM_QUERYENDSESSION = 0x11;

        //用来结束程序运行   
        public const int WM_QUIT = 0x12;

        //当用户窗口恢复以前的大小位置时，把此消息发送给某个图标   
        public const int WM_QUERYOPEN = 0x13;

        //当窗口背景必须被擦除时（例在窗口改变大小时）   
        public const int WM_ERASEBKGND = 0x14;

        //当系统颜色改变时，发送此消息给所有顶级窗口   
        public const int WM_SYSCOLORCHANGE = 0x15;

        //当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束   
        public const int WM_ENDSESSION = 0x16;

        //当隐藏或显示窗口是发送此消息给这个窗口   
        public const int WM_SHOWWINDOW = 0x18;

        //发此消息给应用程序哪个窗口是激活的，哪个是非激活的   
        public const int WM_ACTIVATEAPP = 0x1C;

        //当系统的字体资源库变化时发送此消息给所有顶级窗口   
        public const int WM_FONTCHANGE = 0x1D;

        //当系统的时间变化时发送此消息给所有顶级窗口   
        public const int WM_TIMECHANGE = 0x1E;

        //发送此消息来取消某种正在进行的摸态（操作）   
        public const int WM_CANCELMODE = 0x1F;

        //如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口   
        public const int WM_SETCURSOR = 0x20;

        //当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给//当前窗口   
        public const int WM_MOUSEACTIVATE = 0x21;

        //发送此消息给MDI子窗口//当用户点击此窗口的标题栏，或//当窗口被激活，移动，改变大小   
        public const int WM_CHILDACTIVATE = 0x22;

        //此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息   
        public const int WM_QUEUESYNC = 0x23;

        //此消息发送给窗口当它将要改变大小或位置   
        public const int WM_GETMINMAXINFO = 0x24;

        //发送给最小化窗口当它图标将要被重画   
        public const int WM_PAINTICON = 0x26;

        //此消息发送给某个最小化窗口，仅//当它在画图标前它的背景必须被重画   
        public const int WM_ICONERASEBKGND = 0x27;

        //发送此消息给一个对话框程序去更改焦点位置   
        public const int WM_NEXTDLGCTL = 0x28;

        //每当打印管理列队增加或减少一条作业时发出此消息    
        public const int WM_SPOOLERSTATUS = 0x2A;

        //当button，combobox，listbox，menu的可视外观改变时发送   
        public const int WM_DRAWITEM = 0x2B;

        //当button, combo box, list box, list view control, or menu item 被创建时   
        public const int WM_MEASUREITEM = 0x2C;

        //此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息    
        public const int WM_VKEYTOITEM = 0x2E;

        //此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息    
        public const int WM_CHARTOITEM = 0x2F;

        //当绘制文本时程序发送此消息得到控件要用的颜色   
        public const int WM_SETFONT = 0x30;

        //应用程序发送此消息得到当前控件绘制文本的字体   
        public const int WM_GETFONT = 0x31;

        //应用程序发送此消息让一个窗口与一个热键相关连    
        public const int WM_SETHOTKEY = 0x32;

        //应用程序发送此消息来判断热键与某个窗口是否有关联   
        public const int WM_GETHOTKEY = 0x33;

        //此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标   
        public const int WM_QUERYDRAGICON = 0x37;

        //发送此消息来判定combobox或listbox新增加的项的相对位置   
        public const int WM_COMPAREITEM = 0x39;

        //显示内存已经很少了   
        public const int WM_COMPACTING = 0x41;

        //发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数   
        public const int WM_WINDOWPOSCHANGING = 0x46;

        //发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数   
        public const int WM_WINDOWPOSCHANGED = 0x47;

        //当系统将要进入暂停状态时发送此消息   
        public const int WM_POWER = 0x48;

        //当一个应用程序传递数据给另一个应用程序时发送此消息   
        public const int WM_COPYDATA = 0x4A;

        //当某个用户取消程序日志激活状态，提交此消息给程序   
        public const int WM_CANCELJOURNA = 0x4B;

        //当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口    
        public const int WM_NOTIFY = 0x4E;

        //当用户选择某种输入语言，或输入语言的热键改变   
        public const int WM_INPUTLANGCHANGEREQUEST = 0x50;

        //当平台现场已经被改变后发送此消息给受影响的最顶级窗口   
        public const int WM_INPUTLANGCHANGE = 0x51;

        //当程序已经初始化windows帮助例程时发送此消息给应用程序   
        public const int WM_TCARD = 0x52;

        //此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就发送给有焦点的窗口，如果//当前都没有焦点，就把此消息发送给//当前激活的窗口   
        public const int WM_HELP = 0x53;

        //当用户已经登入或退出后发送此消息给所有的窗口，//当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息   
        public const int WM_USERCHANGED = 0x54;

        //公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构   
        public const int WM_NOTIFYFORMAT = 0x55;

        //当用户某个窗口中点击了一下右键就发送此消息给这个窗口   
        //public const int WM_CONTEXTMENU = ??;   
        //当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口   
        public const int WM_STYLECHANGING = 0x7C;

        //当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口   
        public const int WM_STYLECHANGED = 0x7D;

        //当显示器的分辨率改变后发送此消息给所有的窗口   
        public const int WM_DISPLAYCHANGE = 0x7E;

        //此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄   
        public const int WM_GETICON = 0x7F;

        //程序发送此消息让一个新的大图标或小图标与某个窗口关联   
        public const int WM_SETICON = 0x80;

        //当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送   
        public const int WM_NCCREATE = 0x81;

        //此消息通知某个窗口，非客户区正在销毁    
        public const int WM_NCDESTROY = 0x82;

        //当某个窗口的客户区域必须被核算时发送此消息   
        public const int WM_NCCALCSIZE = 0x83;

        //移动鼠标，按住或释放鼠标时发生   
        public const int WM_NCHITTEST = 0x84;

        //程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时   
        public const int WM_NCPAINT = 0x85;

        //此消息发送给某个窗口仅当它的非客户区需要被改变来显示是激活还是非激活状态   
        public const int WM_NCACTIVATE = 0x86;

        //发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件通过应   
        public const int WM_GETDLGCODE = 0x87;

        //当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 非客户区为：窗体的标题栏及窗 的边框体   
        public const int WM_NCMOUSEMOVE = 0xA0;

        //当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息   
        public const int WM_NCLBUTTONDOWN = 0xA1;

        //当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息    
        public const int WM_NCLBUTTONUP = 0xA2;

        //当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息   
        public const int WM_NCLBUTTONDBLCLK = 0xA3;

        //当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息   
        public const int WM_NCRBUTTONDOWN = 0xA4;

        //当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息   
        public const int WM_NCRBUTTONUP = 0xA5;

        //当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息   
        public const int WM_NCRBUTTONDBLCLK = 0xA6;

        //当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息   
        public const int WM_NCMBUTTONDOWN = 0xA7;

        //当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息   
        public const int WM_NCMBUTTONUP = 0xA8;

        //当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息   
        public const int WM_NCMBUTTONDBLCLK = 0xA9;

        public const int WM_CLICK = 0xF5;

        //WM_KEYDOWN 按下一个键   
        public const int WM_KEYDOWN = 0x0100;

        //释放一个键   
        public const int WM_KEYUP = 0x0101;

        //按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息   
        public const int WM_CHAR = 0x102;

        //当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口   
        public const int WM_DEADCHAR = 0x103;

        //当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口   
        public const int WM_SYSKEYDOWN = 0x104;

        //当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口   
        public const int WM_SYSKEYUP = 0x105;

        //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口   
        public const int WM_SYSCHAR = 0x106;

        //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口   
        public const int WM_SYSDEADCHAR = 0x107;

        //在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务   
        public const int WM_INITDIALOG = 0x110;

        //当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译   
        public const int WM_COMMAND = 0x111;

        //当用户选择窗口菜单的一条命令或//当用户选择最大化或最小化时那个窗口会收到此消息   
        public const int WM_SYSCOMMAND = 0x112;

        //发生了定时器事件   
        public const int WM_TIMER = 0x113;

        //当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件   
        public const int WM_HSCROLL = 0x114;

        //当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件   
        public const int WM_VSCROLL = 0x115;

        //当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单   
        public const int WM_INITMENU = 0x116;

        //当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部   
        public const int WM_INITMENUPOPUP = 0x117;

        //当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）   
        public const int WM_MENUSELECT = 0x11F;

        //当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者   
        public const int WM_MENUCHAR = 0x120;

        //当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待   
        public const int WM_ENTERIDLE = 0x121;

        //在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色   
        public const int WM_CTLCOLORMSGBOX = 0x132;

        //当一个编辑型控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色   
        public const int WM_CTLCOLOREDIT = 0x133;

        //当一个列表框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色   
        public const int WM_CTLCOLORLISTBOX = 0x134;

        //当一个按钮控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色   
        public const int WM_CTLCOLORBTN = 0x135;

        //当一个对话框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色   
        public const int WM_CTLCOLORDLG = 0x136;

        //当一个滚动条控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色   
        public const int WM_CTLCOLORSCROLLBAR = 0x137;

        //当一个静态控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色   
        public const int WM_CTLCOLORSTATIC = 0x138;

        //当鼠标轮子转动时发送此消息个当前有焦点的控件   
        public const int WM_MOUSEWHEEL = 0x20A;

        //双击鼠标中键   
        public const int WM_MBUTTONDBLCLK = 0x209;

        //释放鼠标中键   
        public const int WM_MBUTTONUP = 0x208;

        //移动鼠标时发生，同WM_MOUSEFIRST   
        public const int WM_MOUSEMOVE = 0x200;

        //按下鼠标左键   
        public const int WM_LBUTTONDOWN = 0x201;

        //释放鼠标左键   
        public const int WM_LBUTTONUP = 0x202;

        //双击鼠标左键   
        public const int WM_LBUTTONDBLCLK = 0x203;

        //按下鼠标右键   
        public const int WM_RBUTTONDOWN = 0x204;

        //释放鼠标右键   
        public const int WM_RBUTTONUP = 0x205;

        //双击鼠标右键   
        public const int WM_RBUTTONDBLCLK = 0x206;

        //按下鼠标中键   
        public const int WM_MBUTTONDOWN = 0x207;

        public const int WM_USER = 0x0400;

        public const int MK_LBUTTON = 0x0001;

        public const int MK_RBUTTON = 0x0002;

        public const int MK_SHIFT = 0x0004;

        public const int MK_CONTROL = 0x0008;

        public const int MK_MBUTTON = 0x0010;

        public const int MK_XBUTTON1 = 0x0020;

        public const int MK_XBUTTON2 = 0x0040;

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        #endregion

        //API回调函数（在c＃中，一个委托实例代表一个函数的指针（入口地址））
        public delegate bool WNDENUMPROC(int hwnd, int lParam);

        #region tools
        //static public string getCode(string strCode)
        //{
        //    if (strCode.Length == 0)
        //        return "000001";

        //    strCode = strCode.ToLower();

        //    if (strCode.Length > 6)
        //        return strCode;

        //    else if (strCode.Substring(0, 1) == "0")
        //        strCode = "sz" + strCode;
        //    else if (strCode.Substring(0, 1) == "6" || strCode.Substring(0, 1) == "5")
        //        strCode = "sh" + strCode;
        //    else
        //        strCode = "sz" + strCode;

        //    return strCode;
        //}

        static public string getItemText(int hWnd)
        {
            int result = 0;
            StringBuilder stringBuilder = new StringBuilder(256);
            Utility.SendMessageTimeoutText(hWnd, Utility.WM_GETTEXT, 256, stringBuilder, SendMessageTimeoutFlags.SMTO_BLOCK, timeout, out result);
            string strCaption = stringBuilder.ToString();
            return strCaption;
        }
        #endregion


        #region P/Invoke USER32

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int _left, int _top, int _right, int _bottom)
            {
                Left = _left;
                Top = _top;
                Right = _right;
                Bottom = _bottom;
            }
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        //Declare wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public POINT(int _x, int _y)
            {
                X = _x;
                Y = _y;
            }
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public int hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LOGPEN
        {
            public uint lopnStyle;
            POINT lopnWidth;
            int lopnColor;
        }

        /// <summary>
        /// 判断一个点是否位于矩形内
        /// </summary>
        /// <param name="lprc"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool PtInRect(
            ref RECT lprc,
            POINT pt
            );

        [DllImport("user32", EntryPoint = "SetParent")]
        public static extern int SetParent(
            IntPtr hwndChild,
            int hwndNewParent
            );

        [DllImport("user32", EntryPoint = "FindWindowA")]
        public static extern int FindWindow(
            string lpClassName,
            string lpWindowName
            );

        [DllImport("user32", EntryPoint = "FindWindowExA")]
        public static extern int FindWindowEx(
            int hwndParent,
            int hwndChildAfter,
            string lpszClass,		//窗口类
            string lpszWindow		//窗口标题
            );

        public static List<int> FindWindowRe(
            int hwndParent,
            int hwndChildAfter,
            string lpszClass,		//窗口类
            string lpszWindow,		//窗口标题
            int depth
            )
        {
            List<int> rets = new List<int>();
            if (depth == 0)
                return rets;

            int handle = FindWindowEx(hwndParent, hwndChildAfter, lpszClass, lpszWindow);
            if (handle > 0)
            {
                rets.Add(handle);
                rets.AddRange(FindWindowRe(hwndParent, handle, lpszClass, lpszWindow, depth));
            }
            else
            {
                int ch = FindWindowEx(hwndParent, 0, null, null);
                while (ch > 0)
                {
                    rets.AddRange(FindWindowRe(ch, 0, lpszClass, lpszWindow, depth - 1));
                    ch = FindWindowEx(hwndParent, ch, null, null);
                }
            }

            return rets;
        }
        public static int FindWindowVisibleEx(
            int hwndParent,
            int hwndChildAfter,
            string lpszClass,		//窗口类
            string lpszWindow		//窗口标题
            )
        {
            while ((hwndChildAfter = FindWindowEx(hwndParent, hwndChildAfter, lpszClass, lpszWindow)) > 0)
            {
                uint result = GetWindowLong(new IntPtr(hwndChildAfter), GWL_STYLE);
                if ((result & WS_VISIBLE) != 0)
                    return hwndChildAfter;
            }

            return 0;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            int hWnd,
            int wMsg,
            int wParam,
            IntPtr lParam
            );

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            int hWnd,
            int wMsg,
            int wParam,
            uint lParam
            );

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(
            int hWnd,
            int wMsg,
            int wParam,
            string lParam
            );

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(
            int hWnd,
            int wMsg,
            int wParam,
            StringBuilder lParam
            );


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            int hWnd,
            uint Msg,
            int wParam,
            string lParam,
            SendMessageTimeoutFlags fuFlags,
            uint timeout,
            out int result);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            int windowHandle,
            uint Msg,
            int wParam,
            int lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out int result);

        /* Version specifically setup for use with WM_GETTEXT message */

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SendMessageTimeoutText(
            int hWnd,
            int Msg,              // Use WM_GETTEXT
            int countOfChars,
            StringBuilder text,
            SendMessageTimeoutFlags flags,
            uint uTImeoutj,
            out int result);

        /* Version for a message which returns an int, such as WM_GETTEXTLENGTH. */

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            int hwnd,
            uint Msg,
            int wParam,
            int lParam,
            uint fuFlags,
            uint uTimeout,
            out int lpdwResult);

        [DllImport("user32", SetLastError = true)]
        public static extern bool PostMessage(
            int hWnd,
            uint Msg,
            int wParam,
            uint lParam
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int PostMessage(
            int hWnd,
            int wMsg,
            int wParam,
            string lParam
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetActiveWindow(int hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(
            int hWnd,
            ref int lpdwProcessId);

        [DllImport("user32")]
        public static extern int Sleep(
            int dwMilliseconds
            );

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(
            int hWnd,
            ref RECT lpRect
            );

        [DllImport("user32")]
        public static extern int GetWindowText(
            int hWnd,
            StringBuilder lpString,
            int nMaxCount
            );


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(
            int hWnd,
            int nCmdShow
            );

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern uint SetWindowLong(
            IntPtr hwnd,
            int nIndex,
            uint dwNewLong
            );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(
            IntPtr hwnd,
            int nIndex
            );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(
            IntPtr hwnd,			//目标窗口句柄
            int ColorRefKey,		//透明色
            int bAlpha,				//不透明度
            int dwFlags
            );

        [DllImport("user32")]
        public static extern bool MoveWindow(
            int hWnd,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            bool bRepaint
            );

        //获得窗口类名称，返回值为字符串的字符数量
        [DllImport("user32")]
        public static extern uint RealGetWindowClass(
            int hWnd,
            StringBuilder pszType,		//缓冲区
            uint cchType);				//缓冲区长度

        //枚举屏幕上所有顶级窗口（不会枚举子窗口，除了一些有WS_CHILD的顶级窗口）
        [DllImport("user32")]
        public static extern bool EnumWindows(
            WNDENUMPROC lpEnumFunc,
            int lParam
            );

        [DllImport("user32")]
        public static extern bool EnumChildWindows(
            int hWndParent,
            WNDENUMPROC lpEnumFunc,
            int lParam
            );

        [DllImport("user32")]
        public static extern bool EnumThreadWindows(		//枚举线程窗口
            int dwThreadId,
            WNDENUMPROC lpEnumFunc,
            int lParam
            );

        [DllImport("user32")]
        public static extern int GetParent(
            int hWnd
            );

        [DllImport("user32")]
        public static extern int GetWindow(
            int hWnd,	//基础窗口
            uint uCmd	//关系
            );

        /*
         * 获取鼠标的屏幕坐标，填充到Point
         * WINUSERAPI
            BOOL
            WINAPI
            GetCursorPos(
                    __out LPPOINT lpPoint);
         */
        [DllImport("user32")]
        public static extern bool GetCursorPos(
            out POINT lpPoint
            );

        [DllImport("user32")]
        public static extern int GetDC(
            int hWnd
            );

        [DllImport("user32")]
        public static extern int GetWindowDC(
            int hWnd
            );

        [DllImport("user32")]
        public static extern int ReleaseDC(
            int hWnd,
            int hDC
            );

        [DllImport("user32")]
        public static extern int FillRect(
            int hDC,
            RECT lprc,
            int hBrush
            );

        [DllImport("user32")]
        public static extern bool InvalidateRect(
            int hwnd,
            ref RECT lpRect,
            bool bErase
            );

        //判断一个窗口是否是可见的
        [DllImport("user32")]
        public static extern bool IsWindowVisible(
            int hwnd
            );

        //绘制焦点举行
        [DllImport("user32")]
        public static extern bool DrawFocusRect(
            int hDC,
            ref RECT lprc
            );

        [DllImport("user32")]
        public static extern bool UpdateWindow(
            int hwnd
            );

        [DllImport("user32")]
        public static extern bool EnableWindow(
            int hwnd,
            bool bEnable
            );

        //设置前景窗口，强制其线程成为前台，并激活窗口
        [DllImport("user32")]
        public static extern bool SetForegroundWindow(
            int hwnd
            );

        [DllImport("user32.dll")]
        public static extern bool IsWindowEnabled(
            int hWnd
            );

        //设置前景窗口，强制其线程成为前台，并激活窗口
        [DllImport("user32")]
        public static extern bool GetForegroundWindow(
            );

        //获取拥有焦点窗口（唯一拥有键盘输入的窗口）
        [DllImport("user32")]
        public static extern int GetFocus(
            );

        //设置焦点窗口（返回值是前一个焦点窗口）
        [DllImport("user32")]
        public static extern int SetFocus(
            int hwnd
            );

        //根据点查找窗口
        [DllImport("user32")]
        public static extern int WindowFromPoint(
            POINT Point
            );

        [DllImport("user32")]
        public static extern int ChildWindowFromPoint(
            int hWndParent,
            POINT Point
            );

        [DllImport("user32")]
        public static extern bool DestroyIcon(
            int hIcon
            );

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(
            int hwnd,
            string lpString
            );

        [DllImport("kernel32")]
        public static extern uint GetTickCount();

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetDlgItem(int hDlg, int nControlID);
        #endregion
    }

}
