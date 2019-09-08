using System;
using XCT.BaseLib.API.Bithumb.Public;
using XCT.BaseLib.API.Bithumb.Trade;
using XCT.BaseLib.API.Bithumb.User;

namespace Bithumb.Sample.Core
{
    public partial class Bithumb
    {
        /// <summary>
        /// 1. Public API
        /// </summary>
        public static async void XPublicApi(int debug_step = 1)
        {
            var _public_api = new BPublicApi();

            if (debug_step == 1)
            {
                var _ticker = await _public_api.Ticker("ETH");
                if (_ticker.status == 0)
                    Console.WriteLine(_ticker.data.closing_price);
            }

            if (debug_step == 2)
            {
                var _orderbook = await _public_api.OrderBook("ETH", 1, 50);
                if (_orderbook.status == 0)
                    Console.WriteLine(_orderbook.data.timestamp);
            }

            if (debug_step == 3)
            {
                var _recent_transactions = await _public_api.RecentTransactions("ETH", 0, 100);
                if (_recent_transactions.status == 0)
                    Console.WriteLine(_recent_transactions.status);
            }
        }

        /// <summary>
        /// 2. User API
        /// </summary>
        public static async void XUserApi(int debug_step = 7)
        {
            var __info_api = new BUserApi("", "");

            if (debug_step == 1)
            {
                var _account = await __info_api.Account("ETH");
                if (_account.status == 0)
                    Console.WriteLine(_account.data.account_id);
            }

            if (debug_step == 2)
            {
                var _balance = await __info_api.Balance("ETH");
                if (_balance.status == 0)
                    Console.WriteLine(_balance.data.available_btc);
            }

            if (debug_step == 3)
            {
                var _wallet_address = await __info_api.WalletAddress("ETH");
                if (_wallet_address.status == 0)
                    Console.WriteLine(_wallet_address.data.wallet_address);
            }

            if (debug_step == 4)
            {
                var _ticker = await __info_api.Ticker(order_currency: "ETH");
                if (_ticker.status == 0)
                    Console.WriteLine(_ticker.data.units_traded);
            }

            if (debug_step == 5)
            {
                var _orders = await __info_api.Orders("ETH");
                if (_orders.status == 0)
                    Console.WriteLine(_orders.data.Count);
            }

            if (debug_step == 6)
            {
                var _order_detail = await __info_api.OrderDetail("ETH", "order_id", "ask");
                if (_order_detail.status == 0)
                    Console.WriteLine(_order_detail.data.Count);
            }

            if (debug_step == 7)
            {
                var _user_transactions = await __info_api.UserTransactions("ETH");
                if (_user_transactions.status == 0)
                    Console.WriteLine(_user_transactions.data.Count);
            }
        }

        /// <summary>
        /// 3. Trade API
        /// </summary>
        public static async void XTradeApi(int debug_step = 1)
        {
            var __trade_api = new BTradeApi("", "");

            if (debug_step == 1)
            {
                var _place = await __trade_api.Place(0.1m, 60000, "ask", "ETH");
                if (_place.status == 0)
                    Console.WriteLine(_place.data.Count);
            }

            if (debug_step == 2)
            {
                var _cancel = await __trade_api.Cancel("ETH", "order_id", "bid");
                if (_cancel.status == 0)
                    Console.WriteLine(_cancel.status);
            }

            if (debug_step == 3)
            {
                var _btc_withdrawal = await __trade_api.BtcWithdrawal("ETH", 0.1m, "address");
                if (_btc_withdrawal.status == 0)
                    Console.WriteLine(_btc_withdrawal.status);
            }

            if (debug_step == 4)
            {
                var _krw_deposit = await __trade_api.KrwDeposit();
                if (_krw_deposit.status == 0)
                    Console.WriteLine(_krw_deposit.status);
            }

            if (debug_step == 5)
            {
                var _krw_withdrawal = await __trade_api.KrwWithdrawal("003_기업은행", "111-2222-33333", 10000m);
                if (_krw_withdrawal.status == 0)
                    Console.WriteLine(_krw_withdrawal.status);
            }

            if (debug_step == 6)
            {
                var _market_buy = await __trade_api.MarketBuy("ETH", 0.1m);
                if (_market_buy.status == 0)
                    Console.WriteLine(_market_buy.order_id);
            }

            if (debug_step == 7)
            {
                var _market_sell = await __trade_api.MarketSell("ETH", 0.1m);
                if (_market_sell.status == 0)
                    Console.WriteLine(_market_sell.order_id);
            }
        }

        public static void Start(int debug_step = 2)
        {
            // 1. Public API
            if (debug_step == 1)
                XPublicApi();

            // 2. Private API
            if (debug_step == 2)
                XUserApi();

            // 3. Trade API
            if (debug_step == 3)
                XTradeApi();
        }
    }
}