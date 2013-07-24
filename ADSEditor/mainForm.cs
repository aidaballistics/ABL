using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADSEditor.ReadWriteCsv;
using AidaBallisticsLibrary;

namespace ADSEditor
{
    public partial class mainForm : Form
    {
        AidaDataSet ds = new AidaDataSet();
        
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader("D:\\_DATA\\_MyCode\\AidaBallisticsLibrary\\ABL\\stdatmospheredata.csv"))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    int n = 0;
                    AidaDataSet.StdAtmosphereDataRow myNewRow = ds.StdAtmosphereData.NewStdAtmosphereDataRow();
                    foreach (string s in row)
                    {
                        switch (n)
                        {
                            case 0:
                                myNewRow.Altitude = Convert.ToInt32(s);
                                break;

                            case 1:
                                myNewRow.Temperature = Convert.ToDouble(s);
                                break;

                            case 2:
                                myNewRow.Pressure = Convert.ToDouble(s);
                                break;

                            case 3:
                                myNewRow.AirDensity = Convert.ToDouble(s);
                                break;

                            case 4:
                                myNewRow.SpeedOfSound = Convert.ToDouble(s);
                                break;

                            case 5:
                                myNewRow.Viscosity= Convert.ToDouble(s);
                                break;
                        }
                        n++;
                    }
                    // add the row just created and move to next row
                    ds.StdAtmosphereData.AddStdAtmosphereDataRow(myNewRow);
                }
            }
            this.dataGridView1.DataSource = ds.StdAtmosphereData;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            this.ds.WriteXml("D:\\_DATA\\_MyCode\\AidaBallisticsLibrary\\ABL\\stdatmospheredata.xml");
        }
    }
}
