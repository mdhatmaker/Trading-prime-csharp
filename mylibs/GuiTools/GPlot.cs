using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
//using System.Windows.Forms;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System.Net;
using System.Drawing;
using static Tools.G;

namespace GuiTools
{
    // http://matplotlib.org/users/pyplot_tutorial.html

    public static class GPlot
    {
        static public Dictionary<string, Color> PlotColors = new Dictionary<string, Color>()
        {
            { "b:blue", Color.Blue },
            { "g:green", Color.Green },
            { "r:red", Color.Red },
            { "c:cyan", Color.Cyan },
            { "m:magenta", Color.Magenta },
            { "y:yellow", Color.Yellow },
            { "k:black", Color.Black },
            { "w:white", Color.White },
        };

        static public string BlankPlotLineStyle = " ";
        static public string DefaultPlotLineStyle = "_ line";
        static public Dictionary<string, string> PlotLineStyles = new Dictionary<string, string>()
        {
            { BlankPlotLineStyle," " },             // BlankPlotLinestyle
            { DefaultPlotLineStyle,"_" },           // DefaultPlotLineStyle
            { ". dot","." },
            { "- dash","-" },
            { "x marker","x" },
            { "o marker","o" },
        };

    } // end of class
} // end of namespace
