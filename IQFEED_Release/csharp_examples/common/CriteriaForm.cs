//-----------------------------------------------------------
//-----------------------------------------------------------
//
//             System: SYS097
//       Program Name: Option Chain Examples
//        Module Name: CriteriaForm.cs
//
//-----------------------------------------------------------
//
//            Proprietary Software Product
//
//                    Telvent DTN
//           9110 West Dodge Road Suite 200
//               Omaha, Nebraska  68114
//
//          Copyright (c) by Schneider Electric 2015
//                 All Rights Reserved
//
//
//-----------------------------------------------------------
// Module Description: Implementation of Option Chains Criteria
//         References: None
//           Compiler: Microsoft Visual Studio Version 2010
//             Author: Steven Schumacher
//        Modified By: 
//
//-----------------------------------------------------------
//-----------------------------------------------------------
//
// REVISION HISTORY
//$Archive$
//$Author$
//$Date$
//$Log$
//$Modtime$
//$Revision$
//$Workfile$
//
//-----------------------------------------------------------
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OptionChainCriteria
{
    public partial class CriteriaForm : Form
    {
        /// <summary>
        /// Constructor for the form
        /// </summary>
        public CriteriaForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the form load event.  
        ///     It controls updating the form controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CriteriaForm_Load(object sender, EventArgs e)
        {
            nudEndYear.Minimum = DateTime.Now.Year;
            nudEndYear.Maximum = DateTime.Now.Year + 10;
            nudStartYear.Minimum = DateTime.Now.Year;
            nudStartYear.Maximum = DateTime.Now.Year + 10;
            chkNears.Checked = true;
            rdbEquityOptions.Checked = true;
        }

        /// <summary>
        /// Property of the Criteria form class that will contain the years string based upon user input
        /// </summary>
        public string Years
        {
            get
            {
                string sReturn = "";
                if (nudStartYear.Enabled)
                {
                    int i = 0;
                    for (i = Convert.ToInt32(nudStartYear.Value); i <= nudEndYear.Value; i++)
                    {
                        sReturn += i.ToString().Substring(i.ToString().Length - 1);
                    }
                }
                return sReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria form class that will contain the proper 
        ///     number of near months requested by the user
        /// </summary>
        public int NearMonths
        {
            get
            {
                int iReturn = 0;
                if (chkNears.Checked)
                {
                    iReturn = Convert.ToInt32(nudNears.Value);
                }
                return iReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria form class that will contain the proper value for 
        ///     Binary options based upon user input
        /// </summary>
        public string Binary
        {
            get
            {
                string sReturn = "F";
                if (chkBinary.Checked)
                {
                    sReturn = "T";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper filter type value
        ///     based upon user input
        /// </summary>
        public int FilterType
        {
            get
            {
                int iReturn = 0;
                if (rdbStrike.Checked)
                {
                    iReturn = 1;
                }
                else if (rdbMoney.Checked)
                {
                    iReturn = 2;
                }
                return iReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper number
        ///     for filter1 based upon user input
        /// </summary>
        public double Filter1
        {
            get
            {
                double dReturn = 0.0;
                if (rdbStrike.Checked)
                {
                    dReturn = Convert.ToDouble(txtFrom.Text);
                }
                else if (rdbMoney.Checked)
                {
                    dReturn = Convert.ToDouble(nudITM.Value);
                }
                return dReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper number
        ///     for filter2 based upon user input
        /// </summary>
        public double Filter2
        {
            get
            {
                double dReturn = 0.0;
                if (rdbStrike.Checked)
                {
                    dReturn = Convert.ToDouble(txtTo.Text);
                }
                else if (rdbMoney.Checked)
                {
                    dReturn = Convert.ToDouble(nudOOTM.Value);
                }
                return dReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper 
        ///     months code string based upon which request type is entered by the user
        /// </summary>
        public string MonthCodes
        {
            get
            {
                string sReturn = "";
                if (IsAnyIndividualMonthChecked())
                {
                    if (rdbEquityOptions.Checked)
                    {
                        if (chkPuts.Checked)
                        {
                            sReturn += GetEquityPutMonthCodes();
                        }
                        if (chkCalls.Checked)
                        {
                            sReturn += GetEquityCallMonthCodes();
                        }
                    }
                    else if (rdbFutures.Checked || rdbFutureOptions.Checked || rdbFutureSpreads.Checked)
                    {
                        sReturn += GetFutureMonthCodes();
                    }
                    else
                    {
                        sReturn = "Error";
                    }                
                }
                return sReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper
        ///     puts/calls string based upon user input
        /// </summary>
        public string PutsCalls
        {
            get
            {
                string sReturn = "";
                if (chkPuts.Checked)
                {
                    sReturn = "p";
                }
                if (chkCalls.Checked)
                {
                    sReturn += "c";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// Property of the Criteria for class that will contain the proper security 
        ///     type string based upon which request type is entered by the user
        /// </summary>
        public string SecurityType
        {
            get
            {
                string sReturn = "Error";
                if (rdbEquityOptions.Checked)
                {
                    sReturn = "ieoption";
                }
                else if (rdbFutures.Checked)
                {
                    sReturn = "future";
                }
                else if (rdbFutureOptions.Checked)
                {
                    sReturn = "foption";
                }
                else if (rdbFutureSpreads.Checked)
                {
                    sReturn = "fspread";
                }
                return sReturn;
            }
        }

        /// <summary>
        /// Function that returns true if any month is checked and false if no months are checked.
        /// </summary>
        /// <returns></returns>
        private bool IsAnyIndividualMonthChecked()
        {
            bool bReturn = false;
            if (chkJan.Checked || chkFeb.Checked || chkMar.Checked || chkApr.Checked
                || chkMay.Checked || chkJun.Checked || chkJul.Checked || chkAug.Checked
                || chkSep.Checked || chkOct.Checked || chkNov.Checked || chkDec.Checked)
            {
                bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// Function that returns the MonthCode string based upon which months are checked
        /// </summary>
        /// <returns></returns>
        private string GetFutureMonthCodes()
        {
            string sReturn = "";
            if (chkAll.Checked)
            {
                sReturn = "FGHJKMNQUVXZ";
            }
            else
            {
                if (chkJan.Checked)
                {
                    sReturn = "F";
                }
                if (chkFeb.Checked)
                {
                    sReturn += "G";
                }
                if (chkMar.Checked)
                {
                    sReturn += "H";
                }
                if (chkApr.Checked)
                {
                    sReturn += "J";
                }
                if (chkMay.Checked)
                {
                    sReturn += "K";
                }
                if (chkJun.Checked)
                {
                    sReturn += "M";
                }
                if (chkJul.Checked)
                {
                    sReturn += "N";
                }
                if (chkAug.Checked)
                {
                    sReturn += "Q";
                }
                if (chkSep.Checked)
                {
                    sReturn += "U";
                }
                if (chkOct.Checked)
                {
                    sReturn += "V";
                }
                if (chkNov.Checked)
                {
                    sReturn += "X";
                }
                if (chkDec.Checked)
                {
                    sReturn += "Z";
                }
            }
            return sReturn;
        }

        /// <summary>
        /// Function that returns the MonthCode string based upon which months are checked
        /// </summary>
        /// <returns></returns>
        private string GetEquityPutMonthCodes()
        {
            string sReturn = "";
            if (chkAll.Checked)
            {
                sReturn = "MNOPQRSTUVWX";
            }
            else
            {
                if (chkJan.Checked)
                {
                    sReturn = "M";
                }
                if (chkFeb.Checked)
                {
                    sReturn += "N";
                }
                if (chkMar.Checked)
                {
                    sReturn += "O";
                }
                if (chkApr.Checked)
                {
                    sReturn += "P";
                }
                if (chkMay.Checked)
                {
                    sReturn += "Q";
                }
                if (chkJun.Checked)
                {
                    sReturn += "R";
                }
                if (chkJul.Checked)
                {
                    sReturn += "S";
                }
                if (chkAug.Checked)
                {
                    sReturn += "T";
                }
                if (chkSep.Checked)
                {
                    sReturn += "U";
                }
                if (chkOct.Checked)
                {
                    sReturn += "V";
                }
                if (chkNov.Checked)
                {
                    sReturn += "W";
                }
                if (chkDec.Checked)
                {
                    sReturn += "X";
                }
            }
            return sReturn;
        }

        /// <summary>
        /// Function that returns the MonthCode string based upon which months are checked
        /// </summary>
        private string GetEquityCallMonthCodes()
        {
            string sReturn = "";
            if (chkAll.Checked)
            {
                sReturn = "ABCDEFGHIJKL";
            }
            else
            {
                if (chkJan.Checked)
                {
                    sReturn = "A";
                }
                if (chkFeb.Checked)
                {
                    sReturn += "B";
                }
                if (chkMar.Checked)
                {
                    sReturn += "C";
                }
                if (chkApr.Checked)
                {
                    sReturn += "D";
                }
                if (chkMay.Checked)
                {
                    sReturn += "E";
                }
                if (chkJun.Checked)
                {
                    sReturn += "F";
                }
                if (chkJul.Checked)
                {
                    sReturn += "G";
                }
                if (chkAug.Checked)
                {
                    sReturn += "H";
                }
                if (chkSep.Checked)
                {
                    sReturn += "I";
                }
                if (chkOct.Checked)
                {
                    sReturn += "J";
                }
                if (chkNov.Checked)
                {
                    sReturn += "K";
                }
                if (chkDec.Checked)
                {
                    sReturn += "L";
                }
            }
            return sReturn;
        }

        /// <summary>
        /// Event that fires when the .Checked value of the No filter radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbNoFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNoFilter.Checked)
            {
                txtFrom.Enabled = false;
                txtFrom.Text = "0";
                txtTo.Enabled = false;
                txtTo.Text = "0";
                nudITM.Enabled = false;
                nudITM.Value = 1;
                nudOOTM.Enabled = false;
                nudOOTM.Value = 1;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the Strike radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbStrike_CheckedChanged(object sender, EventArgs e)
        {
            txtFrom.Enabled = true;
            txtTo.Enabled = true;
            nudITM.Enabled = false;
            nudITM.Value = 1;
            nudOOTM.Enabled = false;
            nudOOTM.Value = 1;
        }

        /// <summary>
        /// Event that fires when the .Checked value of the In/Out of the Money radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbMoney_CheckedChanged(object sender, EventArgs e)
        {
            txtFrom.Enabled = false;
            txtFrom.Text = "0";
            txtTo.Enabled = false;
            txtTo.Text = "0";
            nudITM.Enabled = true;
            nudOOTM.Enabled = true;
        }

        /// <summary>
        /// Event that fires when the .Checked value of the nears checkbox changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNears_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNears.Checked)
            {
                chkAll.Checked = false;
                nudNears.Enabled = true;
                chkJan.Checked = false;
                chkJan.Enabled = false;
                chkFeb.Checked = false;
                chkFeb.Enabled = false;
                chkMar.Checked = false;
                chkMar.Enabled = false;
                chkApr.Checked = false;
                chkApr.Enabled = false;
                chkMay.Checked = false;
                chkMay.Enabled = false;
                chkJun.Checked = false;
                chkJun.Enabled = false;
                chkJul.Checked = false;
                chkJul.Enabled = false;
                chkAug.Checked = false;
                chkAug.Enabled = false;
                chkSep.Checked = false;
                chkSep.Enabled = false;
                chkOct.Checked = false;
                chkOct.Enabled = false;
                chkNov.Checked = false;
                chkNov.Enabled = false;
                chkDec.Checked = false;
                chkDec.Enabled = false;
            }
            else
            {
                nudNears.Enabled = false;
                chkJan.Enabled = true;
                chkFeb.Enabled = true;
                chkMar.Enabled = true;
                chkApr.Enabled = true;
                chkMay.Enabled = true;
                chkJun.Enabled = true;
                chkJul.Enabled = true;
                chkAug.Enabled = true;
                chkSep.Enabled = true;
                chkOct.Enabled = true;
                chkNov.Enabled = true;
                chkDec.Enabled = true;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the Equity Options radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbEquityOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEquityOptions.Checked)
            {
                chkPuts.Enabled = true;
                chkCalls.Enabled = true;
                chkPuts.Checked = true;
                chkCalls.Checked = true;
                chkBinary.Enabled = true;
                nudStartYear.Enabled = false;
                nudEndYear.Enabled = false;
                rdbNoFilter.Enabled = true;
                rdbNoFilter.Checked = true;
                rdbStrike.Enabled = true;
                rdbMoney.Enabled = true;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the Futures radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutures_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFutures.Checked)
            {
                nudStartYear.Enabled = true;
                nudEndYear.Enabled = true;
                chkCalls.Checked = false;
                chkCalls.Enabled = false;
                chkPuts.Checked = false;
                chkPuts.Enabled = false;
                chkBinary.Checked = false;
                chkBinary.Enabled = false;
                rdbNoFilter.Checked = true;
                rdbNoFilter.Enabled = false;
                rdbStrike.Enabled = false;
                rdbMoney.Enabled = false;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the Futures Options radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutureOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFutureOptions.Checked)
            {
                nudStartYear.Enabled = true;
                nudEndYear.Enabled = true;
                chkCalls.Enabled = true;
                chkPuts.Enabled = true;
                chkPuts.Checked = true;
                chkCalls.Checked = true;
                chkBinary.Checked = false;
                chkBinary.Enabled = false;
                rdbNoFilter.Checked = true;
                rdbNoFilter.Enabled = false;
                rdbStrike.Enabled = false;
                rdbMoney.Enabled = false;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the Future Spreads radio button changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutureSpreads_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFutureSpreads.Checked)
            {
                nudStartYear.Enabled = true;
                nudEndYear.Enabled = true;
                chkCalls.Checked = false;
                chkCalls.Enabled = false;
                chkPuts.Checked = false;
                chkPuts.Enabled = false;
                chkBinary.Checked = false;
                chkBinary.Enabled = false;
                rdbNoFilter.Checked = true;
                rdbNoFilter.Enabled = false;
                rdbStrike.Enabled = false;
                rdbMoney.Enabled = false;
            }
        }

        /// <summary>
        /// Event that fires when the .Checked value of the All Months checkbox changes.
        ///     updates controls based upon what other controls are dependant on this change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                chkNears.Checked = false;
                nudNears.Enabled = false;
                nudNears.Value = 1;
                chkJan.Checked = true;
                chkJan.Enabled = false;
                chkFeb.Checked = true;
                chkFeb.Enabled = false;
                chkMar.Checked = true;
                chkMar.Enabled = false;
                chkApr.Checked = true;
                chkApr.Enabled = false;
                chkMay.Checked = true;
                chkMay.Enabled = false;
                chkJun.Checked = true;
                chkJun.Enabled = false;
                chkJul.Checked = true;
                chkJul.Enabled = false;
                chkAug.Checked = true;
                chkAug.Enabled = false;
                chkSep.Checked = true;
                chkSep.Enabled = false;
                chkOct.Checked = true;
                chkOct.Enabled = false;
                chkNov.Checked = true;
                chkNov.Enabled = false;
                chkDec.Checked = true;
                chkDec.Enabled = false;
            }
            else
            {
                nudNears.Enabled = true;
                chkJan.Checked = false;
                chkJan.Enabled = true;
                chkFeb.Checked = false;
                chkFeb.Enabled = true;
                chkMar.Checked = false;
                chkMar.Enabled = true;
                chkApr.Checked = false;
                chkApr.Enabled = true;
                chkMay.Checked = false;
                chkMay.Enabled = true;
                chkJun.Checked = false;
                chkJun.Enabled = true;
                chkJul.Checked = false;
                chkJul.Enabled = true;
                chkAug.Checked = false;
                chkAug.Enabled = true;
                chkSep.Checked = false;
                chkSep.Enabled = true;
                chkOct.Checked = false;
                chkOct.Enabled = true;
                chkNov.Checked = false;
                chkNov.Enabled = true;
                chkDec.Checked = false;
                chkDec.Enabled = true;
            }
        }
    }
}