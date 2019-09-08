using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeTrader
{


    public partial class TSxPairPicker : UserControl
    {
        public event EventHandler<NewPairEventArgs> NewPairEventCallbackHandler;
        private string m_pair = "";

        public TSxPairPicker()
        {

            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox_DrawItem);

            listBox2.DrawMode = DrawMode.OwnerDrawFixed;
            listBox2.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
        }
        void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox list = (ListBox)sender;
            if (e.Index > -1)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    e = new DrawItemEventArgs(e.Graphics,
                                              e.Font,
                                              e.Bounds,
                                              e.Index,
                                              e.State ^ DrawItemState.Selected,
                                              e.ForeColor,
                                              TSx.m_buttonBackColorHighlight);//Choose the color
                object item = list.Items[e.Index];
                e.DrawBackground();
                e.DrawFocusRectangle();
                Brush brush = new SolidBrush(e.ForeColor);
                SizeF size = e.Graphics.MeasureString(item.ToString(), e.Font);
                e.Graphics.DrawString(item.ToString(), e.Font, brush, e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2), e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            }
        }

        public void SetLists(List<string> l1, List<string>l2)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach(string s in l1)
            {
                listBox1.Items.Add(s);
            }
            foreach (string s in l2)
            {
                listBox2.Items.Add(s);
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkPairSelection();
            if (m_pair != "")
            {
                NewPairEventCallbackHandler(this, new NewPairEventArgs(m_pair));
            }
        }

        private void checkPairSelection()
        {
            if (listBox1.SelectedIndex >= 0 && listBox2.SelectedIndex >= 0)
            {
                m_pair = listBox1.SelectedItem.ToString() + "_" + listBox2.SelectedItem.ToString();
                listBox1.ClearSelected();
                listBox2.ClearSelected();
            }
        }
    }
}
