using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiTools.Grid
{
    /// <summary>
    /// To use this in code:
    /// myDataGridView.DataSource = Settings.Default.TheValues.Clone();             // In settings editor, add item of type MyMapCollection, called say TheValues, and use this line to load
    /// Settings.Default.TheValues = (MyMapCollection)myDataGridView.DataSource;    // If data should be written back to settinggs
    /// </summary>
    public class PropertyGrid : DataGridView
    {

    }

    public class MyProperty
    {
        public String Name { get; set; }
        public String Value { get; set; }

        //public MyProperty() { }       // thought this was needed to use object initializer, but apparently not
    }

    public class MyPropertyCollection : BindingList<MyProperty>
    {
        public MyPropertyCollection Clone()
        {
            MyPropertyCollection result = new MyPropertyCollection();

            foreach (MyProperty map in this)
                result.Add(new MyProperty()
                {
                    Name = map.Name,
                    Value = map.Value
                });

            return result;
        }

        public void Add(string propertyName, string propertyValue)
        {
            this.Add(new MyProperty { Name="xx", Value="yy" });
        }

    }

} // end of namespace
