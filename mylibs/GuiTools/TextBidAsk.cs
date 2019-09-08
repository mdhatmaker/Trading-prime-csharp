using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace GuiTools
{
    public class TextBidAsk
    {
        public string Symbol { get { return lblSymbol.Text; } }
        public string BidSize { get { return lblBidSize.Text; } }
        public string Bid { get { return lblBid.Text; } }
        public string Ask { get { return lblAsk.Text; } }
        public string AskSize { get { return lblAskSize.Text; } }

        public TextBidAsk(Panel panel, string symbol)
        {
            Initialize(panel);
            lblSymbol.Text = symbol;
        }

        public void Update(Form frm, PriceUpdate pu)
        {
            Update(frm, pu.Bid, pu.BidSize, pu.Ask, pu.AskSize);
        }

        public void Update(Form frm, decimal bid, decimal bidSize, decimal ask, decimal askSize)
        {
            GUi.SetLabelText(frm, lblBidSize, bidSize.ToString());
            GUi.SetLabelText(frm, lblBid, bid.ToString());
            GUi.SetLabelText(frm, lblAsk, ask.ToString());
            GUi.SetLabelText(frm, lblAskSize, askSize.ToString());
        }
        

        public void Initialize(Panel panel)
        {
            this.tableBidAsk = new System.Windows.Forms.TableLayoutPanel();
            this.lblBidSize = new System.Windows.Forms.Label();
            this.lblAsk = new System.Windows.Forms.Label();
            this.lblBid = new System.Windows.Forms.Label();
            this.lblAskSize = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.tableBidAsk.SuspendLayout();
            panel.SuspendLayout();

            // 
            // tableBidAsk
            // 
            this.tableBidAsk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBidAsk.ColumnCount = 5;
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableBidAsk.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableBidAsk.Controls.Add(this.lblAskSize, 4, 0);
            this.tableBidAsk.Controls.Add(this.lblAsk, 3, 0);
            this.tableBidAsk.Controls.Add(this.lblBidSize, 1, 0);
            this.tableBidAsk.Controls.Add(this.lblSymbol, 0, 0);
            this.tableBidAsk.Controls.Add(this.lblBid, 2, 0);
            this.tableBidAsk.Location = new System.Drawing.Point(122, -2);
            this.tableBidAsk.Name = "tableBidAsk";
            this.tableBidAsk.RowCount = 1;
            this.tableBidAsk.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBidAsk.Size = new System.Drawing.Size(341, 36);
            this.tableBidAsk.TabIndex = 1;
            // 
            // lblBidSize
            // 
            this.lblBidSize.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBidSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBidSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBidSize.Location = new System.Drawing.Point(102, 0);
            this.lblBidSize.Margin = new System.Windows.Forms.Padding(0);
            this.lblBidSize.Name = "lblBidSize";
            this.lblBidSize.Size = new System.Drawing.Size(51, 36);
            this.lblBidSize.TabIndex = 3;
            this.lblBidSize.Text = "0";
            this.lblBidSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAsk
            // 
            this.lblAsk.BackColor = System.Drawing.Color.LightCoral;
            this.lblAsk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAsk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsk.Location = new System.Drawing.Point(221, 0);
            this.lblAsk.Margin = new System.Windows.Forms.Padding(0);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblAsk.Size = new System.Drawing.Size(68, 36);
            this.lblAsk.TabIndex = 4;
            this.lblAsk.Text = "0";
            this.lblAsk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBid
            // 
            this.lblBid.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblBid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBid.Location = new System.Drawing.Point(153, 0);
            this.lblBid.Margin = new System.Windows.Forms.Padding(0);
            this.lblBid.Name = "lblBid";
            this.lblBid.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblBid.Size = new System.Drawing.Size(68, 36);
            this.lblBid.TabIndex = 5;
            this.lblBid.Text = "0";
            this.lblBid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAskSize
            // 
            this.lblAskSize.BackColor = System.Drawing.Color.LightCoral;
            this.lblAskSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAskSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAskSize.Location = new System.Drawing.Point(289, 0);
            this.lblAskSize.Margin = new System.Windows.Forms.Padding(0);
            this.lblAskSize.Name = "lblAskSize";
            this.lblAskSize.Size = new System.Drawing.Size(52, 36);
            this.lblAskSize.TabIndex = 6;
            this.lblAskSize.Text = "0";
            this.lblAskSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSymbol
            // 
            this.lblSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(3, 0);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblSymbol.Size = new System.Drawing.Size(96, 36);
            this.lblSymbol.TabIndex = 7;
            this.lblSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            panel.Controls.Add(this.tableBidAsk);
            this.tableBidAsk.ResumeLayout(false);
            panel.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableBidAsk;
        private System.Windows.Forms.Label lblAskSize;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblBidSize;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblBid;
    }
}
