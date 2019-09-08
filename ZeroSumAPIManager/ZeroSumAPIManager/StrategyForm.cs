using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
//using TradingTechnologies.TTAPI;
//using TradingTechnologies.TTAPI.WinFormsHelpers;
using ZeroSumAPI;
using Tools;
using GuiTools;
using static Tools.G;
using static Tools.GFile;

namespace ZeroSumAPI
{
    public partial class StrategyForm : Form
    {
        private TengineTT m_api;
        private TradingEngine m_te;

        private string m_pathname;
        //private StrategyA m_strategy;
        
        public StrategyForm()
        {
            InitializeComponent();

            if (CheckTTAPIArchitecture() == true)
            {
                m_api = new TengineTT();
                m_te = m_api as TradingEngine;
                //m_te.SetTradingEngineCallbacks(this);

                PopulateTradingEngineDropdown();

                G.COutput += G_COutput;
                //writer = new RichTextBoxConsole(rtbStrategyOutput);
                //Console.SetOut(writer);
            }
        }

        private void G_COutput(MessageArgs e)
        {
            if (rtbStrategyOutput.InvokeRequired)
                rtbStrategyOutput.Invoke(new Action<MessageArgs>(G_COutput), new object[] { e });
            else
            {
                rtbStrategyOutput.AppendText(e.Text + "\n");
                rtbStrategyOutput.ScrollToBottom();
            }
        }

        private void PopulateTradingEngineDropdown()
        {
            cboTradingEngines.Items.Add("TT");
            cboTradingEngines.Items.Add("CTS T4");
            cboTradingEngines.Items.Add("Kraken");
            cboTradingEngines.Items.Add("Bittrex");
            cboTradingEngines.Items.Add("XCrypto");
            cboTradingEngines.SelectedIndex = 0;
        }

        public bool CheckTTAPIArchitecture()
        {
            // Check that the compiler settings are compatible with the version of TT API installed
            TTAPIArchitectureCheck archCheck = new TTAPIArchitectureCheck();
            if (archCheck.validate())
            {
                cout("TTAPI Architecture check passed.");
                return true;
            }
            else
            {
                ErrorMessage("Architecture check failed.  {0}", archCheck.ErrorString);
                return false;
            }
        }

        private void StrategyForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!m_te.IsShutdown)
            {
                e.Cancel = true;
                m_te.Shutdown();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void GetReflectionInfo()
        {
            /*// Get the Type information
            Type myTypeObj = typeof(StrategyA);

            // Get Method information
            MethodInfo[] methodInfo = myTypeObj.GetMethods();

            listStrategyMethods.Items.Clear();
            foreach (var mi in methodInfo)
            {
                if (mi.IsPublic)
                {
                    Console.WriteLine("{0}  {1}", mi.Name, mi.MemberType);
                    if (mi.Name.StartsWith("Strategy_"))
                        listStrategyMethods.Items.Add(mi.Name);
                    //mi.IsStatic
                    //Type[] genArgs = mi.GetGenericArguments();
                    //foreach (var ga in genArgs)
                    //{
                    //    Console.WriteLine("  {0}", ga.Name);
                    //}
                }
            }*/
        }

        public void DisplayStrategyCode(string pathname)
        {
            m_pathname = pathname;
            List<string> lines = ReadTextFileLines(pathname);
            DisplayStrategyCode(lines);

            //m_strategy = new StrategyA(m_te);
            //m_te.SetTradingEngineCallbacks(m_strategy);

            GetReflectionInfo();
        }


        public void DisplayStrategyCode(List<string> lines)
        {
            rtbStrategyCode.Clear();
            rtbStrategyCode.WordWrap = false;

            foreach (string line in lines)
            {
                //Console.WriteLine(line);
                rtbStrategyCode.AppendText(line + "\n");
                //Application.DoEvents();
            }
        }

        private void btnRunStrategy_Click(object sender, EventArgs e)
        {
            //await CSharpScript.EvaluateAsync("System.Console.WriteLine(\"Eat my shorts!\")");
            //m_strategy.Strategy_CreateInstruments();
            string methodName = listStrategyMethods.SelectedItem as string;
            if (methodName != null)
            {
                cout("Invoking method: '{0}'", methodName);
                //MethodInfo method = m_strategy.GetType().GetMethod(methodName);
                //var mb = method.GetMethodBody();
                /*object result = method.Invoke(m_strategy, new object[] { });
                return (R)result;*/

                //method.Invoke(m_strategy, new object[] { });
            }
        }

        private string GetSourceCodeMethod(string pathname, string method)
        {
            List<string> lines = ReadTextFileLines(pathname);
            bool findOpenBrace = false, findCloseBrace = false;
            int braceCount = 0;
            List<string> methodLines = new List<string>();
            foreach (string line in lines)
            {
                if (findCloseBrace)
                {
                    int openCount = line.Count(x => x == '{');
                    int closeCount = line.Count(x => x == '}');
                    braceCount = braceCount + openCount - closeCount;
                    methodLines.Add(line);
                    if (braceCount <= 0)
                        break;
                }
                else if (findOpenBrace)
                {
                    if (line.Contains("{"))
                    {
                        findOpenBrace = false;
                        findCloseBrace = true;
                        braceCount = 1;
                    }
                    methodLines.Add(line);
                }
                else if (line.Contains(method + "()"))
                {
                    // TODO: Deal with comments, opening brace on same line as method, etc.
                    //cout(line);
                    methodLines.Add(line);
                    findOpenBrace = true;
                }
            }
            return string.Join("\n", methodLines);
        }

        private void listStrategyMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            string methodName = listStrategyMethods.SelectedItem as string;
            string source = GetSourceCodeMethod(m_pathname, methodName);
            rtbStrategyCode.Clear();
            rtbStrategyCode.AppendText(source);
        }

        private void timerStrategyManager_Tick(object sender, EventArgs e)
        {
            if (m_te.IsStarted == true && m_te.IsShutdown == false)
            {
                btnRunStrategy.BackColor = Color.Green;
            }
            else
            {
                btnRunStrategy.BackColor = Color.Red;
            }
        }
    } // end of class
} // end of namespace
