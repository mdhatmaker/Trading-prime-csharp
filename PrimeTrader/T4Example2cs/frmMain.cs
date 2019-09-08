using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;


// Import the T4 definitions namespace.
using T4;

// Import the API namespace.
using T4.API;

// Import XML for saving and retriving markets.
using System.Xml;

// Generic collections.
using System.Collections.Generic;

namespace T4Example2CSharp
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {

        #region Windows Form Designer generated code

        internal System.Windows.Forms.Label lblMarket;
        internal System.Windows.Forms.ComboBox cboMarkets;
        internal System.Windows.Forms.Label lblContract;
        internal System.Windows.Forms.Label lblExchange;
        internal System.Windows.Forms.ComboBox cboContracts;
        internal System.Windows.Forms.ComboBox cboExchanges;
        internal System.Windows.Forms.GroupBox grpAccountPicker;
        internal System.Windows.Forms.ComboBox cboAccounts;
        internal System.Windows.Forms.Label lblCash;
        internal System.Windows.Forms.Label lblAccount;
        internal System.Windows.Forms.TextBox txtCash;
        internal System.Windows.Forms.GroupBox grpMarkets;
        internal System.Windows.Forms.Label lblMisc1;
        internal System.Windows.Forms.Button cmdRunMisc1;
        internal System.Windows.Forms.ComboBox cboMisc1;
        internal System.Windows.Forms.Label lblMisc2;
        internal System.Windows.Forms.Button cmdRunMisc2;
        internal System.Windows.Forms.ComboBox cboMisc2;
        internal System.Windows.Forms.Label lblLastPrice;
        internal System.Windows.Forms.Label lblOfferPrice;
        internal System.Windows.Forms.Label lblBidPrice;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtOfferVol1;
        internal System.Windows.Forms.TextBox txtLastVolTotal1;
        internal System.Windows.Forms.TextBox txtLast2;
        internal System.Windows.Forms.TextBox txtLast1;
        internal System.Windows.Forms.TextBox txtOfferVol2;
        internal System.Windows.Forms.TextBox txtBidVol2;
        internal System.Windows.Forms.TextBox txtOffer2;
        internal System.Windows.Forms.TextBox txtBid2;
        internal System.Windows.Forms.TextBox txtMarketDescription2;
        internal System.Windows.Forms.Button cmdGet2;
        internal System.Windows.Forms.TextBox txtNet1;
        internal System.Windows.Forms.TextBox txtNet2;
        internal System.Windows.Forms.Label lblNet;
        internal System.Windows.Forms.TextBox txtLastVol1;
        internal System.Windows.Forms.Label lblSells;
        internal System.Windows.Forms.TextBox txtBuys1;
        internal System.Windows.Forms.TextBox txtBuys2;
        internal System.Windows.Forms.TextBox txtSells1;
        internal System.Windows.Forms.Button cmdGet1;
        internal System.Windows.Forms.TextBox txtMarketDescription1;
        internal System.Windows.Forms.TextBox txtBid1;
        internal System.Windows.Forms.Button cmdBuy2;
        internal System.Windows.Forms.Button cmdSell1;
        internal System.Windows.Forms.Button cmdSell2;
        internal System.Windows.Forms.Label lblBuys;
        internal System.Windows.Forms.TextBox txtSells2;
        internal System.Windows.Forms.TextBox txtOffer1;
        internal System.Windows.Forms.Button cmdBuy1;
        internal System.Windows.Forms.TextBox txtBidVol1;
        internal System.Windows.Forms.TextBox txtLastVol2;
        internal System.Windows.Forms.TextBox txtLastVolTotal2;
        internal System.Windows.Forms.Label lblTotalVol;
        internal System.Windows.Forms.Label lblLastVol;
        internal System.Windows.Forms.Label lblOfferVol;
        internal System.Windows.Forms.Label lblBidVol;
        internal System.Windows.Forms.Button cmdSave;
        internal System.Windows.Forms.Label lblSaveInfo;
        internal System.Windows.Forms.GroupBox grpOrders;
        internal System.Windows.Forms.ListBox lstOrders;
        internal System.Windows.Forms.Label lblOrderInfo;
        private GroupBox grpMarket2;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //


            // Finnally register Form events.
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Closed += new System.EventHandler(this.frmMain_Closed);

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMarket = new System.Windows.Forms.Label();
            this.cboMarkets = new System.Windows.Forms.ComboBox();
            this.lblContract = new System.Windows.Forms.Label();
            this.lblExchange = new System.Windows.Forms.Label();
            this.cboContracts = new System.Windows.Forms.ComboBox();
            this.cboExchanges = new System.Windows.Forms.ComboBox();
            this.grpAccountPicker = new System.Windows.Forms.GroupBox();
            this.cboAccounts = new System.Windows.Forms.ComboBox();
            this.lblCash = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtCash = new System.Windows.Forms.TextBox();
            this.grpMarkets = new System.Windows.Forms.GroupBox();
            this.lblMisc1 = new System.Windows.Forms.Label();
            this.cmdRunMisc1 = new System.Windows.Forms.Button();
            this.cboMisc1 = new System.Windows.Forms.ComboBox();
            this.lblLastPrice = new System.Windows.Forms.Label();
            this.lblOfferPrice = new System.Windows.Forms.Label();
            this.lblBidPrice = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtOfferVol1 = new System.Windows.Forms.TextBox();
            this.txtLastVolTotal1 = new System.Windows.Forms.TextBox();
            this.txtLast1 = new System.Windows.Forms.TextBox();
            this.txtNet1 = new System.Windows.Forms.TextBox();
            this.lblNet = new System.Windows.Forms.Label();
            this.txtLastVol1 = new System.Windows.Forms.TextBox();
            this.lblSells = new System.Windows.Forms.Label();
            this.txtBuys1 = new System.Windows.Forms.TextBox();
            this.txtSells1 = new System.Windows.Forms.TextBox();
            this.cmdGet1 = new System.Windows.Forms.Button();
            this.txtMarketDescription1 = new System.Windows.Forms.TextBox();
            this.txtBid1 = new System.Windows.Forms.TextBox();
            this.cmdSell1 = new System.Windows.Forms.Button();
            this.lblBuys = new System.Windows.Forms.Label();
            this.txtOffer1 = new System.Windows.Forms.TextBox();
            this.cmdBuy1 = new System.Windows.Forms.Button();
            this.txtBidVol1 = new System.Windows.Forms.TextBox();
            this.lblTotalVol = new System.Windows.Forms.Label();
            this.lblLastVol = new System.Windows.Forms.Label();
            this.lblOfferVol = new System.Windows.Forms.Label();
            this.lblBidVol = new System.Windows.Forms.Label();
            this.lblMisc2 = new System.Windows.Forms.Label();
            this.cmdRunMisc2 = new System.Windows.Forms.Button();
            this.cboMisc2 = new System.Windows.Forms.ComboBox();
            this.txtLast2 = new System.Windows.Forms.TextBox();
            this.txtOfferVol2 = new System.Windows.Forms.TextBox();
            this.txtBidVol2 = new System.Windows.Forms.TextBox();
            this.txtOffer2 = new System.Windows.Forms.TextBox();
            this.txtBid2 = new System.Windows.Forms.TextBox();
            this.txtMarketDescription2 = new System.Windows.Forms.TextBox();
            this.cmdGet2 = new System.Windows.Forms.Button();
            this.txtNet2 = new System.Windows.Forms.TextBox();
            this.txtBuys2 = new System.Windows.Forms.TextBox();
            this.cmdBuy2 = new System.Windows.Forms.Button();
            this.cmdSell2 = new System.Windows.Forms.Button();
            this.txtSells2 = new System.Windows.Forms.TextBox();
            this.txtLastVol2 = new System.Windows.Forms.TextBox();
            this.txtLastVolTotal2 = new System.Windows.Forms.TextBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.lblSaveInfo = new System.Windows.Forms.Label();
            this.grpOrders = new System.Windows.Forms.GroupBox();
            this.lstOrders = new System.Windows.Forms.ListBox();
            this.lblOrderInfo = new System.Windows.Forms.Label();
            this.grpMarket2 = new System.Windows.Forms.GroupBox();
            this.grpAccountPicker.SuspendLayout();
            this.grpMarkets.SuspendLayout();
            this.grpOrders.SuspendLayout();
            this.grpMarket2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMarket
            // 
            this.lblMarket.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarket.Location = new System.Drawing.Point(31, 76);
            this.lblMarket.Name = "lblMarket";
            this.lblMarket.Size = new System.Drawing.Size(75, 24);
            this.lblMarket.TabIndex = 11;
            this.lblMarket.Text = "Market:";
            this.lblMarket.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboMarkets
            // 
            this.cboMarkets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarkets.Location = new System.Drawing.Point(108, 77);
            this.cboMarkets.Name = "cboMarkets";
            this.cboMarkets.Size = new System.Drawing.Size(401, 25);
            this.cboMarkets.TabIndex = 6;
            this.cboMarkets.TabStop = false;
            this.cboMarkets.SelectedIndexChanged += new System.EventHandler(this.cboMarkets_SelectedIndexChanged);
            // 
            // lblContract
            // 
            this.lblContract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContract.Location = new System.Drawing.Point(31, 48);
            this.lblContract.Name = "lblContract";
            this.lblContract.Size = new System.Drawing.Size(75, 25);
            this.lblContract.TabIndex = 10;
            this.lblContract.Text = "Contract:";
            this.lblContract.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExchange
            // 
            this.lblExchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExchange.Location = new System.Drawing.Point(10, 21);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(91, 24);
            this.lblExchange.TabIndex = 9;
            this.lblExchange.Text = "Exchange:";
            this.lblExchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboContracts
            // 
            this.cboContracts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContracts.Location = new System.Drawing.Point(108, 50);
            this.cboContracts.Name = "cboContracts";
            this.cboContracts.Size = new System.Drawing.Size(401, 25);
            this.cboContracts.Sorted = true;
            this.cboContracts.TabIndex = 8;
            this.cboContracts.TabStop = false;
            this.cboContracts.SelectedIndexChanged += new System.EventHandler(this.cboContracts_SelectedIndexChanged);
            // 
            // cboExchanges
            // 
            this.cboExchanges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExchanges.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboExchanges.Location = new System.Drawing.Point(108, 22);
            this.cboExchanges.Name = "cboExchanges";
            this.cboExchanges.Size = new System.Drawing.Size(401, 25);
            this.cboExchanges.Sorted = true;
            this.cboExchanges.TabIndex = 7;
            this.cboExchanges.TabStop = false;
            this.cboExchanges.SelectedIndexChanged += new System.EventHandler(this.cboExchanges_SelectedIndexChanged);
            // 
            // grpAccountPicker
            // 
            this.grpAccountPicker.Controls.Add(this.cboAccounts);
            this.grpAccountPicker.Controls.Add(this.lblCash);
            this.grpAccountPicker.Controls.Add(this.lblAccount);
            this.grpAccountPicker.Controls.Add(this.txtCash);
            this.grpAccountPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAccountPicker.Location = new System.Drawing.Point(10, 9);
            this.grpAccountPicker.Name = "grpAccountPicker";
            this.grpAccountPicker.Size = new System.Drawing.Size(859, 60);
            this.grpAccountPicker.TabIndex = 64;
            this.grpAccountPicker.TabStop = false;
            this.grpAccountPicker.Text = "Account";
            // 
            // cboAccounts
            // 
            this.cboAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAccounts.Location = new System.Drawing.Point(154, 25);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new System.Drawing.Size(249, 25);
            this.cboAccounts.Sorted = true;
            this.cboAccounts.TabIndex = 42;
            this.cboAccounts.TabStop = false;
            this.cboAccounts.SelectedIndexChanged += new System.EventHandler(this.cboAccounts_SelectedIndexChanged);
            // 
            // lblCash
            // 
            this.lblCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCash.Location = new System.Drawing.Point(418, 25);
            this.lblCash.Name = "lblCash";
            this.lblCash.Size = new System.Drawing.Size(49, 25);
            this.lblCash.TabIndex = 44;
            this.lblCash.Text = "Cash:";
            this.lblCash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(26, 25);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(142, 25);
            this.lblAccount.TabIndex = 41;
            this.lblAccount.Text = "Current Account:";
            this.lblAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCash
            // 
            this.txtCash.BackColor = System.Drawing.Color.White;
            this.txtCash.Location = new System.Drawing.Point(469, 25);
            this.txtCash.Name = "txtCash";
            this.txtCash.ReadOnly = true;
            this.txtCash.Size = new System.Drawing.Size(130, 23);
            this.txtCash.TabIndex = 43;
            this.txtCash.TabStop = false;
            this.txtCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpMarkets
            // 
            this.grpMarkets.Controls.Add(this.lblMarket);
            this.grpMarkets.Controls.Add(this.lblMisc1);
            this.grpMarkets.Controls.Add(this.cboMarkets);
            this.grpMarkets.Controls.Add(this.cmdRunMisc1);
            this.grpMarkets.Controls.Add(this.lblContract);
            this.grpMarkets.Controls.Add(this.cboMisc1);
            this.grpMarkets.Controls.Add(this.lblExchange);
            this.grpMarkets.Controls.Add(this.lblLastPrice);
            this.grpMarkets.Controls.Add(this.cboContracts);
            this.grpMarkets.Controls.Add(this.lblOfferPrice);
            this.grpMarkets.Controls.Add(this.cboExchanges);
            this.grpMarkets.Controls.Add(this.lblBidPrice);
            this.grpMarkets.Controls.Add(this.Label1);
            this.grpMarkets.Controls.Add(this.txtOfferVol1);
            this.grpMarkets.Controls.Add(this.txtLastVolTotal1);
            this.grpMarkets.Controls.Add(this.txtLast1);
            this.grpMarkets.Controls.Add(this.txtNet1);
            this.grpMarkets.Controls.Add(this.lblNet);
            this.grpMarkets.Controls.Add(this.txtLastVol1);
            this.grpMarkets.Controls.Add(this.lblSells);
            this.grpMarkets.Controls.Add(this.txtBuys1);
            this.grpMarkets.Controls.Add(this.txtSells1);
            this.grpMarkets.Controls.Add(this.cmdGet1);
            this.grpMarkets.Controls.Add(this.txtMarketDescription1);
            this.grpMarkets.Controls.Add(this.txtBid1);
            this.grpMarkets.Controls.Add(this.cmdSell1);
            this.grpMarkets.Controls.Add(this.lblBuys);
            this.grpMarkets.Controls.Add(this.txtOffer1);
            this.grpMarkets.Controls.Add(this.cmdBuy1);
            this.grpMarkets.Controls.Add(this.txtBidVol1);
            this.grpMarkets.Controls.Add(this.lblTotalVol);
            this.grpMarkets.Controls.Add(this.lblLastVol);
            this.grpMarkets.Controls.Add(this.lblOfferVol);
            this.grpMarkets.Controls.Add(this.lblBidVol);
            this.grpMarkets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMarkets.Location = new System.Drawing.Point(10, 85);
            this.grpMarkets.Name = "grpMarkets";
            this.grpMarkets.Size = new System.Drawing.Size(831, 217);
            this.grpMarkets.TabIndex = 66;
            this.grpMarkets.TabStop = false;
            this.grpMarkets.Text = "Market 1";
            // 
            // lblMisc1
            // 
            this.lblMisc1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMisc1.Location = new System.Drawing.Point(463, 183);
            this.lblMisc1.Name = "lblMisc1";
            this.lblMisc1.Size = new System.Drawing.Size(84, 24);
            this.lblMisc1.TabIndex = 67;
            this.lblMisc1.Text = "Misc Code:";
            this.lblMisc1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdRunMisc1
            // 
            this.cmdRunMisc1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRunMisc1.Location = new System.Drawing.Point(773, 183);
            this.cmdRunMisc1.Name = "cmdRunMisc1";
            this.cmdRunMisc1.Size = new System.Drawing.Size(45, 24);
            this.cmdRunMisc1.TabIndex = 66;
            this.cmdRunMisc1.TabStop = false;
            this.cmdRunMisc1.Text = "Run";
            this.cmdRunMisc1.Click += new System.EventHandler(this.cmdRunMisc1_Click);
            // 
            // cboMisc1
            // 
            this.cboMisc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMisc1.Location = new System.Drawing.Point(552, 183);
            this.cboMisc1.Name = "cboMisc1";
            this.cboMisc1.Size = new System.Drawing.Size(216, 25);
            this.cboMisc1.Sorted = true;
            this.cboMisc1.TabIndex = 65;
            this.cboMisc1.TabStop = false;
            // 
            // lblLastPrice
            // 
            this.lblLastPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastPrice.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblLastPrice.Location = new System.Drawing.Point(488, 135);
            this.lblLastPrice.Name = "lblLastPrice";
            this.lblLastPrice.Size = new System.Drawing.Size(72, 23);
            this.lblLastPrice.TabIndex = 25;
            this.lblLastPrice.Text = "Price:";
            this.lblLastPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOfferPrice
            // 
            this.lblOfferPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOfferPrice.ForeColor = System.Drawing.Color.Crimson;
            this.lblOfferPrice.Location = new System.Drawing.Point(378, 135);
            this.lblOfferPrice.Name = "lblOfferPrice";
            this.lblOfferPrice.Size = new System.Drawing.Size(72, 23);
            this.lblOfferPrice.TabIndex = 24;
            this.lblOfferPrice.Text = "Price:";
            this.lblOfferPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBidPrice
            // 
            this.lblBidPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBidPrice.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblBidPrice.Location = new System.Drawing.Point(268, 135);
            this.lblBidPrice.Name = "lblBidPrice";
            this.lblBidPrice.Size = new System.Drawing.Size(72, 23);
            this.lblBidPrice.TabIndex = 23;
            this.lblBidPrice.Text = "Price:";
            this.lblBidPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(13, 135);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(252, 23);
            this.Label1.TabIndex = 22;
            this.Label1.Text = "Market Description:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOfferVol1
            // 
            this.txtOfferVol1.BackColor = System.Drawing.Color.MistyRose;
            this.txtOfferVol1.Location = new System.Drawing.Point(455, 158);
            this.txtOfferVol1.Name = "txtOfferVol1";
            this.txtOfferVol1.ReadOnly = true;
            this.txtOfferVol1.Size = new System.Drawing.Size(33, 23);
            this.txtOfferVol1.TabIndex = 17;
            this.txtOfferVol1.TabStop = false;
            this.txtOfferVol1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLastVolTotal1
            // 
            this.txtLastVolTotal1.BackColor = System.Drawing.Color.Honeydew;
            this.txtLastVolTotal1.Location = new System.Drawing.Point(601, 158);
            this.txtLastVolTotal1.Name = "txtLastVolTotal1";
            this.txtLastVolTotal1.ReadOnly = true;
            this.txtLastVolTotal1.Size = new System.Drawing.Size(72, 23);
            this.txtLastVolTotal1.TabIndex = 21;
            this.txtLastVolTotal1.TabStop = false;
            this.txtLastVolTotal1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLast1
            // 
            this.txtLast1.BackColor = System.Drawing.Color.Honeydew;
            this.txtLast1.Location = new System.Drawing.Point(491, 158);
            this.txtLast1.Name = "txtLast1";
            this.txtLast1.ReadOnly = true;
            this.txtLast1.Size = new System.Drawing.Size(72, 23);
            this.txtLast1.TabIndex = 18;
            this.txtLast1.TabStop = false;
            this.txtLast1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNet1
            // 
            this.txtNet1.BackColor = System.Drawing.Color.White;
            this.txtNet1.Location = new System.Drawing.Point(676, 158);
            this.txtNet1.Name = "txtNet1";
            this.txtNet1.ReadOnly = true;
            this.txtNet1.Size = new System.Drawing.Size(45, 23);
            this.txtNet1.TabIndex = 46;
            this.txtNet1.TabStop = false;
            this.txtNet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblNet
            // 
            this.lblNet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNet.Location = new System.Drawing.Point(673, 135);
            this.lblNet.Name = "lblNet";
            this.lblNet.Size = new System.Drawing.Size(46, 21);
            this.lblNet.TabIndex = 48;
            this.lblNet.Text = "Net:";
            this.lblNet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLastVol1
            // 
            this.txtLastVol1.BackColor = System.Drawing.Color.Honeydew;
            this.txtLastVol1.Location = new System.Drawing.Point(565, 158);
            this.txtLastVol1.Name = "txtLastVol1";
            this.txtLastVol1.ReadOnly = true;
            this.txtLastVol1.Size = new System.Drawing.Size(34, 23);
            this.txtLastVol1.TabIndex = 20;
            this.txtLastVol1.TabStop = false;
            this.txtLastVol1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSells
            // 
            this.lblSells.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSells.Location = new System.Drawing.Point(769, 135);
            this.lblSells.Name = "lblSells";
            this.lblSells.Size = new System.Drawing.Size(46, 21);
            this.lblSells.TabIndex = 50;
            this.lblSells.Text = "Sells:";
            this.lblSells.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBuys1
            // 
            this.txtBuys1.BackColor = System.Drawing.Color.White;
            this.txtBuys1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtBuys1.Location = new System.Drawing.Point(724, 158);
            this.txtBuys1.Name = "txtBuys1";
            this.txtBuys1.ReadOnly = true;
            this.txtBuys1.Size = new System.Drawing.Size(45, 23);
            this.txtBuys1.TabIndex = 51;
            this.txtBuys1.TabStop = false;
            this.txtBuys1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSells1
            // 
            this.txtSells1.BackColor = System.Drawing.Color.White;
            this.txtSells1.ForeColor = System.Drawing.Color.Crimson;
            this.txtSells1.Location = new System.Drawing.Point(772, 158);
            this.txtSells1.Name = "txtSells1";
            this.txtSells1.ReadOnly = true;
            this.txtSells1.Size = new System.Drawing.Size(45, 23);
            this.txtSells1.TabIndex = 53;
            this.txtSells1.TabStop = false;
            this.txtSells1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdGet1
            // 
            this.cmdGet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGet1.Location = new System.Drawing.Point(577, 23);
            this.cmdGet1.Name = "cmdGet1";
            this.cmdGet1.Size = new System.Drawing.Size(42, 24);
            this.cmdGet1.TabIndex = 10;
            this.cmdGet1.TabStop = false;
            this.cmdGet1.Text = "Get";
            this.cmdGet1.Click += new System.EventHandler(this.cmdGet1_Click);
            // 
            // txtMarketDescription1
            // 
            this.txtMarketDescription1.BackColor = System.Drawing.Color.White;
            this.txtMarketDescription1.Location = new System.Drawing.Point(18, 158);
            this.txtMarketDescription1.Name = "txtMarketDescription1";
            this.txtMarketDescription1.ReadOnly = true;
            this.txtMarketDescription1.Size = new System.Drawing.Size(250, 23);
            this.txtMarketDescription1.TabIndex = 11;
            this.txtMarketDescription1.TabStop = false;
            // 
            // txtBid1
            // 
            this.txtBid1.BackColor = System.Drawing.Color.LightCyan;
            this.txtBid1.Location = new System.Drawing.Point(270, 158);
            this.txtBid1.Name = "txtBid1";
            this.txtBid1.ReadOnly = true;
            this.txtBid1.Size = new System.Drawing.Size(72, 23);
            this.txtBid1.TabIndex = 12;
            this.txtBid1.TabStop = false;
            this.txtBid1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdSell1
            // 
            this.cmdSell1.BackColor = System.Drawing.Color.Crimson;
            this.cmdSell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSell1.ForeColor = System.Drawing.Color.White;
            this.cmdSell1.Location = new System.Drawing.Point(378, 183);
            this.cmdSell1.Name = "cmdSell1";
            this.cmdSell1.Size = new System.Drawing.Size(72, 24);
            this.cmdSell1.TabIndex = 58;
            this.cmdSell1.TabStop = false;
            this.cmdSell1.Text = "Sell";
            this.cmdSell1.UseVisualStyleBackColor = false;
            this.cmdSell1.Click += new System.EventHandler(this.cmdSell1_Click);
            // 
            // lblBuys
            // 
            this.lblBuys.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuys.Location = new System.Drawing.Point(721, 135);
            this.lblBuys.Name = "lblBuys";
            this.lblBuys.Size = new System.Drawing.Size(46, 21);
            this.lblBuys.TabIndex = 49;
            this.lblBuys.Text = "Buys:";
            this.lblBuys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOffer1
            // 
            this.txtOffer1.BackColor = System.Drawing.Color.MistyRose;
            this.txtOffer1.Location = new System.Drawing.Point(380, 158);
            this.txtOffer1.Name = "txtOffer1";
            this.txtOffer1.ReadOnly = true;
            this.txtOffer1.Size = new System.Drawing.Size(72, 23);
            this.txtOffer1.TabIndex = 14;
            this.txtOffer1.TabStop = false;
            this.txtOffer1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdBuy1
            // 
            this.cmdBuy1.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdBuy1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBuy1.ForeColor = System.Drawing.Color.White;
            this.cmdBuy1.Location = new System.Drawing.Point(268, 183);
            this.cmdBuy1.Name = "cmdBuy1";
            this.cmdBuy1.Size = new System.Drawing.Size(72, 24);
            this.cmdBuy1.TabIndex = 55;
            this.cmdBuy1.TabStop = false;
            this.cmdBuy1.Text = "Buy";
            this.cmdBuy1.UseVisualStyleBackColor = false;
            this.cmdBuy1.Click += new System.EventHandler(this.cmdBuy1_Click);
            // 
            // txtBidVol1
            // 
            this.txtBidVol1.BackColor = System.Drawing.Color.LightCyan;
            this.txtBidVol1.Location = new System.Drawing.Point(344, 158);
            this.txtBidVol1.Name = "txtBidVol1";
            this.txtBidVol1.ReadOnly = true;
            this.txtBidVol1.Size = new System.Drawing.Size(34, 23);
            this.txtBidVol1.TabIndex = 16;
            this.txtBidVol1.TabStop = false;
            this.txtBidVol1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalVol
            // 
            this.lblTotalVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalVol.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTotalVol.Location = new System.Drawing.Point(599, 135);
            this.lblTotalVol.Name = "lblTotalVol";
            this.lblTotalVol.Size = new System.Drawing.Size(77, 23);
            this.lblTotalVol.TabIndex = 29;
            this.lblTotalVol.Text = "Total Vol:";
            this.lblTotalVol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLastVol
            // 
            this.lblLastVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastVol.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblLastVol.Location = new System.Drawing.Point(563, 135);
            this.lblLastVol.Name = "lblLastVol";
            this.lblLastVol.Size = new System.Drawing.Size(38, 23);
            this.lblLastVol.TabIndex = 28;
            this.lblLastVol.Text = "Vol:";
            this.lblLastVol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOfferVol
            // 
            this.lblOfferVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOfferVol.ForeColor = System.Drawing.Color.Crimson;
            this.lblOfferVol.Location = new System.Drawing.Point(452, 135);
            this.lblOfferVol.Name = "lblOfferVol";
            this.lblOfferVol.Size = new System.Drawing.Size(39, 23);
            this.lblOfferVol.TabIndex = 27;
            this.lblOfferVol.Text = "Vol:";
            this.lblOfferVol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBidVol
            // 
            this.lblBidVol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBidVol.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblBidVol.Location = new System.Drawing.Point(342, 135);
            this.lblBidVol.Name = "lblBidVol";
            this.lblBidVol.Size = new System.Drawing.Size(38, 23);
            this.lblBidVol.TabIndex = 26;
            this.lblBidVol.Text = "Vol:";
            this.lblBidVol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMisc2
            // 
            this.lblMisc2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMisc2.Location = new System.Drawing.Point(463, 83);
            this.lblMisc2.Name = "lblMisc2";
            this.lblMisc2.Size = new System.Drawing.Size(84, 23);
            this.lblMisc2.TabIndex = 64;
            this.lblMisc2.Text = "Misc Code:";
            this.lblMisc2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdRunMisc2
            // 
            this.cmdRunMisc2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRunMisc2.Location = new System.Drawing.Point(773, 83);
            this.cmdRunMisc2.Name = "cmdRunMisc2";
            this.cmdRunMisc2.Size = new System.Drawing.Size(45, 23);
            this.cmdRunMisc2.TabIndex = 63;
            this.cmdRunMisc2.TabStop = false;
            this.cmdRunMisc2.Text = "Run";
            this.cmdRunMisc2.Click += new System.EventHandler(this.cmdRunMisc2_Click);
            // 
            // cboMisc2
            // 
            this.cboMisc2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMisc2.Location = new System.Drawing.Point(552, 83);
            this.cboMisc2.Name = "cboMisc2";
            this.cboMisc2.Size = new System.Drawing.Size(216, 25);
            this.cboMisc2.Sorted = true;
            this.cboMisc2.TabIndex = 62;
            this.cboMisc2.TabStop = false;
            // 
            // txtLast2
            // 
            this.txtLast2.BackColor = System.Drawing.Color.Honeydew;
            this.txtLast2.Location = new System.Drawing.Point(491, 58);
            this.txtLast2.Name = "txtLast2";
            this.txtLast2.ReadOnly = true;
            this.txtLast2.Size = new System.Drawing.Size(72, 23);
            this.txtLast2.TabIndex = 36;
            this.txtLast2.TabStop = false;
            this.txtLast2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOfferVol2
            // 
            this.txtOfferVol2.BackColor = System.Drawing.Color.MistyRose;
            this.txtOfferVol2.Location = new System.Drawing.Point(455, 58);
            this.txtOfferVol2.Name = "txtOfferVol2";
            this.txtOfferVol2.ReadOnly = true;
            this.txtOfferVol2.Size = new System.Drawing.Size(33, 23);
            this.txtOfferVol2.TabIndex = 35;
            this.txtOfferVol2.TabStop = false;
            this.txtOfferVol2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBidVol2
            // 
            this.txtBidVol2.BackColor = System.Drawing.Color.LightCyan;
            this.txtBidVol2.Location = new System.Drawing.Point(344, 58);
            this.txtBidVol2.Name = "txtBidVol2";
            this.txtBidVol2.ReadOnly = true;
            this.txtBidVol2.Size = new System.Drawing.Size(34, 23);
            this.txtBidVol2.TabIndex = 34;
            this.txtBidVol2.TabStop = false;
            this.txtBidVol2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOffer2
            // 
            this.txtOffer2.BackColor = System.Drawing.Color.MistyRose;
            this.txtOffer2.Location = new System.Drawing.Point(380, 58);
            this.txtOffer2.Name = "txtOffer2";
            this.txtOffer2.ReadOnly = true;
            this.txtOffer2.Size = new System.Drawing.Size(72, 23);
            this.txtOffer2.TabIndex = 33;
            this.txtOffer2.TabStop = false;
            this.txtOffer2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBid2
            // 
            this.txtBid2.BackColor = System.Drawing.Color.LightCyan;
            this.txtBid2.Location = new System.Drawing.Point(270, 58);
            this.txtBid2.Name = "txtBid2";
            this.txtBid2.ReadOnly = true;
            this.txtBid2.Size = new System.Drawing.Size(72, 23);
            this.txtBid2.TabIndex = 32;
            this.txtBid2.TabStop = false;
            this.txtBid2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMarketDescription2
            // 
            this.txtMarketDescription2.BackColor = System.Drawing.Color.White;
            this.txtMarketDescription2.Location = new System.Drawing.Point(18, 58);
            this.txtMarketDescription2.Name = "txtMarketDescription2";
            this.txtMarketDescription2.ReadOnly = true;
            this.txtMarketDescription2.Size = new System.Drawing.Size(250, 23);
            this.txtMarketDescription2.TabIndex = 31;
            this.txtMarketDescription2.TabStop = false;
            // 
            // cmdGet2
            // 
            this.cmdGet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGet2.Location = new System.Drawing.Point(17, 28);
            this.cmdGet2.Name = "cmdGet2";
            this.cmdGet2.Size = new System.Drawing.Size(69, 23);
            this.cmdGet2.TabIndex = 30;
            this.cmdGet2.TabStop = false;
            this.cmdGet2.Text = "Picker";
            this.cmdGet2.Click += new System.EventHandler(this.cmdGet2_Click);
            // 
            // txtNet2
            // 
            this.txtNet2.BackColor = System.Drawing.Color.White;
            this.txtNet2.Location = new System.Drawing.Point(676, 58);
            this.txtNet2.Name = "txtNet2";
            this.txtNet2.ReadOnly = true;
            this.txtNet2.Size = new System.Drawing.Size(45, 23);
            this.txtNet2.TabIndex = 47;
            this.txtNet2.TabStop = false;
            this.txtNet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBuys2
            // 
            this.txtBuys2.BackColor = System.Drawing.Color.White;
            this.txtBuys2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtBuys2.Location = new System.Drawing.Point(724, 58);
            this.txtBuys2.Name = "txtBuys2";
            this.txtBuys2.ReadOnly = true;
            this.txtBuys2.Size = new System.Drawing.Size(45, 23);
            this.txtBuys2.TabIndex = 52;
            this.txtBuys2.TabStop = false;
            this.txtBuys2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdBuy2
            // 
            this.cmdBuy2.BackColor = System.Drawing.Color.RoyalBlue;
            this.cmdBuy2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdBuy2.ForeColor = System.Drawing.Color.White;
            this.cmdBuy2.Location = new System.Drawing.Point(268, 83);
            this.cmdBuy2.Name = "cmdBuy2";
            this.cmdBuy2.Size = new System.Drawing.Size(72, 23);
            this.cmdBuy2.TabIndex = 56;
            this.cmdBuy2.TabStop = false;
            this.cmdBuy2.Text = "Buy";
            this.cmdBuy2.UseVisualStyleBackColor = false;
            this.cmdBuy2.Click += new System.EventHandler(this.cmdBuy2_Click);
            // 
            // cmdSell2
            // 
            this.cmdSell2.BackColor = System.Drawing.Color.Crimson;
            this.cmdSell2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSell2.ForeColor = System.Drawing.Color.White;
            this.cmdSell2.Location = new System.Drawing.Point(378, 83);
            this.cmdSell2.Name = "cmdSell2";
            this.cmdSell2.Size = new System.Drawing.Size(72, 23);
            this.cmdSell2.TabIndex = 59;
            this.cmdSell2.TabStop = false;
            this.cmdSell2.Text = "Sell";
            this.cmdSell2.UseVisualStyleBackColor = false;
            this.cmdSell2.Click += new System.EventHandler(this.cmdSell2_Click);
            // 
            // txtSells2
            // 
            this.txtSells2.BackColor = System.Drawing.Color.White;
            this.txtSells2.ForeColor = System.Drawing.Color.Crimson;
            this.txtSells2.Location = new System.Drawing.Point(772, 58);
            this.txtSells2.Name = "txtSells2";
            this.txtSells2.ReadOnly = true;
            this.txtSells2.Size = new System.Drawing.Size(45, 23);
            this.txtSells2.TabIndex = 54;
            this.txtSells2.TabStop = false;
            this.txtSells2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLastVol2
            // 
            this.txtLastVol2.BackColor = System.Drawing.Color.Honeydew;
            this.txtLastVol2.Location = new System.Drawing.Point(565, 58);
            this.txtLastVol2.Name = "txtLastVol2";
            this.txtLastVol2.ReadOnly = true;
            this.txtLastVol2.Size = new System.Drawing.Size(34, 23);
            this.txtLastVol2.TabIndex = 37;
            this.txtLastVol2.TabStop = false;
            this.txtLastVol2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLastVolTotal2
            // 
            this.txtLastVolTotal2.BackColor = System.Drawing.Color.Honeydew;
            this.txtLastVolTotal2.Location = new System.Drawing.Point(601, 58);
            this.txtLastVolTotal2.Name = "txtLastVolTotal2";
            this.txtLastVolTotal2.ReadOnly = true;
            this.txtLastVolTotal2.Size = new System.Drawing.Size(72, 23);
            this.txtLastVolTotal2.TabIndex = 38;
            this.txtLastVolTotal2.TabStop = false;
            this.txtLastVolTotal2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdSave
            // 
            this.cmdSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSave.Location = new System.Drawing.Point(10, 452);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(168, 30);
            this.cmdSave.TabIndex = 40;
            this.cmdSave.TabStop = false;
            this.cmdSave.Text = "Save Selected Markets";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // lblSaveInfo
            // 
            this.lblSaveInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveInfo.Location = new System.Drawing.Point(185, 452);
            this.lblSaveInfo.Name = "lblSaveInfo";
            this.lblSaveInfo.Size = new System.Drawing.Size(444, 30);
            this.lblSaveInfo.TabIndex = 66;
            this.lblSaveInfo.Text = "Click Save to store the current markets in an XML file on the server.  The market" +
    "s will be loaded automatically on the next login.";
            this.lblSaveInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpOrders
            // 
            this.grpOrders.Controls.Add(this.lstOrders);
            this.grpOrders.Controls.Add(this.lblOrderInfo);
            this.grpOrders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpOrders.Location = new System.Drawing.Point(10, 489);
            this.grpOrders.Name = "grpOrders";
            this.grpOrders.Size = new System.Drawing.Size(831, 219);
            this.grpOrders.TabIndex = 67;
            this.grpOrders.TabStop = false;
            this.grpOrders.Text = "Orders";
            // 
            // lstOrders
            // 
            this.lstOrders.ItemHeight = 17;
            this.lstOrders.Location = new System.Drawing.Point(10, 24);
            this.lstOrders.Name = "lstOrders";
            this.lstOrders.Size = new System.Drawing.Size(808, 140);
            this.lstOrders.TabIndex = 60;
            this.lstOrders.TabStop = false;
            this.lstOrders.DoubleClick += new System.EventHandler(this.lstOrders_DoubleClick);
            // 
            // lblOrderInfo
            // 
            this.lblOrderInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderInfo.Location = new System.Drawing.Point(48, 192);
            this.lblOrderInfo.Name = "lblOrderInfo";
            this.lblOrderInfo.Size = new System.Drawing.Size(761, 20);
            this.lblOrderInfo.TabIndex = 67;
            this.lblOrderInfo.Text = "Double Click orders to Pull them.  Volume is displayed Filled/Working to clarify " +
    "which orders have been Pulled.";
            this.lblOrderInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpMarket2
            // 
            this.grpMarket2.Controls.Add(this.txtMarketDescription2);
            this.grpMarket2.Controls.Add(this.txtLastVolTotal2);
            this.grpMarket2.Controls.Add(this.txtLastVol2);
            this.grpMarket2.Controls.Add(this.lblMisc2);
            this.grpMarket2.Controls.Add(this.txtSells2);
            this.grpMarket2.Controls.Add(this.cmdRunMisc2);
            this.grpMarket2.Controls.Add(this.cmdSell2);
            this.grpMarket2.Controls.Add(this.cboMisc2);
            this.grpMarket2.Controls.Add(this.cmdBuy2);
            this.grpMarket2.Controls.Add(this.txtBuys2);
            this.grpMarket2.Controls.Add(this.txtNet2);
            this.grpMarket2.Controls.Add(this.cmdGet2);
            this.grpMarket2.Controls.Add(this.txtBid2);
            this.grpMarket2.Controls.Add(this.txtOffer2);
            this.grpMarket2.Controls.Add(this.txtBidVol2);
            this.grpMarket2.Controls.Add(this.txtLast2);
            this.grpMarket2.Controls.Add(this.txtOfferVol2);
            this.grpMarket2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMarket2.Location = new System.Drawing.Point(10, 324);
            this.grpMarket2.Name = "grpMarket2";
            this.grpMarket2.Size = new System.Drawing.Size(831, 121);
            this.grpMarket2.TabIndex = 68;
            this.grpMarket2.TabStop = false;
            this.grpMarket2.Text = "Market 2";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(879, 724);
            this.Controls.Add(this.grpMarket2);
            this.Controls.Add(this.grpOrders);
            this.Controls.Add(this.grpMarkets);
            this.Controls.Add(this.grpAccountPicker);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.lblSaveInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "T4 Example 2";
            this.grpAccountPicker.ResumeLayout(false);
            this.grpAccountPicker.PerformLayout();
            this.grpMarkets.ResumeLayout(false);
            this.grpMarkets.PerformLayout();
            this.grpOrders.ResumeLayout(false);
            this.grpMarket2.ResumeLayout(false);
            this.grpMarket2.PerformLayout();
            this.ResumeLayout(false);

        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            System.Windows.Forms.Application.Run(new Form1());
        }

        #endregion

        #region Delegates

        //internal delegate void Test();
        private delegate void OnAccountDetailsDelegate(T4.API.AccountList.UpdateList poAccounts);
        private delegate void OnAccountUpdateDelegate(T4.API.AccountList.UpdateList poAccounts);
        private delegate void OnPositionUpdateDelegate(T4.API.Position poPosition);
        private delegate void OnMarketDepthUpdateDelegate(Market poMarket);
        private delegate void OnAccountOrderUpdateDelegate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders);
        private delegate void OnAccountOrderAddedDelegate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders);
        private delegate void OnAccountListCompleteDelegate(T4.API.AccountList poAccounts);

        #endregion

        #region Member Variables

        // Reference to the main api host object.
        internal Host moHost;

        //  Reference to the current account.
        internal Account moAccount;

        // Reference to the exchange list.
        internal ExchangeList moExchanges;

        // Reference to the current exchange.
        internal Exchange moExchange;

        // Reference to an exchange's contract list.
        internal ContractList moContracts;

        // Reference to the current contract.
        internal Contract moContract;

        // Reference to a contract's market list.
        internal MarketList moPickerMarkets;

        // Reference to the current market.
        internal Market moPickerMarket;

        // Market1 filter.
        internal MarketList moMarkets1Filter;
        internal MarketList moMarkets2Filter;

        // References to selected markets.
        internal Market moMarket1;
        internal Market moMarket2;

        // References to marketid's retrieved from saved settings.
        internal string mstrMarketID1;
        internal string mstrMarketID2;

        // Reference to the accounts list.
        internal AccountList moAccounts;

        // Reference to Order arraylist.
        // Stores the collection of orders.
        internal ArrayList moOrderArrayList = new ArrayList();

        #endregion

        #region " Initialization "

        // Initialize the application.
        private void Init()
        {

            Trace.WriteLine("Init");

            // Populate the available exchanges.
            moExchanges = moHost.MarketData.Exchanges;

            // Register the exchangelist events.
            moExchanges.ExchangeListComplete += new T4.API.ExchangeList.ExchangeListCompleteEventHandler(moExchanges_ExchangeListComplete);

            // Check to see if the data is already loaded.
            if (moExchanges.Complete)
            {
                // Call the event handler ourselves as the data is 
                // already loaded.
                moExchanges_ExchangeListComplete(moExchanges);

            }

            // Set the account list reference so that we can get 
            // Account and order events.
            moAccounts = moHost.Accounts;

            // Register the accountlist events.
            moAccounts.AccountDetails += new T4.API.AccountList.AccountDetailsEventHandler(moAccounts_AccountDetails);
            moAccounts.PositionUpdate += new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
            moAccounts.AccountUpdate += new T4.API.AccountList.AccountUpdateEventHandler(moAccounts_AccountUpdate);
            moAccounts.PositionUpdate += new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
            moAccounts.AccountListComplete += new T4.API.AccountList.AccountListCompleteEventHandler(moAccounts_AccountListComplete);

            if (moAccounts.Complete)
            {

                moAccounts_AccountListComplete(moAccounts);

            }

            try
            {

                // Read saved markets.
                // XML Doc.
                XmlDocument oDoc;

                // XML Nodes for viewing the doc.
                XmlNode oMarkets;

                // Temporary string variables for referencing contract and exchange details.
                string strContractID;
                string strExchangeID;


                // Pull the xml doc from the server.
                oDoc = moHost.UserSettings;

                // Reference the saved markets via xml node.
                oMarkets = oDoc.ChildNodes[0];

                // Load the saved markets.
                foreach (XmlNode oMarket in oMarkets)
                {

                    // Check each child node for existance of saved markets.
                    switch (oMarket.Name)
                    {
                        case "market1":
                            {

                                mstrMarketID1 = oMarket.Attributes["MarketID"].Value;
                                strExchangeID = oMarket.Attributes["ExchangeID"].Value;
                                strContractID = oMarket.Attributes["ContractID"].Value;

                                // Create a market filter for the desired exchange and contract.
                                moMarkets1Filter = moHost.MarketData.CreateMarketFilter(strExchangeID, strContractID, 0, T4.ContractType.Any, T4.StrategyType.Any);

                                // Register the events.
                                moMarkets1Filter.MarketListComplete += new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets1Filter_MarketListComplete);

                                if (moMarkets1Filter.Complete)
                                {
                                    // Call the event handler directly as the list is already complete.
                                    moMarkets1Filter_MarketListComplete(moMarkets1Filter);

                                }
                                break;
                            }
                        case "market2":
                            {

                                mstrMarketID2 = oMarket.Attributes["MarketID"].Value;
                                strExchangeID = oMarket.Attributes["ExchangeID"].Value;
                                strContractID = oMarket.Attributes["ContractID"].Value;

                                //Create a market filter for the desired exchange and contract.
                                moMarkets2Filter = moHost.MarketData.CreateMarketFilter(strExchangeID, strContractID, 0, T4.ContractType.Any, T4.StrategyType.Any);

                                // Register the events.
                                moMarkets2Filter.MarketListComplete += new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets2Filter_MarketListComplete);

                                if (moMarkets2Filter.Complete)
                                {
                                    // Call the event handler directly as the list is already complete.
                                    moMarkets2Filter_MarketListComplete(moMarkets2Filter);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trace the exception.
                Trace.WriteLine("Error: " + ex.ToString());
            }
        }
        
        #endregion 

        #region Market Filters
                     
        private void moMarkets1Filter_MarketListComplete(T4.API.MarketList poMarketList)
        {
            // Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired )
                this.BeginInvoke(new MethodInvoker(Markets1ListComplete));
            else
                Markets1ListComplete();
        }

        private void Markets1ListComplete()
        {
            // Reference the desired market.
            Market oMarket1 = moMarkets1Filter[mstrMarketID1];

            // Subscribe to market1.
            NewMarketSubscription(ref moMarket1,ref  oMarket1 );

        }

        private void moMarkets2Filter_MarketListComplete(T4.API.MarketList poMarketList)
        {
            // Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(Markets2ListComplete));
            else
                Markets2ListComplete();
        }

        private void Markets2ListComplete()
        {
            // Reference the desired market.
            Market oMarket2 = moMarkets2Filter[mstrMarketID2];

            // Subscribe to market1.
            NewMarketSubscription(ref moMarket2, ref moPickerMarket);
        }


        #endregion

        #region Market Picker

        private void moExchanges_ExchangeListComplete(T4.API.ExchangeList poExchangeList)
        {
            // Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ExchangeListComplete));
            }
            else
            {
                ExchangeListComplete();
            }
        }

        private void ExchangeListComplete()
        {
            // First clear all the combo's.
            cboExchanges.Items.Clear();
            cboContracts.Items.Clear();
            cboMarkets.Items.Clear();

            // Eliminate any previous references.
            moExchange = null;
            moContracts = null;
            moContract = null;
            moPickerMarkets = null;
            moPickerMarket = null;

            // Populate the list of exchanges.

            if ((moExchanges != null))
            {

                try
                {
                    // Lock the API while traversing the api collection.
                    // Lock at the lowest level object for the shortest period of time.
                    moHost.EnterLock("ExchangeList");

                    // Add the exchanges to the dropdown list.
                    foreach (Exchange oExchange in moExchanges)
                    {
                        //  cboExchanges.Items.Add(New ExchangeItem(oExchange))
                        cboExchanges.Items.Add(oExchange);
                    }


                }
                catch (Exception ex)
                {
                    // Trace the error.
                    Trace.WriteLine("Error " + ex.ToString());


                }
                finally
                {
                    // This is guarenteed to execute last.
                    moHost.ExitLock("ExchangeList");

                }

            }

        }

        private void cboExchanges_SelectedIndexChanged(Object sender, System.EventArgs e)
        {

            // Populate the current exchange's available contracts.
            if (cboExchanges.SelectedItem != null)
            {

                // Reference the current exchange.
                moExchange = ((Exchange)(cboExchanges.SelectedItem));

                // Unregister previous events.
                if (moContracts != null)
                {
                    moContracts.ContractListComplete -= new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);
                }

                // Reference the exchange's available contracts.
                moContracts = moExchange.Contracts;

                // Register the events.
                if (moContracts != null)
                {
                    moContracts.ContractListComplete += new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);
                }

                // Check to see if the data is already loaded.
                if (moContracts.Complete)
                {
                    // Call the event handler ourselves as the data is 
                    // already loaded.
                    moContracts_ContractListComplete(moContracts);
                }
            }
        }


        private void moContracts_ContractListComplete(T4.API.ContractList poContractList)
        {
            // Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ContractListComplete));
            }
            else
            {
                ContractListComplete();
            }
        }

        private void ContractListComplete()
        {
            // Populate the list of contracts available for the current exchange.

            // First clear all the combo's.
            cboContracts.Items.Clear();
            cboMarkets.Items.Clear();

            // Eliminate any previous references.
            moContract = null;
            moPickerMarkets = null;
            moPickerMarket = null;


            if ((moContracts != null))
            {

                try
                {
                    // Lock the API while traversing the api collection.
                    // Lock at the lowest level object for the shortest period of time.
                    moHost.EnterLock("ContractList");

                    // Add the exchanges to the dropdown list.
                    foreach (Contract oContract in moContracts)
                    {
                        cboContracts.Items.Add(oContract);
                    }


                }
                catch (Exception ex)
                {
                    // Trace the error.
                    Trace.WriteLine("Error " + ex.ToString());


                }
                finally
                {
                    // This is guarenteed to execute last.
                    moHost.ExitLock("ContractList");

                }

            }

        }

        private void cboContracts_SelectedIndexChanged(Object sender, System.EventArgs e)
        {

            // Populate the current contract's available markets.

            {

                if ((cboContracts.SelectedItem != null))
                {
                    // Reference the current contract.
                    moContract = (Contract)cboContracts.SelectedItem;

                    // This would return all markets for the contract.
                    // moPickerMarkets = moContract.Markets

                    // This will return outright futures only.
                    moPickerMarkets = moHost.MarketData.CreateMarketFilter(moContract.ExchangeID, moContract.ContractID, 0, ContractType.Future, StrategyType.None);
                    
                    // Register the events.
                    if (moPickerMarkets != null)
                    {
                        moPickerMarkets.MarketListComplete += new T4.API.MarketList.MarketListCompleteEventHandler(moPickerMarkets_MarketListComplete);
                    }

                    // Check to see if the data is already loaded.
                    if (moPickerMarkets.Complete)
                    {
                        // Call the event handler ourselves as the data is 
                        // already loaded.
                        moPickerMarkets_MarketListComplete(moPickerMarkets);

                    }

                }
            }

        }

        private void moPickerMarkets_MarketListComplete(T4.API.MarketList poMarketList)
        {

            // Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(MarketListComplete));
            }
            else
            {
                MarketListComplete();
            }
        }

        private void MarketListComplete()
        {
            // Populate the list of markets available for the current contract.

            // First clear the combo.
            cboMarkets.Items.Clear();

            // Eliminate any previous references.
            moPickerMarket = null;


            if ((moPickerMarkets != null))
            {

                try
                {
                    // Lock the API while traversing the api collection.
                    // Lock at the lowest level object for the shortest period of time.
                    moHost.EnterLock("MarketList");

                    // Create a sorted list of the markets.
                    // Remember to turn sorting off on the combo or it will do a text sort.
                    System.Collections.Generic.SortedList<int, Market> oSortedList = new System.Collections.Generic.SortedList<int, Market>();
                    
                    foreach (Market oMarket in moPickerMarkets)
                    {
                        oSortedList.Add(oMarket.ExpiryDate, oMarket);

                    }

                    // Add the exchanges to the dropdown list.

                    foreach (Market oMarket in oSortedList.Values)
                    {
                        cboMarkets.Items.Add(oMarket);

                    }


                }
                catch (Exception ex)
                {
                    // Trace the error.
                    Trace.WriteLine("Error " + ex.ToString());


                }
                finally
                {
                    // This is guarenteed to execute last.
                    moHost.ExitLock("MarketList");

                }

            }

        }


        private void cboMarkets_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            if (cboMarkets.SelectedItem != null)
            {
                // Store a reference to the current market.
                moPickerMarket = ((Market)(cboMarkets.SelectedItem));
            }
        }

        #endregion

        #region Account Data

              // Event that is raised when details for an account have 
        // changed, or a new account is recieved.
        private void moAccounts_AccountDetails(T4.API.AccountList.UpdateList poAccounts)
        {

            //
            //  Invoke the update.
            //  This places process on GUI thread.
            //  Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountDetailsDelegate(OnAccountDetails), new Object[] { poAccounts });
            }
            else
            {
                OnAccountDetails(poAccounts);
            }
        }

        private void OnAccountDetails(AccountList.UpdateList poAccounts)
        {

            // Display the account list.
            foreach (Account oAccount in poAccounts)
            {
                // Check to see if the account exists prior to adding/subscribing to it.
                if (oAccount.Subscribed != true)
                {

                    // Add the account to the list.
                    cboAccounts.Items.Add(oAccount);

                    // Subscribe to the account.
                    oAccount.Subscribe();

                }
            }
        }

        // Event that is raised when the accounts overall balance,
        // P&L or margin details have changed.
        private void moAccounts_AccountUpdate(T4.API.AccountList.UpdateList poAccounts)
        {
            // Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.

            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountUpdateDelegate(OnAccountUpdate), new Object[] { poAccounts });
            }
            else
            {
                OnAccountUpdate(poAccounts);
            }

        }
        
        private void OnAccountUpdate(T4.API.AccountList.UpdateList poAccounts)
        {
            // Just refresh the current account.
            DisplayAccount(moAccount);

        }

        // Event that is raised when the account list is loaded.
        private void moAccounts_AccountListComplete(T4.API.AccountList poAccounts)
        {
            // Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.

            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountListCompleteDelegate(OnAccountListComplete), new Object[] { poAccounts });
            }
            else
            {
                OnAccountListComplete(poAccounts);
            }


        }

        private void OnAccountListComplete(T4.API.AccountList poAccounts)
        {

            try
            {
                // Lock the API.
                moHost.EnterLock("OnAccountListComplete");

                // Display the account list.

                foreach (Account oAccount in moHost.Accounts)
                {
                    // Add the account to the combo.
                    cboAccounts.Items.Add(oAccount);

                    // Subscribe to the account.
                    oAccount.Subscribe();

                }

                if (cboAccounts.Items.Count > 0)
                {
                    cboAccounts.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                // Trace Errors.
                Trace.WriteLine(ex.ToString());
            }
            finally
            {
                // Unlock the api.
                moHost.ExitLock("OnAccountListComplete");
            }

        }

        //' Event that is raised when positions for accounts have changed.
        private void moAccounts_PositionUpdate(AccountList.PositionUpdateList poPositions)
        {

            // Display the position details.

            {

                foreach (AccountList.PositionUpdateList.PositionUpdate oUpdate in poPositions)
                {
                    // If the position is for the current account
                    // then update the value.

                    if (object.ReferenceEquals(oUpdate.Account, moAccount))
                    {
                        // Invoke the update.
                        // This places process on GUI thread.
                        // Must use a delegate to pass arguments.
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new OnPositionUpdateDelegate(OnPositionUpdate), new object[] { oUpdate.Position });
                        }
                        else
                        {
                            OnPositionUpdate(oUpdate.Position);
                        }

                        break; // TODO: might not be correct. Was : Exit For

                    }

                }
            }

        }

        private void OnPositionUpdate(T4.API.Position poPosition)
        {

            if (object.ReferenceEquals(poPosition.Market, moMarket1))
            {
                // Display the position details.
                DisplayPosition(poPosition.Market, 1);

            }
            else if (object.ReferenceEquals(poPosition.Market, moMarket2))
            {

                // Display the position details.
                DisplayPosition(poPosition.Market, 2);

            }

        }


                private void DisplayAccount(Account poAccount)
                {

                    if ((moAccount != null))
                    {

                        try
                        {
                            // Lock the host while we retrive details.
                            moHost.EnterLock("DisplayAccount");

                            // Display the current account balance.
                            txtCash.Text = String.Format("{0:#,###,##0.00}", moAccount.AvailableCash);

                        }
                        catch (Exception ex)
                        {
                            // Trace the error.
                            Trace.WriteLine("Error: " + ex.ToString());

                        }
                        finally
                        {
                            // Unlock the host object.
                            moHost.ExitLock("DisplayAccount");

                        }

                    }

                }
        
                private void DisplayPosition(Market poMarket, int piID)
                {
                    string strNet = "";
                    string strBuys = "";
                    string strSells = "";

                    bool blnLocked = false;
                    
                    try
                    {

                        if ((poMarket != null) && (moAccount != null))
                        {
                            // Lock the host while we retrive details.
                            moHost.EnterLock("DisplayPositions");

                            // Update the locked flag.
                            blnLocked = true;

                            // Temporary position object used for referencing the account's positions.
                            Position oPosition = default(Position);

                            // Display positions for current account and market1.

                            // Reference the market's positions.
                            oPosition = moAccount.Positions[poMarket.MarketID];

                            if ((oPosition != null))
                            {
                                // Reference the net position.
                                strNet = oPosition.Net.ToString();
                                strBuys = oPosition.Buys.ToString();
                                strSells = oPosition.Sells.ToString();
                            }

                            switch (piID)
                            {
                                case 1:

                                    // Display the net position.
                                    txtNet1.Text = strNet;
                                    // Display the total Buys.
                                    txtBuys1.Text = strBuys;
                                    // Display the total Sells.
                                    txtSells1.Text = strSells;

                                    break;
                                case 2:

                                    // Display the net position.
                                    txtNet2.Text = strNet;
                                    // Display the total Buys.
                                    txtBuys2.Text = strBuys;
                                    // Display the total Sells.
                                    txtSells2.Text = strSells;

                                    break;
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        // Trace the error.
                        Trace.WriteLine("Error " + ex.ToString());

                    }
                    finally
                    {
                        // Unlock the host object.
                        if (blnLocked)
                            moHost.ExitLock("DisplayPositions");

                    }
                    
                }

        private void cboAccounts_SelectedIndexChanged(Object sender, System.EventArgs e)
        {

            if ((cboAccounts.SelectedItem != null))
            {
                // Reference the current account.
                moAccount = (Account)cboAccounts.SelectedItem;

                // Register the account's events.
                if (moAccount != null)
                {
                    moAccount.OrderAdded += new T4.API.Account.OrderAddedEventHandler(moAccount_OrderAdded);
                    moAccount.OrderUpdate += new T4.API.Account.OrderUpdateEventHandler(moAccount_OrderUpdate);
                }

                // Display the current account balance.
                DisplayAccount(moAccount);

                // Refresh positions.
                DisplayPosition(moMarket1, 1);
                DisplayPosition(moMarket2, 2);

            }

        }


        #endregion

        #region Startup and shutdown code

        // Initialise the api when the application starts.
        private void frmMain_Load(object sender, System.EventArgs e)
        {

            moHost = Host.Login(APIServerType.Simulator, "T4Example", "112A04B0-5AAF-42F4-994E-FA7CB959C60B");

            // Check for success.

            if (moHost == null)
            {
                // Host object not returned which means the user cancelled the login dialog.
                this.Close();
                
            }
            else
            {
                // Login was successfull.
                Trace.WriteLine("Login Success");

                // Initialize.
                Init();

            }
            
        }

        // Shutdown the api when the application exits.
        private void frmMain_Closed(object sender, System.EventArgs e)
        {

            // Check to see that we have an api object.
            if (moHost != null)
            {

                // Unregister events.

                // Markets.
                if (moMarket1 != null)
                {
                    moMarket1.MarketCheckSubscription -= new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                    moMarket1.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                }
                if (moMarket2 != null)
                {
                    moMarket2.MarketCheckSubscription -= new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                    moMarket2.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                }

                // Market Filters.
                if (moMarkets1Filter != null)
                {
                    moMarkets1Filter.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets1Filter_MarketListComplete);
                }
                if (moMarkets2Filter != null)
                {
                    moMarkets2Filter.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets2Filter_MarketListComplete);
                }

                // Account events.
                if (moAccounts != null)
                {
                    moAccounts.AccountDetails -= new T4.API.AccountList.AccountDetailsEventHandler(moAccounts_AccountDetails);
                    moAccounts.PositionUpdate -= new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
                    moAccounts.AccountUpdate -= new T4.API.AccountList.AccountUpdateEventHandler(moAccounts_AccountUpdate);
                    moAccounts.PositionUpdate -= new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
                }

                if (moAccount != null)
                {
                    moAccount.OrderAdded -= new T4.API.Account.OrderAddedEventHandler(moAccount_OrderAdded);
                    moAccount.OrderUpdate -= new T4.API.Account.OrderUpdateEventHandler(moAccount_OrderUpdate);
                }

                // Exchange list events.
                if (moExchanges != null)
                {
                    moExchanges.ExchangeListComplete -= new T4.API.ExchangeList.ExchangeListCompleteEventHandler(moExchanges_ExchangeListComplete);
                }

                // Contract list events.
                if (moContracts != null)
                {
                    moContracts.ContractListComplete -= new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);
                }

                // Market list events.
                if (moPickerMarkets != null)
                {
                    moPickerMarkets.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moPickerMarkets_MarketListComplete);
                }

                // Market events.
                if (moMarket1 != null)
                {
                    // Register to the events.
                    moMarket1.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                }

                if (moMarket2 != null)
                {
                    // Register to the events.
                    moMarket2.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                }

                // Host events.
                if (moHost != null)
                {

                    // Dispose of the api.
                    moHost.Dispose();
                    moHost = null;
                }
            }
        }

        #endregion

        #region Market Subscription 


        private void cmdGet1_Click(System.Object sender, System.EventArgs e)
        {

            // Clear the values.
            DisplayMarketDetails(null, 1);

            // Subscribe to market1.
            NewMarketSubscription(ref moMarket1, ref moPickerMarket);

            // Refresh the positions.
            DisplayPosition(moMarket1, 1);

        }


        private void cmdGet2_Click(System.Object sender, System.EventArgs e)
        {
            Market oMarket = moHost.MarketData.MarketPicker(ref moMarket2);

            // Clear the values.
            DisplayMarketDetails(null, 2);

            // Subscribe to market2.
            NewMarketSubscription(ref moMarket2, ref oMarket);

            // Refresh the positions.
            DisplayPosition(moMarket2,2);

        }

        private void NewMarketSubscription(ref Market poMarket, ref Market poNewMarket)
        {
            // Update an existing market reference to subscribe to a new/different market.

            // If they are the same then don't do anything.
            // We don't need to resubscribe to the same market.

            // Explicitly register events as opposed to declaring withevents.
            // This gives us more control.  
            // It is important to unregister the marketchecksubscription prior to unsubscribing or the event will override and maintain the subscription.


            if ((!object.ReferenceEquals(poMarket, poNewMarket)))
            {
                // Unsubscribe from the currently selected market.
                if ((poMarket != null))

                    {
                        // Unregister the events for this market.
                        poMarket.MarketCheckSubscription -= new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                        poMarket.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                    
                        poMarket.DepthUnsubscribe();

                    }
                
                    // Update the market reference.
                    poMarket = poNewMarket;
                
                    if ((poMarket != null))
                    {

                        // Register the events.
                        poMarket.MarketCheckSubscription += new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                        poMarket.MarketDepthUpdate += new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                    
                        // Subscribe to the market.
                        // Use smart buffering.
                        poMarket.DepthSubscribe(DepthBuffer.Smart, DepthLevels.BestOnly);

                    }

            }

        }
                
        private void Markets_MarketCheckSubscription(T4.API.Market poMarket, ref T4.DepthBuffer penDepthBuffer, ref T4.DepthLevels penDepthLevels)
        {
            // No need to invoke on the gui thread.
            penDepthBuffer = poMarket.DepthSubscribeAtLeast(DepthBuffer.Smart, penDepthBuffer);
            penDepthLevels = poMarket.DepthSubscribeAtLeast(DepthLevels.BestOnly, penDepthLevels);

        }
        
        private void Markets_MarketDepthUpdate(T4.API.Market poMarket)
        {
            // Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired )
            {
                this.BeginInvoke(new OnMarketDepthUpdateDelegate(OnMarketDepthUpdate), new object[] {poMarket});
            }
            else
            {
                OnMarketDepthUpdate(poMarket);
            }

        }
        
        private void OnMarketDepthUpdate(Market poMarket)
        {

            try
            {

                if (object.ReferenceEquals(poMarket, moMarket1))
                {
                    DisplayMarketDetails(poMarket, 1);


                }
                else if (object.ReferenceEquals(poMarket, moMarket2))
                {
                    DisplayMarketDetails(poMarket, 2);

                }


            }
            catch (Exception ex)
            {
                // Trace the error.
                Trace.WriteLine("Error " + ex.ToString());

            }

        }

        /// <summary>
        /// Update the market display values.
        /// </summary>

        private void DisplayMarketDetails(Market poMarket, int piID)
        {
            string strDescription = "";
            string strBid = "";
            string strBidVol = "";
            string strOffer = "";
            string strOfferVol = "";
            string strLast = "";
            string strLastVol = "";
            string strLastVolTotal = "";


            if ((poMarket != null))
            {

                try
                {
                    // Lock the host while we retrive details.
                    moHost.EnterLock("DisplayMarketDetails");

                    // Display the market description.
                    strDescription = poMarket.Description;


                    if ((poMarket.LastDepth != null))
                    {
                        // Best bid.
                        if (poMarket.LastDepth.Bids.Count > 0)
                        {
                            strBid = poMarket.ConvertTicksDisplay(poMarket.LastDepth.Bids[0].Ticks);
                            strBidVol = poMarket.LastDepth.Bids[0].Volume.ToString();
                        }

                        // Best offer.
                        if (poMarket.LastDepth.Offers.Count > 0)
                        {
                            strOffer = poMarket.ConvertTicksDisplay(poMarket.LastDepth.Offers[0].Ticks);
                            strOfferVol = poMarket.LastDepth.Offers[0].Volume.ToString();
                        }

                        // Last trade.
                        strLast = poMarket.ConvertTicksDisplay(poMarket.LastDepth.LastTradeTicks);
                        strLastVol = poMarket.LastDepth.LastTradeVolume.ToString();
                        strLastVolTotal = poMarket.LastDepth.LastTradeTotalVolume.ToString();

                    }

                }
                catch (Exception ex)
                {
                    // Trace the error.
                    Trace.WriteLine("Error " + ex.ToString());

                }
                finally
                {
                    // Unlock the host object.
                    moHost.ExitLock("DisplayMarketDetails");

                }

            }

            switch (piID)
            {
                case 1:

                    // Update the market1 display values.
                    txtMarketDescription1.Text = strDescription;
                    txtBid1.Text = strBid;
                    txtBidVol1.Text = strBidVol;
                    txtOffer1.Text = strOffer;
                    txtOfferVol1.Text = strOfferVol;
                    txtLast1.Text = strLast;
                    txtLastVol1.Text = strLastVol;
                    txtLastVolTotal1.Text = strLastVolTotal;

                    break;
                case 2:

                    // Update the market2 display values.
                    txtMarketDescription2.Text = strDescription;
                    txtBid2.Text = strBid;
                    txtBidVol2.Text = strBidVol;
                    txtOffer2.Text = strOffer;
                    txtOfferVol2.Text = strOfferVol;
                    txtLast2.Text = strLast;
                    txtLastVol2.Text = strLastVol;
                    txtLastVolTotal2.Text = strLastVolTotal;

                    break;
            }

        }

                #endregion

        #region Save Settings

        private void cmdSave_Click(System.Object sender, System.EventArgs e)
        {
            try
            {

                // XML Doc.
                XmlDocument oDoc = new XmlDocument();

                // XML Node.
                XmlNode oMarket;
                XmlNode oMarkets;
                XmlAttribute oAttribute;

                // Create the main node.
                oMarkets = oDoc.CreateNode(XmlNodeType.Element, "markets", "");
                oDoc.AppendChild(oMarkets);

                if (moMarket1 != null)
                {

                    // Create a node.
                    oMarket = oDoc.CreateNode(XmlNodeType.Element, "market1", "");

                    // Exchange ID.
                    oAttribute = oDoc.CreateAttribute("ExchangeID");
                    oAttribute.Value = moMarket1.ExchangeID;
                    oMarket.Attributes.Append(oAttribute);

                    // Contract ID.
                    oAttribute = oDoc.CreateAttribute("ContractID");
                    oAttribute.Value = moMarket1.ContractID;
                    oMarket.Attributes.Append(oAttribute);

                    // Market ID.
                    oAttribute = oDoc.CreateAttribute("MarketID");
                    oAttribute.Value = moMarket1.MarketID;
                    oMarket.Attributes.Append(oAttribute);

                    // Add the node to the xml document.
                    oMarkets.AppendChild(oMarket);
                }

                if (moMarket2 != null)
                {

                    // Create a node.
                    oMarket = oDoc.CreateNode(XmlNodeType.Element, "market2", "");

                    // Exchange ID.
                    oAttribute = oDoc.CreateAttribute("ExchangeID");
                    oAttribute.Value = moMarket2.ExchangeID;
                    oMarket.Attributes.Append(oAttribute);

                    // Contract ID.
                    oAttribute = oDoc.CreateAttribute("ContractID");
                    oAttribute.Value = moMarket2.ContractID;
                    oMarket.Attributes.Append(oAttribute);

                    // Market ID.
                    oAttribute = oDoc.CreateAttribute("MarketID");
                    oAttribute.Value = moMarket2.MarketID;
                    oMarket.Attributes.Append(oAttribute);

                    // Add the node to the xml document.
                    oMarkets.AppendChild(oMarket);

                }

                // Save the xml to the server.
                moHost.UserSettings = oDoc;
                moHost.SaveUserSettings();
            }
            catch (Exception ex)
            {
                // Trace.
                Trace.WriteLine(ex.ToString());
            }

        }

        public string App_Path()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }


        #endregion

        #region Single Order

        // Method that submits a single order.
        private void SubmitSingleOrder(Market poMarket, BuySell peBuySell, Double pdblLimitPrice)
        {
            if (moAccount != null && poMarket != null)
            {

                // Submit an order.
                Order oOrder = moAccounts.SubmitNewOrder(
                    moAccount,
                    poMarket,
                    peBuySell,
                    PriceType.Limit,
                    TimeType.Normal,
                    1,
                    pdblLimitPrice);

                // Add the order to the arraylist.
                AddOrder(oOrder);

                // Display the orders.
                DisplayOrders();

            }
        }

        // Pull the single order that was submitted.
        private void PullSingleOrder(Order poOrder)
        {
            // Check to see that we have an order.
            if (poOrder != null)
            {
                // Check to see if the order is working.
                if (poOrder.IsWorking)
                {
                    // Pull the order.
                    poOrder.Pull();
                }
            }
        }


        #endregion

        #region Submission/Cancelation

        private void cmdBuy1_Click(System.Object sender, System.EventArgs e)
        {
            // Submit a single order.
            if (txtBid1.Text != "")
            {
                SubmitSingleOrder(moMarket1, BuySell.Buy, System.Convert.ToDouble(txtBid1.Text));
            }
        }
        private void cmdSell1_Click(System.Object sender, System.EventArgs e)
        {
            // Submit a single order.
            if (txtOffer1.Text != "")
            {
                SubmitSingleOrder(moMarket1, BuySell.Sell, System.Convert.ToDouble(txtOffer1.Text));
            }
        }

        private void cmdSell2_Click(System.Object sender, System.EventArgs e)
        {
            // Submit a single order.
            if (txtOffer2.Text != "")
            {
                SubmitSingleOrder(moMarket2, BuySell.Sell, System.Convert.ToDouble(txtOffer2.Text));
            }
        }

        private void cmdBuy2_Click(System.Object sender, System.EventArgs e)
        {
            // Submit a single order.
            if (txtBid2.Text != "")
            {
                SubmitSingleOrder(moMarket2, BuySell.Buy, System.Convert.ToDouble(txtBid2.Text));
            }
        }

        private void lstOrders_DoubleClick(Object sender, System.EventArgs e)
        {

            // Pull the order that has been double clicked on.
            int iOrderIndex;

            // Be sure that the selected index is valid.
            if (lstOrders.SelectedIndex >= 0 & lstOrders.SelectedIndex <= lstOrders.Items.Count - 1)
            {

                // The orders were listed in reverse so we need 
                // to calculate the index of the order within the arraylist.
                iOrderIndex = (lstOrders.Items.Count - lstOrders.SelectedIndex - 1);

                // Reference the order in the collection.
                Order oOrder = (Order)(moOrderArrayList[iOrderIndex]);

                // Attempt to pull the order.
                PullSingleOrder(oOrder);
            }
        }

        #endregion

        #region  Order Data

        private void moAccount_OrderUpdate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            // Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {

                this.BeginInvoke(new OnAccountOrderUpdateDelegate(OnAccountOrderUpdate), new Object[] { poAccount, poPosition, poOrders });
            }
            else
            {
                OnAccountOrderUpdate(poAccount, poPosition, poOrders);
            }
        }

        private void moAccount_OrderAdded(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            // Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
                this.BeginInvoke(new OnAccountOrderAddedDelegate(OnAccountOrderAdded), new Object[] { poAccount, poPosition, poOrders });
            else
                OnAccountOrderAdded(poAccount, poPosition, poOrders);
        }

        private void OnAccountOrderUpdate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            // Redraw the order list.
            DisplayOrders();
        }

        private void OnAccountOrderAdded(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            // Add all the orders to the arraylist.
            foreach (Order oOrder in poOrders)
            {
                // Add the order.
                AddOrder(oOrder);
            }

            // Redraw the order list.
            DisplayOrders();
        }

        private void AddOrder(Order poOrder)
        {
            // Add the order to the arraylist.
            if (poOrder != null)
            {
                if (moOrderArrayList.Contains(poOrder) == false)
                {
                    // Add the order to the arraylist.
                    moOrderArrayList.Add(poOrder);
                }
            }
        }


        private void DisplayOrders()
        {
            try
            {

                // Lock the api.
                moHost.EnterLock();

                // Suspend the layout of the listbox.
                lstOrders.SuspendLayout();

                // Clear and repopulate the list.
                lstOrders.Items.Clear();

                // Temporary order object.
                Order oOrder;

                // Itterate through the collection backwards.
                for (int i = moOrderArrayList.Count - 1; i >= 0; i--)
                {

                    // Reference an order.
                    oOrder = (Order)(moOrderArrayList[i]);

                    // Display some order details.
                    lstOrders.Items.Add(oOrder.Market.Description + "   " +
                        oOrder.BuySell.ToString() + "   " +
                        oOrder.TotalFillVolume + "/" + oOrder.CurrentVolume + " @ " +
                        oOrder.Market.ConvertTicksDisplay(oOrder.CurrentLimitTicks, false) + "   " +
                        oOrder.Status.ToString() + "   " +
                        oOrder.StatusDetail + "  " +
                        oOrder.SubmitTime);

                }
            }
            catch (Exception ex)
            {
                // Trace the error.
                Trace.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                // Unlock the api.
                moHost.ExitLock();

                // Resume layout of the listbox.
                lstOrders.ResumeLayout();
            }
        }

        #endregion

        #region Misc Examples

        const string AUTOOCO = "Submit Auto OCO";
        const string FIVETICKSOFF = "Work 5 Ticks Off Market";

        // Setup misc example combos.
        private void SetupMiscExamples()
        {
            // Add examples to combos.
            cboMisc1.Items.Add(AUTOOCO);
            cboMisc1.Items.Add(FIVETICKSOFF);

            cboMisc2.Items.Add(AUTOOCO);
            cboMisc2.Items.Add(FIVETICKSOFF);

            // Be sure the first items are selected.
            cboMisc1.SelectedIndex = 0;
            cboMisc2.SelectedIndex = 0;

        }

        private void cmdRunMisc1_Click(Object sender, System.EventArgs e)
        {
            if (moMarket1 != null)
            {
                switch (cboMisc1.Text)
                {
                    case AUTOOCO:
                        {
                            // Run autooco sample code.
                            SubmitAOCO(moMarket1, BuySell.Buy, txtBid1.Text);
                            break;
                        }
                    case FIVETICKSOFF:
                        {
                            // Run the five ticks off code.
                            SubmitFiveTicksOff(moMarket1, BuySell.Buy, txtBid1.Text);
                            break;
                        }
                }
            }
        }

        private void cmdRunMisc2_Click(Object sender, System.EventArgs e)
        {
            if (moMarket2 != null)
            {

                switch (cboMisc2.Text)
                {
                    case AUTOOCO:
                        {
                            // Run autooco sample code.
                            SubmitAOCO(moMarket2, BuySell.Sell, txtOffer2.Text);
                            break;
                        }
                    case FIVETICKSOFF:
                        {
                            // Run the five ticks off code.
                            SubmitFiveTicksOff(moMarket2, BuySell.Sell, txtOffer2.Text);
                            break;
                        }
                }
            }
        }

        #region Auto OCO

        // Simple example of how to submit and cancel an Auto OCO.
        private void SubmitAOCO(Market poMarket, BuySell peBuySell, string pstrLimitDisplayPrice)
        {
            if (moAccount != null && poMarket != null)
            {

                // Limit price reference.
                // Convert the limit price to a double.
                Double dblLimitPrice = System.Convert.ToDouble(pstrLimitDisplayPrice);

                // Create the batch submission object.
                OrderList.Submission oBatch;
                oBatch = moAccounts.SubmitOrders(moAccount, poMarket);

                // Set the order link.
                oBatch.OrderLink = OrderLink.AutoOCO;

                // Add an order to the batch.
                // This is the trigger order.
                Order oOrder1 = oBatch.Add(peBuySell,
                    PriceType.Limit,
                    TimeType.Normal,
                    1,
                    dblLimitPrice);

                if (peBuySell == BuySell.Buy)
                {

                    // Add an order to the batch.
                    // This is the sell limit of the oco above the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    Order oOrder2 = oBatch.Add(BuySell.Sell,
                        PriceType.Limit,
                        TimeType.Normal,
                        0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(5, 0, false), false));

                    // Add an order to the batch.
                    // This is the stop of the oco below the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    Order oOrder3 = oBatch.Add(BuySell.Sell,
                        PriceType.StopMarket,
                        TimeType.Normal,
                        0,
                        0.0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(-5, 0, false), false),
                        OpenClose.Undefined, "", 0, ActivationType.Immediate, "", 0, null, null, true, null, true);
                }
                else
                {

                    // Add an order to the batch.
                    // This is the buy limit of the oco below the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    Order oOrder2 = oBatch.Add(BuySell.Buy,
                        PriceType.Limit,
                        TimeType.Normal,
                        0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(-5, 0, false), false));

                    // Add an order to the batch.
                    // This is the buy stop of the oco above the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    Order oOrder3 = oBatch.Add(BuySell.Buy,
                        PriceType.StopMarket,
                        TimeType.Normal,
                        0, 0.0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(5, 0, false), false),
                        OpenClose.Undefined, "", 0, ActivationType.Immediate, "", 0, null, null, true, null, true);

                }


                // Submit the batch.
                oBatch.Submit();

                // Display the orders.
                DisplayOrders();


                // Pull may fail if attempted too soon.
                // Like 1 millisecond later.

                //// This is how you would cancel the batch.
                //Dim oBatchPull As OrderList.Pull = moAccounts.PullOrders(moAccount, poMarket)

                //// Add the orders to the pull.
                //oBatchPull.Add(oOrder1)
                //oBatchPull.Add(oOrder2)
                //oBatchPull.Add(oOrder3)

                //// Pull the batch.
                //oBatchPull.Pull()

                //// Add the orders to the arraylist.
                //AddOrder(oOrder1)
                //AddOrder(oOrder2)
                //AddOrder(oOrder3)


            }

        }

        #endregion

        #region  Work Order Five Ticks From Market

        // Place an order five ticks off the market.
        private void SubmitFiveTicksOff(Market poMarket, BuySell peBuySell, string pstrLimitDisplayPrice)
        {
            // Limit price reference.
            // Convert the limit price to a double.
            Double dblLimitPrice = System.Convert.ToDouble(pstrLimitDisplayPrice);

            // Convert the price to ticks.
            int iTicks = poMarket.ConvertPrice(dblLimitPrice, false);
            int iNewTicks;

            // Add or subtract five ticks from the current price depending on what side of the market we are.
            if (peBuySell == BuySell.Buy)
                iNewTicks = poMarket.TicksAdd(-5, iTicks, false);
            else
                iNewTicks = poMarket.TicksAdd(5, iTicks, false);

            Double iNewPrice = poMarket.ConvertTicks(iNewTicks, false);

            // Submit a single order five ticks off the market.
            SubmitSingleOrder(poMarket, peBuySell, iNewPrice);

        }

        #endregion

        #endregion

    }
}
