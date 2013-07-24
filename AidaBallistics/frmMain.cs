using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AidaBallisticsLibrary;

namespace AidaBallistics
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

           
            int k = 0;
            int w = 155; //weight in grains
            double bc = 0.5; // The ballistic coefficient for the projectile.
            double v = 2820; // Intial velocity, in ft/s
            double sh = 1.5; // The Sight height over bore, in inches.
            double angle = 0; // The shooting angle (uphill / downhill), in degrees.
            double zero = 200; // The zero range of the rifle, in yards.
            double windspeed = 0; // The wind speed in miles per hour.
            double windangle = 90; // The wind angle (0=headwind, 90=right to left, 180=tailwind, 270/-90=left to right)


            bc = GNUBL.AtmosphereCorrection(bc, 0, 29, 70, 0);
            double zeroangle = GNUBL.ZeroAngle(DragFunctions.G1, bc, v, sh, zero, 0);
            k = GNUBL.Solve(DragFunctions.G1, bc, v, sh, angle, zeroangle, windspeed, windangle);

            StringBuilder sb = new StringBuilder();
            for (int s = 0; s <= 1000; s+=50)
            {
                sb.AppendLine("Range: " + GNUBL.Solution.GetRange(s) + "\t|\tSpeed: " + Math.Round(GNUBL.Solution.GetVelocity(s),3) + "\t|\tEnergy: " + Math.Round(GNUBL.Solution.GetKineticEnergy(s, w),3) + "\t|\tMOAElevation: " + Math.Round(GNUBL.Solution.GetElevationMOA(s),3));
            }
            this.txtMain.Text = sb.ToString();
            

            //Aida.Inputs.BC = 0.314;
            //Aida.Inputs.HeightOfSight = 1.5;
            //Aida.Inputs.MuzzleVelocity = 2820;
            //Aida.Inputs.RangeIncrement = 50;
            //Aida.Inputs.RangeMax = 600;
            //Aida.Inputs.RangeMin = 0;
            //Aida.Inputs.ZeroedRange = 200;

            //if (Aida.Solve())
            //{
            //    this.dgvMain.DataSource = Aida.myDataset.Solution;
            //}
        }
    }
}
