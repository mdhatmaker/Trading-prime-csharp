using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Diagnostics;
using static Tools.G;
using static Tools.GDate;
using ZS = ZeroSumAPI;
using T4;
using T4.API;

namespace ZeroSumAPI
{
    // Define the T4 instrument details
    public struct T4Instrument
    {
        public Exchange Exchange { get; private set; }
        public Contract Contract { get; private set; }
        public Market Market { get; private set; }

        private string m_exchange;
        private string m_contract;
        private string m_market;

        public T4Instrument(T4Product product, string market)
        {
            m_exchange = product.ExchangeStr;
            m_contract = product.ContractStr;
            m_market = market;
            this.Exchange = null;
            this.Contract = null;
            this.Market = null;
        }

        public T4Instrument(Exchange exchange, Contract contract, Market market)
        {
            this.Exchange = exchange;
            this.Contract = contract;
            this.Market = market;
            m_exchange = exchange.Description;
            m_contract = contract.Description;
            m_market = market.Description;
        }
    } // end of STRUCT

    public struct T4Product
    {
        public string ExchangeStr { get; private set; }
        public string ContractStr { get; private set; }

        public T4Product(string exchange, string contract)
        {
            this.ExchangeStr = exchange;
            this.ContractStr = contract;
        }
    } // end of STRUCT


    public class TengineT4 : TradingEngine
    {
        // ****** ADD NEW PRODUCTS HERE!!! ******
        // Translate IQFeed symbol into T4 contract
        static Dictionary<string, T4Product> T4ProductTranslate = new Dictionary<string, T4Product>()
        {
            { "@VX", new T4Product("SIM:Volatility Index", "A") },
            { "@ES", new T4Product("D-30m:CME Equity Futures", "D-30m:E-mini S&P 500") },
        };
        // ****** OR HERE IF NOT FUTURES!!! ******
        // Translate IQFeed symbol into TTInstrument struct
        static Dictionary<string, T4Instrument> T4InstrumentTranslate = new Dictionary<string, T4Instrument>()
        {
            { "M.CU3=LX", new T4Instrument(new T4Product("LME", "CA"), "3M") }
        };

        private Dictionary<uint, T4Instrument> _instruments = new Dictionary<uint, T4Instrument>();

