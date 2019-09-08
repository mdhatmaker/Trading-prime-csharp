using CryptoAPIs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Tools;
using static Tools.G;

namespace CryptoForms
{
    public partial class CryptoChartForm : Form
    {
        private ZCandlestickMap m_map;
        private string m_title = "";
        private int m_dpcount = 100;

        private static Dictionary<string, string> m_intervalText = new Dictionary<string, string>() { { "1m", "1-minute" }, { "5m", "5-minute" }, { "15m", "15-minute" }, { "30m", "30-minute" }, { "1h", "1-hour" }, { "1d", "daily" }, { "1800", "30-minute" }, { "minute", "1-minute" }, { "hour", "1-hour" }, { "daily", "daily" }, { "15minute", "15-minute" }, { "60minute", "1-hour" } };

        public CryptoChartForm()
        {
            InitializeComponent();
        }

        public void DisplayChart(ZCandlestickMap map)   //, string symbol, string interval)
        {
            m_map = map;
            m_title = string.Format("{0} {1}", map.Symbol, m_intervalText[map.Interval]);

            chart1.BackColor = Color.Black;
            chart1.ForeColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Black;
            
            // Chart Title
            this.Text = "Crypto Charts: " + m_title;
            lblTitle.Text = m_title;
            /*chart1.Titles.Clear();
            chart1.Titles.Add(m_title);
            chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
            chart1.Titles[0].ForeColor = Color.White;*/

            // Chart Series
            chart1.Series.Clear();
            var series = new Series("candlestick");
            series.ChartType = SeriesChartType.Candlestick;     // Set series chart type
            series["OpenCloseStyle"] = "Triangle";              // Set the style of the open-close marks
            series["ShowOpenClose"] = "Both";                   // Show both open and close marks
            series["PointWidth"] = "1.0";                       // Set point width
            series["PriceUpColor"] = "Green";                   // Set colors bars (up)
            series["PriceDownColor"] = "Red";                   // Set colors bars (down)

            double min = double.MaxValue, max = double.MinValue;

            // Add the datapoints to the series
            foreach (var kv in map)
            {
                DataPoint item = new DataPoint((double) kv.Key.ToUnixTimestamp(), kv.Value.yValues);
                max = Math.Max(max, kv.Value.yValues[0]);
                min = Math.Min(min, kv.Value.yValues[1]);
                item.AxisLabel = kv.Key.ToString("MM-dd HH:mm");
                series.Points.Add(item);
            }
            chart1.ChartAreas[0].AxisY.Maximum = (max * 1.05);
            chart1.ChartAreas[0].AxisY.Minimum = (min * 0.95);
            chart1.Series.Add(series);

            UpdateChartScale();
        }

        public bool UpdateChartScale()
        {
            try
            {
                // Create the chart scale to show m_dpcount data points
                chart1.ChartAreas[0].AxisX.ScaleView.Size = m_dpcount * 60 * 60;
                return true;
            }
            catch { return false; }

        }

        /*public bool Draw()
        {
            try
            {
                view.Data = this.dllCall.GetData(1);

                int startSecond = 0;
                foreach (Int16 item in view.Data)
                {
                    this.view.chart.Series["MySeries"].Points.AddXY(startSecond, item);
                    startSecond++;

                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage("CryptoChartForm::Draw => {0}", ex.Message);
                return false;
            }
        }*/

        private void chart_AxisScrollBarClicked(object sender, System.Windows.Forms.DataVisualization.Charting.ScrollBarEventArgs e)
        {
            if (e.Axis == chart1.ChartAreas[0].AxisX)
            {
                if (e.ButtonType == System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonType.SmallIncrement)
                    chart1.ChartAreas[0].AxisX.ScaleView.Position += m_dpcount;
                else if (e.ButtonType == System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonType.SmallDecrement)
                    chart1.ChartAreas[0].AxisX.ScaleView.Position -= m_dpcount;
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            m_dpcount = (int)(m_dpcount * 0.75);
            UpdateChartScale();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            m_dpcount = (int)(m_dpcount * 1.5);
            UpdateChartScale();
        }

        private void chart1_AxisViewChanged(object sender, ViewEventArgs e)
        {
            /*int min = (int)chart1.ChartAreas[0].AxisX.Minimum;
            int max = (int)chart1.ChartAreas[0].AxisX.Maximum;

            if (max > chart1.Series[0].Points.Count)
                max = chart1.Series[0].Points.Count;

            var points = chart1.Series[0].Points.Skip(min).Take(max - min);

            var minValue = points.Min(x => x.YValues[0]);
            var maxValue = points.Max(x => x.YValues[1]);

            chart1.ChartAreas[0].AxisY.Minimum = minValue;
            chart1.ChartAreas[0].AxisY.Maximum = maxValue;*/
        }
    } // end of class CryptoChartForm
} // end of namespace
