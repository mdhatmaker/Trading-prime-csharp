using System;
using System.Drawing;

namespace PrimeTrader
{
    public static class TSx
    {
        public static int m_width = 1920;
        public static int m_height = 1024;

        public static int m_width_new = 1920 / 2;
        public static int m_height_new = 1024 / 2;

        public static int m_panel_top = 100;

        public static Color m_buttonBackColor = Color.FromArgb(161, 194, 218);
        public static Color m_buttonBackColorHighlight = Color.FromArgb(52, 106, 151);
        public static Color m_buttonBackColorSelected = Color.FromArgb(0, 0, 0);
        public static Color m_buttonForeColor = Color.FromArgb(0, 0, 0);
        public static Color m_buttonForeColorHighlight = Color.FromArgb(255, 255, 255);
        public static Color m_buttonForeColorSelected = Color.FromArgb(255, 255, 255);
        public static Color m_brightBlue = Color.FromArgb(40, 150, 255);
    }
    public class TSxImages
    {
        public Image m_low_black = null;
        public Image m_low_white = null;
        public Image m_none_black = null;
        public Image m_none_white = null;
        public Image m_untab_white = null;
        public Image m_untab_black = null;
        public Image m_tab_black = null;
        public Image m_tab_white = null;
        public Image m_copy_white = null;
        public Image m_skyline = null;
        public Image m_fake_orders = null;

        public string m_dir;
        public TSxImages()
        {
            m_dir = System.Reflection.Assembly.GetEntryAssembly().Location;
            int i = m_dir.LastIndexOf("\\");
            m_dir = m_dir.Substring(0, i +1) + "Images\\";

            //Image im = (Image)PrimeTrader.Properties.Resources.copy_white_alpha;
            m_low_black = load("low_black_alpha.png", 18, 18);
            m_low_white = load("low_white_alpha.png", 18, 18);
            m_none_black = load("none_black_alpha.png", 18, 18);
            m_none_white = load("none_white_alpha.png", 18, 18);
            m_untab_black = load("untab_black_alpha.png", 23, 23);
            m_untab_white = load("untab_white_alpha.png", 23, 23);
            m_tab_black = load("tab_black_alpha.png", 18, 18);
            m_tab_white = load("tab_white_alpha.png", 18, 18); ;
            m_copy_white = load("copy_white_alpha.png", 23, 23);
            m_skyline = load("skyline.png", -1, 200);
            m_fake_orders = load("fake_orders_list.png", -1, -1);
        }
        public Image Resize(Image i, int x, int y)
        {
            return (Image)(new Bitmap(i, x, y));
        }

        Image load(string s, int x, int y)
        {
            Bitmap b = (Bitmap)Image.FromFile(m_dir + s);
            if (x == -1)
                x = b.Width;
            if (y == -1)
                y = b.Height;
            return (Image)(new Bitmap(b, x, y));
        }
    }
    public class NewPairEventArgs : EventArgs
    {
        public NewPairEventArgs(string pair) { this.Pair = pair; }
        public string Pair { get; set; }
    }
}
