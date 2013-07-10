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
using ABL;

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
            using (CsvFileReader reader = new CsvFileReader("D:\\_DATA\\_MyCode\\AidaBallisticsLibrary\\ABL\\dataset.csv"))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    int n = 0;
                    AidaDataSet.BulletsRow myNewRow = ds.Bullets.NewBulletsRow();
                    foreach (string s in row)
                    {
                        switch (n)
                        {
                            case 0:
                                myNewRow.Caliber = Convert.ToDouble(s);
                                break;

                            case 1:
                                myNewRow.Brand = s;
                                break;

                            case 2:
                                myNewRow.Name = s;
                                break;

                            case 3:
                                myNewRow.StatedWeight = Convert.ToDouble(s);
                                break;

                            case 4:
                                myNewRow.TrueWeight = Convert.ToDouble(s);
                                break;

                            case 5:
                                if (s == "N/A")
                                {
                                    myNewRow.Length = 0;
                                    break;
                                }
                                else
                                    myNewRow.Length = Convert.ToDouble(s);
                                break;

                            case 6:
                                myNewRow.SectionalDensity = Convert.ToDouble(s);
                                break;

                            case 7:
                                myNewRow.BC1 = Convert.ToDouble(s);
                                break;

                            case 8:
                                myNewRow.BC2 = Convert.ToDouble(s);
                                break;

                            case 9:
                                myNewRow.BC3 = Convert.ToDouble(s);
                                break;

                            case 10:
                                myNewRow.BC4 = Convert.ToDouble(s);
                                break;

                            case 11:
                                myNewRow.BC5 = Convert.ToDouble(s);
                                break;

                            case 12:
                                myNewRow.V1 = Convert.ToInt32(s);
                                break;

                            case 13:
                                myNewRow.V2 = Convert.ToInt32(s);
                                break;

                            case 14:
                                myNewRow.V3 = Convert.ToInt32(s);
                                break;

                            case 15:
                                myNewRow.V4 = Convert.ToInt32(s);
                                break;
                        }
                        n++;
                    }
                    myNewRow.UniqueID = Guid.NewGuid();
                    // add the row just created and move to next row
                    ds.Bullets.AddBulletsRow(myNewRow);
                }
            }
            this.dataGridView1.DataSource = ds.Bullets;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            this.ds.WriteXml("D:\\_DATA\\_MyCode\\AidaBallisticsLibrary\\ABL\\dataset.xml");
        }
    }
}