        #region Declare the T4API objects (and delegates)
        private delegate void OnAccountDetailsDelegate(T4.API.AccountList.UpdateList poAccounts);
        private delegate void OnAccountUpdateDelegate(T4.API.AccountList.UpdateList poAccounts);
        private delegate void OnPositionUpdateDelegate(T4.API.Position poPosition);
        private delegate void OnMarketDepthUpdateDelegate(Market poMarket);
        private delegate void OnAccountOrderUpdateDelegate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders);
        private delegate void OnAccountOrderAddedDelegate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders);
        private delegate void OnAccountListCompleteDelegate(T4.API.AccountList poAccounts);

        internal Host moHost;                           // Reference to the main api host object.
        internal Account moAccount;                     // Reference to the current account.
        internal ExchangeList moExchanges;              // Reference to the exchange list.
        internal Exchange moExchange;                   // Reference to the current exchange.
        internal ContractList moContracts;              // Reference to an exchange's contract list.
        internal Contract moContract;                   // Reference to the current contract.
        internal MarketList moPickerMarkets;            // Reference to a contract's market list.
        internal Market moPickerMarket;                 // Reference to the current market.
        internal MarketList moMarkets1Filter;           // Market1 filter.
        internal MarketList moMarkets2Filter;           // Market2 filter.
        internal Market moMarket1;                      // References to selected markets.
        internal Market moMarket2;                      // References to selected markets.
        internal string mstrMarketID1;                  // References to marketid's retrieved from saved settings.
        internal string mstrMarketID2;                  // References to marketid's retrieved from saved settings.
        internal AccountList moAccounts;                // Reference to the accounts list.
        internal ArrayList moOrderArrayList = new ArrayList();  // Reference to Order arraylist. Stores the collection of orders.
        #endregion
        private List<Exchange> m_exchanges = new List<Exchange>();
        private List<Contract> m_contracts = new List<Contract>();
        private List<Market> m_markets = new List<Market>();
        private List<Account> m_accounts = new List<Account>();
        private Exchange SelectedExchange { get; set; }
        private Contract SelectedContract { get; set; }
        private Market SelectedMarket { get; set; }
        private Account SelectedAccount { get; set; }

        public TengineT4()
        {
            // Add translations from IQFeed symbols to definition fields of a TT Instrument
            //_translateSymbol.Add("@ESZ17", new TTInstrument("CME", ProductType.Future, "ES", "Dec17"));
        }

        #region TradingEngine abstract overrides
        public override void Startup()
        {
            StartupAPI();
        }

        public override void Shutdown()
        {
            ShutdownAPI();
        }

        // Expect 'symbol' as IQFeed Symbol ("@ESJ16", "@VXK16", "@VXQ17")
        public override uint CreateInstrument(uint iid, string symbol)
        {
            instruments[iid] = new ZInstrument(iid, symbol);
            _instruments.Add(iid, TranslateInstrument(symbol));
            return iid;
        }

        public override void Subscribe(uint iid)
        {
            SubscribeInstrument(_instruments[iid]);
        }

        public override void Unsubscribe(uint iid)
        {
            UnsubscribeInstrument(_instruments[iid]);
        }

        public override uint CreateOrder(uint iid, ZOrderSide side, int price, uint qty, ZOrderType type, ZOrderTimeInForce tif)
        {
            uint oid = GenerateOrderId();
            var o = new ZOrder(oid, iid, side, price, qty, type, tif);
            orders[oid] = o;

            //_orders[o.Oid] = new OME.EquityOrder(_instruments[o.Iid], OME.Order.OrderTypes.GoodUntilCancelled, GetSide(o), o.Price, o.Qty);

            return oid;
        }

        public override void SubmitOrder(uint oid)
        {
            ZOrder o = orders[oid];
            //var order = _orders[oid];
            //GetBook(o).Insert(order);
            var t4instr = _instruments[o.Iid];
            SendOrder(t4instr, o);
            o.SetState(ZOrderState.Working);
        }

        public override void DeleteOrder(uint oid)
        {
            ZOrder o = orders[oid];
            //var order = _orders[oid];
            //GetBook(o).Remove(order);
            o.SetState(ZOrderState.Cancelled);
        }

        public override void DeleteAllOrders()
        {
            throw new NotImplementedException();
        }

        public override void ModifyOrder(uint oid, int price, uint qty)
        {
            orders[oid].SetPrice(price);
            orders[oid].SetQty(qty);
        }

        public override void ModifyOrder(uint oid, int price)
        {
            orders[oid].SetPrice(price);
        }
        public override void ModifyOrder(uint oid, uint qty)
        {
            orders[oid].SetQty(qty);
        }

        public override ZOrder GetOrder(uint oid)
        {
            return orders[oid];
        }
        #endregion

        #region Helper Functions
        // Given an IQFeed Future Symbol (that ends in 'mYY' monthcode and 2-digit year)
        // Return (as out parameters) both the corresponding T4 Symbol and the TT Month/Year (both as strings)
        // Return True if both Symbol and Month/Year were translated successfully; otherwise return False
        public static T4Instrument TranslateInstrument(string iqSymbol)
        {
            string mYY = GetmYY(iqSymbol);
            string symbol = iqSymbol;
            // symbol will either be the "root" (if it's a futures contract) OR the whole iqSymbol (if not)
            if (mYY != null)
            {
                symbol = iqSymbol.Substring(0, iqSymbol.Length - 3);
                T4Product prod = T4ProductTranslate[symbol];
                string ttMonthYear = MonthYear(mYY);
                return new T4Instrument(prod, ttMonthYear);
            }
            else
                return T4InstrumentTranslate[symbol];
        }
        #endregion


        #region Startup and shutdown code
        // Initialise the api when the application starts.
        private void StartupAPI()
        {
            moHost = Host.Login(APIServerType.Simulator, "T4Example", "112A04B0-5AAF-42F4-994E-FA7CB959C60B");
            
            if (moHost == null)                     // Check for success.
            {
                //this.Close();                       // Host object not returned which means the user cancelled the login dialog.
            }
            else
            {
                // Login was successfull.
                Trace.WriteLine("Login Success");
                Init();                             // Initialize.
            }
        }

        // Shutdown the api when the application exits.
        private void ShutdownAPI()
        {
            if (moHost != null)                     // Check to see that we have an api object.
            {
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
                if (moMarkets1Filter != null) moMarkets1Filter.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets1Filter_MarketListComplete);
                if (moMarkets2Filter != null) moMarkets2Filter.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moMarkets2Filter_MarketListComplete);

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
                //if (moExchanges != null) moExchanges.ExchangeListComplete -= new T4.API.ExchangeList.ExchangeListCompleteEventHandler(moExchanges_ExchangeListComplete);

                // Contract list events.
                if (moContracts != null) moContracts.ContractListComplete -= new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);

                // Market list events.
                if (moPickerMarkets != null) moPickerMarkets.MarketListComplete -= new T4.API.MarketList.MarketListCompleteEventHandler(moPickerMarkets_MarketListComplete);

                // Market events.
                if (moMarket1 != null) moMarket1.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                if (moMarket2 != null) moMarket2.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);

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

        #region " Initialization "
        // Initialize the application.
        private void Init()
        {

            Trace.WriteLine("Init");

            // Populate the available exchanges.
            moExchanges = moHost.MarketData.Exchanges;

            // Register the exchangelist events.
            //moExchanges.ExchangeListComplete += new T4.API.ExchangeList.ExchangeListCompleteEventHandler(moExchanges_ExchangeListComplete);

            // Check to see if the data is already loaded.
            /*if (moExchanges.Complete)
            {
                // Call the event handler ourselves as the data is 
                // already loaded.
                moExchanges_ExchangeListComplete(moExchanges);

            }*/

            // Set the account list reference so that we can get 
            // Account and order events.
            moAccounts = moHost.Accounts;

            // Register the accountlist events.
            moAccounts.AccountDetails += new T4.API.AccountList.AccountDetailsEventHandler(moAccounts_AccountDetails);
            moAccounts.PositionUpdate += new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
            moAccounts.AccountUpdate += new T4.API.AccountList.AccountUpdateEventHandler(moAccounts_AccountUpdate);
            moAccounts.PositionUpdate += new T4.API.AccountList.PositionUpdateEventHandler(moAccounts_PositionUpdate);
            //moAccounts.AccountListComplete += new T4.API.AccountList.AccountListCompleteEventHandler(moAccounts_AccountListComplete);

            //if (moAccounts.Complete) moAccounts_AccountListComplete(moAccounts);

            try
            {
                XmlDocument oDoc;               // Read saved markets. XML Doc.
                XmlNode oMarkets;               // XML Nodes for viewing the doc.

                // Temporary string variables for referencing contract and exchange details.
                string strContractID;
                string strExchangeID;
                
                oDoc = moHost.UserSettings;     // Pull the xml doc from the server.
                oMarkets = oDoc.ChildNodes[0];  // Reference the saved markets via xml node.

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
            Markets1ListComplete();
            /*// Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(Markets1ListComplete));
            else
                Markets1ListComplete();*/
        }

        private void Markets1ListComplete()
        {
            Market oMarket1 = moMarkets1Filter[mstrMarketID1];          // Reference the desired market.
            NewMarketSubscription(ref moMarket1, ref oMarket1);         // Subscribe to market1.
        }

        private void moMarkets2Filter_MarketListComplete(T4.API.MarketList poMarketList)
        {
            Markets2ListComplete();
            /*// Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(Markets2ListComplete));
            else
                Markets2ListComplete();*/
        }

        private void Markets2ListComplete()
        {
            Market oMarket2 = moMarkets2Filter[mstrMarketID2];          // Reference the desired market.
            NewMarketSubscription(ref moMarket2, ref moPickerMarket);   // Subscribe to market2.
        }
        #endregion

        #region Market Picker (Exchanges, Contracts, Markets)
        private void moExchanges_ExchangeListComplete(T4.API.ExchangeList poExchangeList)
        {
            ExchangeListComplete();
            /*// Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ExchangeListComplete));
            }
            else
            {
                ExchangeListComplete();
            }*/
        }

        private void ExchangeListComplete()
        {
            // First clear all the Lists.
            m_exchanges.Clear();
            m_contracts.Clear();
            m_markets.Clear();

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
                        //  cboExchangesItems.Add(New ExchangeItem(oExchange))
                        m_exchanges.Add(oExchange);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage("T4API Error " + ex.ToString());
                }
                finally
                {
                    moHost.ExitLock("ExchangeList");        // This is guarenteed to execute last.
                }
            }
        }

        private void cboExchanges_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            // Populate the current exchange's available contracts.
            if (SelectedExchange != null)
            {
                moExchange = ((Exchange)(SelectedExchange));        // Reference the current exchange.

                // Unregister previous events.
                if (moContracts != null)
                {
                    moContracts.ContractListComplete -= new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);
                }

                moContracts = moExchange.Contracts;                     // Reference the exchange's available contracts.

                // Register the events.
                if (moContracts != null)
                {
                    moContracts.ContractListComplete += new T4.API.ContractList.ContractListCompleteEventHandler(moContracts_ContractListComplete);
                }

                // Check to see if the data is already loaded.
                if (moContracts.Complete)
                {
                    moContracts_ContractListComplete(moContracts);      // Call the event handler ourselves as the data is already loaded.
                }
            }
        }

        private void moContracts_ContractListComplete(T4.API.ContractList poContractList)
        {
            ContractListComplete();
            /*// Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ContractListComplete));
            }
            else
            {
                ContractListComplete();
            }*/
        }

        private void ContractListComplete()
        {
            // Populate the list of contracts available for the current exchange.

            // First clear all the combo's.
            m_contracts.Clear();
            m_markets.Clear();

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
                        m_contracts.Add(oContract);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage("T4API Error " + ex.ToString());
                }
                finally
                {
                    moHost.ExitLock("ContractList");                // This is guarenteed to execute last.
                }
            }
        }

        private void cboContracts_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            // Populate the current contract's available markets.
            {
                if ((SelectedContract != null))
                {
                    // Reference the current contract.
                    moContract = (Contract)SelectedContract;

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
                        moPickerMarkets_MarketListComplete(moPickerMarkets);    // Call the event handler ourselves as the data is already loaded.
                    }
                }
            }
        }

        private void moPickerMarkets_MarketListComplete(T4.API.MarketList poMarketList)
        {
            MarketListComplete();
            /*// Invoke the update.
            // This places process on GUI thread.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(MarketListComplete));
            }
            else
            {
                MarketListComplete();
            }*/
        }

        private void MarketListComplete()
        {
            // Populate the list of markets available for the current contract.
            
            m_markets.Clear();          // First clear the market List.
            moPickerMarket = null;      // Eliminate any previous references.

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
                        m_markets.Add(oMarket);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage("T4API Error " + ex.ToString());
                }
                finally
                {
                    moHost.ExitLock("MarketList");      // This is guarenteed to execute last.
                }
            }
        }

        private void cboMarkets_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            if (SelectedMarket != null)
            {
                moPickerMarket = ((Market)(SelectedMarket));            // Store a reference to the current market.
            }
        }
        #endregion

        #region Account Data
        // Event that is raised when details for an account have 
        // changed, or a new account is recieved.
        private void moAccounts_AccountDetails(T4.API.AccountList.UpdateList poAccounts)
        {
            OnAccountDetails(poAccounts);
            /*//  Invoke the update.
            //  This places process on GUI thread.
            //  Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountDetailsDelegate(OnAccountDetails), new Object[] { poAccounts });
            }
            else
            {
                OnAccountDetails(poAccounts);
            }*/
        }

        private void OnAccountDetails(AccountList.UpdateList poAccounts)
        {
            // Display the account list.
            foreach (Account oAccount in poAccounts)
            {
                // Check to see if the account exists prior to adding/subscribing to it.
                if (oAccount.Subscribed != true)
                {
                    m_accounts.Add(oAccount);                       // Add the account to the list.                    
                    oAccount.Subscribe();                           // Subscribe to the account.
                }
            }
        }

        // Event that is raised when the accounts overall balance,
        // P&L or margin details have changed.
        private void moAccounts_AccountUpdate(T4.API.AccountList.UpdateList poAccounts)
        {
            OnAccountUpdate(poAccounts);
            /*// Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountUpdateDelegate(OnAccountUpdate), new Object[] { poAccounts });
            }
            else
            {
                OnAccountUpdate(poAccounts);
            }*/
        }

        private void OnAccountUpdate(T4.API.AccountList.UpdateList poAccounts)
        {
            DisplayAccount(moAccount);                              // Just refresh the current account.
        }

        // Event that is raised when the account list is loaded.
        private void moAccounts_AccountListComplete(T4.API.AccountList poAccounts)
        {
            OnAccountListComplete(poAccounts);
            /*// Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {
                BeginInvoke(new OnAccountListCompleteDelegate(OnAccountListComplete), new Object[] { poAccounts });
            }
            else
            {
                OnAccountListComplete(poAccounts);
            }*/
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
                    m_accounts.Add(oAccount);                       // Add the account to the account List.
                    oAccount.Subscribe();                           // Subscribe to the account.
                }

                if (m_accounts.Count > 0)
                {
                    SelectedAccount = m_accounts[0];
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
                    // If the position is for the current account then update the value.
                    if (object.ReferenceEquals(oUpdate.Account, moAccount))
                    {
                        OnPositionUpdate(oUpdate.Position);
                        /*// Invoke the update.
                        // This places process on GUI thread.
                        // Must use a delegate to pass arguments.
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new OnPositionUpdateDelegate(OnPositionUpdate), new object[] { oUpdate.Position });
                        }
                        else
                        {
                            OnPositionUpdate(oUpdate.Position);
                        }*/
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }

        private void OnPositionUpdate(T4.API.Position poPosition)
        {
            if (object.ReferenceEquals(poPosition.Market, moMarket1))
            {                
                DisplayPosition(poPosition.Market, 1);                          // Display the position details.

            }
            else if (object.ReferenceEquals(poPosition.Market, moMarket2))
            {                
                DisplayPosition(poPosition.Market, 2);                          // Display the position details.
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
                    //txtCash.Text = String.Format("{0:#,###,##0.00}", moAccount.AvailableCash);
                    cout("{0:#,###,##0.00}", moAccount.AvailableCash);
                }
                catch (Exception ex)
                {
                    ErrorMessage("Error: " + ex.ToString());
                }
                finally
                {
                    moHost.ExitLock("DisplayAccount");                          // Unlock the host object.
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
                    moHost.EnterLock("DisplayPositions");       // Lock the host while we retrive details.
                    blnLocked = true;                           // Update the locked flag.
                    
                    Position oPosition = default(Position);     // Temporary position object used for referencing the account's positions.

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

                    cout("net:{0} buys:{1} sells:{2}", strNet, strBuys, strSells);
                    /*switch (piID)
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
                    }*/
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("T4API Error " + ex.ToString());
            }
            finally
            {
                if (blnLocked) moHost.ExitLock("DisplayPositions");     // Unlock the host object.
            }
        }

        private void cboAccounts_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            if ((SelectedAccount != null))
            {
                moAccount = (Account)SelectedAccount;                   // Reference the current account.

                // Register the account's events.
                if (moAccount != null)
                {
                    moAccount.OrderAdded += new T4.API.Account.OrderAddedEventHandler(moAccount_OrderAdded);
                    moAccount.OrderUpdate += new T4.API.Account.OrderUpdateEventHandler(moAccount_OrderUpdate);
                }
                
                DisplayAccount(moAccount);                              // Display the current account balance.

                // Refresh positions.
                DisplayPosition(moMarket1, 1);
                DisplayPosition(moMarket2, 2);
            }
        }
        #endregion

        #region Market Subscription 
        private void cmdGet1_Click(System.Object sender, System.EventArgs e)
        {
            DisplayMarketDetails(null, 1);                              // Clear the values.
            NewMarketSubscription(ref moMarket1, ref moPickerMarket);   // Subscribe to market1.
            DisplayPosition(moMarket1, 1);                              // Refresh the positions.
        }

        private void cmdGet2_Click(System.Object sender, System.EventArgs e)
        {
            Market oMarket = moHost.MarketData.MarketPicker(ref moMarket2);
            DisplayMarketDetails(null, 2);                              // Clear the values.
            NewMarketSubscription(ref moMarket2, ref oMarket);          // Subscribe to market2.
            DisplayPosition(moMarket2, 2);                              // Refresh the positions.
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
            OnMarketDepthUpdate(poMarket);
            /*// Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new OnMarketDepthUpdateDelegate(OnMarketDepthUpdate), new object[] { poMarket });
            }
            else
            {
                OnMarketDepthUpdate(poMarket);
            }*/
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
                ErrorMessage("T4API Error " + ex.ToString());
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
                    ErrorMessage("T4API Error " + ex.ToString());
                }
                finally
                {
                    moHost.ExitLock("DisplayMarketDetails");            // Unlock the host object.
                }
            }

            cout("{0} {1} {2} {3} {4} {5} {6} {7}", strDescription, strBid, strBidVol, strOffer, strOfferVol, strLast, strLastVol, strLastVolTotal);
            /*switch (piID)
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
            }*/
        }
        #endregion

        #region Single Order
        // Method that submits a single order.
        private void SubmitSingleOrder(Market poMarket, BuySell peBuySell, Double pdblLimitPrice)
        {
            if (moAccount != null && poMarket != null)
            {
                // Submit an order.
                T4.API.Order oOrder = moAccounts.SubmitNewOrder(
                    moAccount,
                    poMarket,
                    peBuySell,
                    PriceType.Limit,
                    TimeType.Normal,
                    1,
                    pdblLimitPrice);

                AddOrder(oOrder);                       // Add the order to the arraylist.
                DisplayOrders();                        // Display the orders.
            }
        }

        // Pull the single order that was submitted.
        private void PullSingleOrder(T4.API.Order poOrder)
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

        #region  Order Data
        private void moAccount_OrderUpdate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            OnAccountOrderUpdate(poAccount, poPosition, poOrders);
            /*// Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
            {

                this.BeginInvoke(new OnAccountOrderUpdateDelegate(OnAccountOrderUpdate), new Object[] { poAccount, poPosition, poOrders });
            }
            else
            {
                OnAccountOrderUpdate(poAccount, poPosition, poOrders);
            }*/
        }

        private void moAccount_OrderAdded(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            OnAccountOrderAdded(poAccount, poPosition, poOrders);
            /*// Invoke the update.
            // This places process on GUI thread.
            // Must use a delegate to pass arguments.
            if (this.InvokeRequired)
                this.BeginInvoke(new OnAccountOrderAddedDelegate(OnAccountOrderAdded), new Object[] { poAccount, poPosition, poOrders });
            else
                OnAccountOrderAdded(poAccount, poPosition, poOrders);*/
        }

        private void OnAccountOrderUpdate(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {            
            DisplayOrders();                                            // Redraw the order list.
        }

        private void OnAccountOrderAdded(T4.API.Account poAccount, T4.API.Position poPosition, T4.API.OrderList.UpdateList poOrders)
        {
            // Add all the orders to the arraylist.
            foreach (T4.API.Order oOrder in poOrders)
            {                
                AddOrder(oOrder);                                       // Add the order.
            }            
            DisplayOrders();                                            // Redraw the order list.
        }

        private void AddOrder(T4.API.Order poOrder)
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
                moHost.EnterLock();                                     // Lock the api.

                //lstOrders.SuspendLayout();                              // Suspend the layout of the listbox.
                //lstOrders.Items.Clear();                                // Clear and repopulate the list.

                T4.API.Order o;                                    // Temporary order object.

                // Repopulate the list by iterating through the collection backwards.
                for (int i = moOrderArrayList.Count - 1; i >= 0; i--)
                {                    
                    o = (T4.API.Order)(moOrderArrayList[i]);       // Reference an order.

                    cout("{0} {1} {2}/{3} @ {4}", o.Market.Description, o.BuySell.ToString(), o.TotalFillVolume, o.CurrentVolume, o.Market.ConvertTicksDisplay(o.CurrentLimitTicks, false), o.Status.ToString(), o.StatusDetail, o.SubmitTime);
                    /*// Display some order details.
                    lstOrders.Items.Add(oOrder.Market.Description + "   " +
                        oOrder.BuySell.ToString() + "   " +
                        oOrder.TotalFillVolume + "/" + oOrder.CurrentVolume + " @ " +
                        oOrder.Market.ConvertTicksDisplay(oOrder.CurrentLimitTicks, false) + "   " +
                        oOrder.Status.ToString() + "   " +
                        oOrder.StatusDetail + "  " +
                        oOrder.SubmitTime);*/
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("T4API Error: " + ex.ToString());
            }
            finally
            {                
                moHost.ExitLock();                                      // Unlock the api.
                //lstOrders.ResumeLayout();                               // Resume layout of the listbox.
            }
        }
        #endregion

        #region Misc Examples
        const string AUTOOCO = "Submit Auto OCO";
        const string FIVETICKSOFF = "Work 5 Ticks Off Market";

        // Setup misc example combos.
        private void SetupMiscExamples()
        {
            /*// Add examples to combos.
            cboMisc1.Items.Add(AUTOOCO);
            cboMisc1.Items.Add(FIVETICKSOFF);

            cboMisc2.Items.Add(AUTOOCO);
            cboMisc2.Items.Add(FIVETICKSOFF);

            // Be sure the first items are selected.
            cboMisc1.SelectedIndex = 0;
            cboMisc2.SelectedIndex = 0;*/
        }

        private void cmdRunMisc1_Click(Object sender, System.EventArgs e)
        {
            //string text1Str = cboMisc1.Text;
            //string bid1Str = txtBid1.Text;
            string text1Str = "FIVETICKSOFF";
            string bid1Str = "5";
            if (moMarket1 != null)
            {
                switch (text1Str)
                {
                    case AUTOOCO:
                        {
                            // Run autooco sample code.
                            SubmitAOCO(moMarket1, BuySell.Buy, bid1Str);
                            break;
                        }
                    case FIVETICKSOFF:
                        {
                            // Run the five ticks off code.
                            SubmitFiveTicksOff(moMarket1, BuySell.Buy, bid1Str);
                            break;
                        }
                }
            }
        }

        private void cmdRunMisc2_Click(Object sender, System.EventArgs e)
        {
            //string text2Str = cboMisc2.Text;
            //string offer2Str = txtOffer2.Text;
            string text2Str = "FIVETICKSOFF";
            string offer2Str = "5";
            if (moMarket2 != null)
            {
                switch (text2Str)
                {
                    case AUTOOCO:
                        {
                            // Run autooco sample code.
                            SubmitAOCO(moMarket2, BuySell.Sell, offer2Str);
                            break;
                        }
                    case FIVETICKSOFF:
                        {
                            // Run the five ticks off code.
                            SubmitFiveTicksOff(moMarket2, BuySell.Sell, offer2Str);
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
                T4.API.Order oOrder1 = oBatch.Add(peBuySell,
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
                    T4.API.Order oOrder2 = oBatch.Add(BuySell.Sell,
                        PriceType.Limit,
                        TimeType.Normal,
                        0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(5, 0, false), false));

                    // Add an order to the batch.
                    // This is the stop of the oco below the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    T4.API.Order oOrder3 = oBatch.Add(BuySell.Sell,
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
                    T4.API.Order oOrder2 = oBatch.Add(BuySell.Buy,
                        PriceType.Limit,
                        TimeType.Normal,
                        0,
                        poMarket.ConvertTicks(poMarket.TicksAdd(-5, 0, false), false));

                    // Add an order to the batch.
                    // This is the buy stop of the oco above the market.
                    // Note the flip of Buy/Sell.
                    // Note the ticks is a distance not a price representation.
                    T4.API.Order oOrder3 = oBatch.Add(BuySell.Buy,
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
            
            SubmitSingleOrder(poMarket, peBuySell, iNewPrice);              // Submit a single order five ticks off the market.
        }
        #endregion
        #endregion

        private void SubscribeInstrument(T4Instrument t4instr)
        {
            cout("Subsribing to Instrument...");

            // Update the market reference.
            //poMarket = poNewMarket;

            var poMarket = t4instr.Market;
            if ((poMarket != null))
            {
                // Register the events for this market.
                poMarket.MarketCheckSubscription += new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                poMarket.MarketDepthUpdate += new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                // Subscribe to the market. Use smart buffering.
                poMarket.DepthSubscribe(DepthBuffer.Smart, DepthLevels.BestOnly);
            }
        }

        private void UnsubscribeInstrument(T4Instrument t4instr)
        {
            cout("Unsubscribing from Instrument...");

            var poMarket = t4instr.Market;
            if ((poMarket != null))
            {
                // Unregister the events for this market.
                poMarket.MarketCheckSubscription -= new T4.API.Market.MarketCheckSubscriptionEventHandler(Markets_MarketCheckSubscription);
                poMarket.MarketDepthUpdate -= new T4.API.Market.MarketDepthUpdateEventHandler(Markets_MarketDepthUpdate);
                poMarket.DepthUnsubscribe();
            }
        }

        private void SendOrder(T4Instrument t4instr, ZS.ZOrder o)
        {

        }

    } // end of CLASS

} // end of NAMESPACE
