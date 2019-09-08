using System;
using System.Collections.Generic;
using System.Threading;
using IQFeed;
using Tools;
using static Tools.GMath;

namespace pyQFeed
{
    class Program
    {
        static PriceUpdateMap m_updates = new PriceUpdateMap();
        static PriceFeed m_prices;

        static HistoryFeed m_history;

        static void Main(string[] args)
        {
            m_prices = PriceFeed.Instance;
            m_prices.UpdatePrices += M_prices_UpdatePrices;

            /*m_history = HistoryFeed.Instance;
            m_history.GetDailyTimeframe("@VXH18", "20160101", "20181231", "20000", "1", "PYQFEED", "5000");
            return;*/

            //SubscribeNikkeiYen("H18");
            //SubscribeCopper("H18");
            SubscribeVIXES("H18");


            Console.WriteLine("(displaying price updates while thread sleeping...)");
            Thread.Sleep(60000);
        }

        static void SubscribeNikkeiYen(string mYY)
        {
            // Subscribe Nikkei/Yen
            m_updates.SetId("niy", "@NIY" + mYY);
            m_updates.SetId("nkd", "@NKD" + mYY);
            m_updates.SetId("jy", "@JY" + mYY);
            m_prices.SubscribePrices("@NIY" + mYY);
            m_prices.SubscribePrices("@NKD" + mYY);
            m_prices.SubscribePrices("@JY" + mYY);
        }

        static void SubscribeCopper(string mYY)
        {
            // Subscribe CME/LME Copper
            string m0 = mYY;
            string m1 = GDate.AddMonths(mYY, 1);
            m_updates.SetId("hg0", "QHG" + m0);
            m_updates.SetId("hg1", "QHG" + m1);
            m_updates.SetId("lme", "M.CU3=LX");
            m_prices.SubscribePrices("QHG" + m0);
            m_prices.SubscribePrices("QHG" + m1);
            m_prices.SubscribePrices("M.CU3=LX");
        }

        static void SubscribeVIXES(string mYY)
        {
            // Subscribe VIX/ES
            string m0 = mYY;
            string m1 = GDate.AddMonths(mYY, 1);
            string m2 = GDate.AddMonths(mYY, 2);
            string m3 = GDate.AddMonths(mYY, 3);
            string m4 = GDate.AddMonths(mYY, 4);
            string m5 = GDate.AddMonths(mYY, 5);
            m_updates.SetId("vix", "VIX.XO");
            m_prices.SubscribePrices("VIX.XO");
            m_updates.SetId("vx0", "@VX" + m0);
            m_updates.SetId("vx1", "@VX" + m1);
            m_updates.SetId("vx2", "@VX" + m2);
            m_updates.SetId("vx3", "@VX" + m3);
            m_updates.SetId("vx4", "@VX" + m4);
            m_updates.SetId("vx5", "@VX" + m5);
            m_prices.SubscribePrices("@VX" + m0);
            m_prices.SubscribePrices("@VX" + m1);
            m_prices.SubscribePrices("@VX" + m2);
            m_prices.SubscribePrices("@VX" + m3);
            m_prices.SubscribePrices("@VX" + m4);
            m_prices.SubscribePrices("@VX" + m5);
            string es_m0 = "H18";
            m_updates.SetId("spu", "@ES" + es_m0);
            m_prices.SubscribePrices("@ES" + es_m0);
        }

        static void M_prices_UpdatePrices(PriceUpdate update)
        {
            // Update messages are sent to the client anytime one of the fields in the current fieldset are updated.
            //var x = "Q,@ESU17,2463.50,2,08:26:54.256829,43,170391,2463.25,116,2463.50,159,2460.25,2463.75,2456.50,2459.75,a,01,";

            m_updates.Update(update);
            //System.Console.WriteLine(update.ToString());

            if (update.HasSymbolRoot("@NIY", "@NKD"))  //  update.Symbol.StartsWith("@NIY") || update.Symbol.StartsWith("@NKD"))
            {
                if (m_updates.HasUpdatesFor("niy", "nkd"))
                {
                    decimal bid = m_updates["niy"].Bid - m_updates["nkd"].Ask;
                    decimal ask = m_updates["niy"].Ask - m_updates["nkd"].Bid;
                    System.Console.WriteLine("b:{0} a:{1}     {2}:{3} {4}:{5}", bid, ask, m_updates["niy"].Bid, m_updates["niy"].Ask, m_updates["nkd"].Bid, m_updates["nkd"].Ask);
                }
            }

            if (update.HasSymbolRoot("QHG", "M.CU3=LX"))
            {
                if (m_updates.HasUpdatesFor("hg0", "hg1", "lme"))
                {
                    decimal hg0 = m_updates["hg0"].Mid;
                    decimal hg1 = m_updates["hg1"].Mid;
                    decimal lme = m_updates["lme"].Mid;

                    decimal spread = Math.Round((hg0 - (0.000454M * lme)) * 100, 2);
                    System.Console.WriteLine("{0} {1}   {2}   spread:{3}", hg0, hg1, lme, spread);
                }
            }

            if (update.HasSymbolRoot("@VX"))
            {
                if (m_updates.HasUpdatesFor("vx0", "vx1", "vx2", "vx3", "vx4", "vx5"))
                {
                    decimal vx0 = m_updates["vx0"].Mid;
                    decimal vx1 = m_updates["vx1"].Mid;
                    decimal vx2 = m_updates["vx2"].Mid;
                    decimal vx3 = m_updates["vx3"].Mid;
                    decimal vx4 = m_updates["vx4"].Mid;
                    decimal vx5 = m_updates["vx5"].Mid;

                    decimal contango1 = Contango(vx0, vx1);
                    decimal contango2 = Contango(vx1, vx2);
                    decimal contango3 = Contango(vx2, vx3);
                    decimal contango4 = Contango(vx3, vx4);
                    decimal contango5 = Contango(vx4, vx5);
                    System.Console.WriteLine("{0}% {1}% {2}% {3}% {4}%   vx0:{5} vx1:{6} vx2:{7} vx3:{8} vx4:{9} vx5:{10}", contango1, contango2, contango3, contango4, contango5, vx0, vx1, vx2, vx3, vx4, vx5);
                }
                else if (m_updates.HasUpdatesFor("vx0", "vx1", "vx2"))
                {
                    decimal vx0 = m_updates["vx0"].Mid;
                    decimal vx1 = m_updates["vx1"].Mid;
                    decimal vx2 = m_updates["vx2"].Mid;

                    decimal contango1 = Contango(vx0, vx1);
                    decimal contango2 = Contango(vx1, vx2);
                    System.Console.WriteLine("{0}% {1}%   vx0:{2} vx1:{3} vx2:{4}", contango1, contango2, vx0, vx1, vx2);
                }
            }

        }
    } // end of class
} // end of namespace
