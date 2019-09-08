using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CryptoAPIs;
using ZeroSumAPI;
using static Tools.G;

namespace ZeroSumAPI_Test
{
    using TradingTechnologies.TTAPI;

    static class Program
    {

        [STAThread]
        static void Main()
        {
            string ttUserId = "PRIMEDTS2";
            string ttPassword = "12345678";

            // 0 = Console App
            // 1 = Test2Form (CTS T4API)
            // 11 TestForm (TTAPI) modified to create ttapi within Form
            // 2 = TestForm (TTAPI)
            // 21 = modified TTAPI Console App for TeTTApi
            // 3 = CryptosAPIsTest.Test()
            // 31 = CryptoTestForm
            // 4 = TTAPIFunctions Console App
            // 5 = another TTAPI test Form (not working)
            int startup = 31;

            //----------------------------------------------------------------------------------------------------------
            // Startup as Console App
            if (startup == 0)
            {
                var main = new ZeroSumAPI.Main();
                main.Test1();

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Test2Form());
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 11)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TestForm());
            }

            if (startup == 12)
            {
                // confirm TTAPI installation archetecture
                AboutDTS.TTAPIArchitectureCheck();

                XTraderModeTTAPIOptions envOptions = new XTraderModeTTAPIOptions();
                // Enable or Disable the TT API Implied Engine
                envOptions.EnableImplieds = false;

                // Create and attach a UI Dispatcher to the main Form
                // When the form exits, this scoping block will auto-dispose of the Dispatcher
                using (var disp = Dispatcher.AttachUIDispatcher())
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Create an instance of TTAPI.
                    frmPriceUpdate priceUpdate = new frmPriceUpdate();
                    ApiInitializeHandler handler = new ApiInitializeHandler(priceUpdate.ttApiInitHandler);
                    TTAPI.CreateXTraderModeTTAPI(disp, handler);

                    var frmTest = new Test3Form(priceUpdate);
                    Application.Run(frmTest);
                }
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 2)
            {
                // confirm TTAPI installation archetecture
                //AboutDTS.TTAPIArchitectureCheck();

                TTAPIArchitectureCheck archCheck = new TTAPIArchitectureCheck();
                if (archCheck.validate())
                {
                    Console.WriteLine("Architecture check passed.");

                    XTraderModeTTAPIOptions envOptions = new XTraderModeTTAPIOptions();
                    // Enable or Disable the TT API Implied Engine
                    envOptions.EnableImplieds = false;

                    // Create and attach a UI Dispatcher to the main Form
                    // When the form exits, this scoping block will auto-dispose of the Dispatcher
                    using (var disp = Dispatcher.AttachUIDispatcher())
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);

                        // Create an instance of TeTTApi.
                        TengineTT api = new TengineTT();
                        //ApiInitializeHandler handler = new ApiInitializeHandler(ttapi.ttApiInitHandler);
                        //TTAPI.CreateXTraderModeTTAPI(disp, handler);

                        //TestForm testForm = new TestForm(ttapi);
                        //Application.Run(testForm);
                    }
                }
                else
                {
                    ErrorMessage("TTAPIArchitectureCheck FAILED!");
                }
                return;
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 21)
            {
                // Check that the compiler settings are compatible with the version of TT API installed
                TTAPIArchitectureCheck archCheck = new TTAPIArchitectureCheck();
                if (archCheck.validate())
                {
                    Console.WriteLine("Architecture check passed.");

                    // Dictates whether TT API will be started on its own thread
                    bool startOnSeparateThread = true;

                    if (startOnSeparateThread)
                    {
                        cout("Starting TTAPI on SEPARATE thread");
                        // Start TT API on a separate thread
                        TengineTT api = new TengineTT();
                        Thread workerThread = new Thread(api.Start);
                        workerThread.Name = "TeTTApi TradingEngine Thread";
                        workerThread.Start();

                        // Insert other code here that will run on this thread
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new TestForm());

                    }
                    else
                    {
                        cout("Starting TTAPI on same thread");
                        // Start the TT API on the same thread
                        using (TengineTT api = new TengineTT())
                        {
                            api.Start();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Architecture check failed.  {0}", archCheck.ErrorString);
                }
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 3)
            {
                var crypto = new CryptoAPIsTest();
                crypto.Test();
                return;
            }
            //----------------------------------------------------------------------------------------------------------
            if (startup == 31)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CryptoTestForm());
            }
            //----------------------------------------------------------------------------------------------------------
            if (startup == 4)
            {
                // Check that the compiler settings are compatible with the version of TT API installed
                TTAPIArchitectureCheck archCheck = new TTAPIArchitectureCheck();
                if (archCheck.validate())
                {
                    Console.WriteLine("Architecture check passed.");

                    // Dictates whether TT API will be started on its own thread
                    bool startOnSeparateThread = false;

                    if (startOnSeparateThread)
                    {
                        // Start TT API on a separate thread
                        TTAPIFunctions tf = new TTAPIFunctions(ttUserId, ttPassword);
                        Thread workerThread = new Thread(tf.Start);
                        workerThread.Name = "TT API Thread";
                        workerThread.Start();

                        // Insert other code here that will run on this thread
                    }
                    else
                    {
                        // Start the TT API on the same thread
                        using (TTAPIFunctions tf = new TTAPIFunctions(ttUserId, ttPassword))
                        {
                            tf.Start();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Architecture check failed.  {0}", archCheck.ErrorString);
                }
            }

            //----------------------------------------------------------------------------------------------------------
            if (startup == 5)
            {
                /*// confirm TTAPI installation archetecture
                AboutDTS.TTAPIArchitectureCheck();

                XTraderModeTTAPIOptions envOptions = new XTraderModeTTAPIOptions();
                // Enable or Disable the TT API Implied Engine
                envOptions.EnableImplieds = false;

                // Create and attach a UI Dispatcher to the main Form
                // When the form exits, this scoping block will auto-dispose of the Dispatcher
                using (var disp = Dispatcher.AttachUIDispatcher())
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    //frmPriceUpdateManual priceUpdateManualConnection = new frmPriceUpdateManual();
                    TeTTApiForm ttapi = new TeTTApiForm();
                    ApiInitializeHandler handler = new ApiInitializeHandler(ttapi.ttApiInitHandler);
                    TTAPI.CreateXTraderModeTTAPI(disp, handler);

                    Application.Run(ttapi);
                }*/
            }

            //----------------------------------------------------------------------------------------------------------

        } // end of Main

    } // end of CLASS
} // end of NAMESPACE
