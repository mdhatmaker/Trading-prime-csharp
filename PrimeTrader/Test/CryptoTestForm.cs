using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using CryptoAPIs;

namespace ZeroSumAPI_Test
{
    public partial class CryptoTestForm : Form
    {
        public CryptoTestForm()
        {
            InitializeComponent();

            GetReflectionInfo();
        }

        private void GetReflectionInfo()
        {
            // Get the Type information
            Type myTypeObj = typeof(BlockchainInfo);

            // Get Method information
            MethodInfo[] methodInfo = myTypeObj.GetMethods();

            foreach (var mi in methodInfo)
            {
                if (mi.IsStatic)
                {
                    Console.WriteLine("{0}  {1} {2}", mi.Name, mi.MemberType, mi.IsStatic);
                    Type[] genArgs = mi.GetGenericArguments();
                    foreach (var ga in genArgs)
                    {
                        Console.WriteLine("  {0}", ga.Name);
                    }
                }
            }
        }

    } // end of class
} // end of namespace
