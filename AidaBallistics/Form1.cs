using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABL;

namespace AidaBallistics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int k = 0;
            double bc = 0.505; // The ballistic coefficient for the projectile.
            double v = 3000; // Intial velocity, in ft/s
            double sh = 1.5; // The Sight height over bore, in inches.
            double angle = 0; // The shooting angle (uphill / downhill), in degrees.
            double zero = 100; // The zero range of the rifle, in yards.
            double windspeed = 0; // The wind speed in miles per hour.
            double windangle = 0; // The wind angle (0=headwind, 90=right to left, 180=tailwind, 270/-90=left to right)

            double zeroangle = 0;
            zeroangle = Aida.ZeroAngle(Aida.DragFunctions.G1, bc, v, 1.6, zero, 0);

            k = Aida.Solve(Aida.DragFunctions.G1, bc, v, sh, angle, zeroangle, windspeed, windangle);

            StringBuilder sb = new StringBuilder();
            for (int s = 0; s <= 100; s++)
            {
                sb.AppendLine("Range: " + Aida.Solution.GetRange(s * 10) + "\t|\tEnergy: " + Aida.Solution.GetKineticEnergy(s * 10, 180) + "\t|\tMOAElevation: " + Aida.Solution.GetElevationMOA(s * 10));
            }
            this.txtMain.Text = sb.ToString();

        }
    }
}
