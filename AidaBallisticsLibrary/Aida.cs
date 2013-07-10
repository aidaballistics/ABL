using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ABL
{
    public static class Aida
    {
        public enum DragFunctions { G1, G2, G3, G4, G5, G6, G7, G8 };
        internal const double GRAVITY = -32.194;
        internal const int BCOMP_MAXRANGE = 50001;
        internal static double[] sln; //an array holding the solution data produced by the solve algorithmn.  
        internal static AidaDataSet myDataset = new AidaDataSet();

        /// <summary>A static class containing public functions realted to obtaining the solution data.</summary>
        public static class Solution
        {
            /// <summary>A function to get a solid int for the range at the indexed yardage.</summary>
            /// <returns>Returns range, in yards.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetRange(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage];

                }
                else return 0;
            }

            /// <summary>A function to get elevation correction, in inches.</summary>
            /// <returns>Returns projectile path, in inches, relative to the line of sight.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetElevation(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 1];
                }
                else return 0;
            }

            /// <summary>A function to get elevation correction in MOA.</summary>
            /// <returns>Returns an estimated elevation correction for achieving a zero at this range; this is useful for "click charts" and the like.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetElevationMOA(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 2];
                }
                else return 0;
            }

            /// <summary>A function to get time in flight at the given yardage/range.</summary>
            /// <returns>Returns the projectile's time of flight to this range.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetTime(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 3];
                }
                else return 0;
            }

            /// <summary>A function to get windage correction in inches.</summary>
            /// <returns>Returns the windage correction in inches required to achieve zero at this range.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetWindage(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 4];
                }
                else return 0;
            }

            /// <summary>A function to get windage correction in MOA.</summary>
            /// <returns>Returns an approximate windage correction in MOA to achieve a zero at this range.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetWindageMOA(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 5];
                }
                else return 0;
            }

            /// <summary>A function to get total velocity.</summary>
            /// <returns>Returns the projectile's total velocity (Vector product of Vx and Vy).</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetVelocity(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 6];
                }
                else return 0;
            }

            /// <summary>A function to get the velocity along the x-axis.</summary>
            /// <returns>Returns the velocity of the projectile in the bore direction.</returns>
            /// <param name="Yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetVx(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 7];
                }
                else return 0;
            }

            /// <summary>A function to get the velocity along the y-axis.</summary>
            /// <returns>Returns the velocity of the projectile perpendicular to the bore direction.</returns>
            /// <param name="yardage">An int representing the the yardage at which to get the data.</param>
            public static double GetVy(int Yardage)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return sln[10 * Yardage + 8];
                }
                else return 0;
            }

            /// <summary>A function to get the force/energy of the bullet.</summary>
            /// <returns>Returns the the energy of the bullet, in ft/lbs, at the given yardage/index.</returns>
            /// <param name="yardage">An int representing the the yardage at which to get the data.</param>
            /// <param name="Yardage">An int representing bullet weight, in grains.</param>
            public static double GetKineticEnergy(int Yardage, int Weight)
            {
                if (Yardage < sln.GetLength(0))
                {
                    return (Internal.SigFigTruncate(Math.Pow(Aida.Solution.GetVelocity(Yardage), 2) * Weight / 450400, 6));
                }
                else return 0;
            }

        }

        /// <summary>A static class containing all functions used only internally by the library.</summary>
        internal static class Internal
        {
            /// <summary>Rounds to sig fig.</summary>
            internal static double SigFigRound(double d, int digits)
            {
                if (d == 0)
                    return 0;

                double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
                return scale * Math.Round(d / scale, digits);
            }

            /// <summary>Truncates to sig fig.</summary>
            internal static double SigFigTruncate(double d, int digits)
            {
                if (d == 0)
                    return 0;

                double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1 - digits);
                return scale * Math.Truncate(d / scale);
            }

            /// <summary>Converts degrees to minutes of angle</summary>
            internal static double DegtoMOA(double deg)
            {
                return deg * 60;
            }

            /// <summary>Converts degrees to radians</summary>
            internal static double DegtoRad(double deg)
            {
                return deg * Math.PI / 180;
            }

            /// <summary>Converts minutes of angle to degrees</summary>
            internal static double MOAtoDeg(double moa)
            {
                return moa / 60;
            }

            /// <summary>Converts minutes of angle to radians</summary>
            internal static double MOAtoRad(double moa)
            {
                return moa / 60 * Math.PI / 180;
            }

            /// <summary>Converts radians to degrees</summary>
            internal static double RadtoDeg(double rad)
            {
                return rad * 180 / Math.PI;
            }

            /// <summary>Converts radiants to minutes of angle</summary>
            internal static double RadtoMOA(double rad)
            {
                return rad * 60 * 180 / Math.PI;
            }

            internal static double calcFR(double Temperature, double Pressure, double RelativeHumidity)
            {
                double VPw = 4e-6 * Math.Pow(Temperature, 3) - 0.0004 * Math.Pow(Temperature, 2) + 0.0234 * Temperature - 0.2517;
                double FRH = 0.995 * (Pressure / (Pressure - (0.3783) * (RelativeHumidity) * VPw));
                return FRH;
            }

            internal static double calcFP(double Pressure)
            {
                double Pstd = 29.53; // in-hg
                double FP = 0;
                FP = (Pressure - Pstd) / (Pstd);
                return FP;
            }

            internal static double calcFT(double Temperature, double Altitude)
            {
                double Tstd = -0.0036 * Altitude + 59;
                double FT = (Temperature - Tstd) / (459.6 + Tstd);
                return FT;
            }

            internal static double calcFA(double Altitude)
            {
                double fa = 0;
                fa = -4e-15 * Math.Pow(Altitude, 3) + 4e-10 * Math.Pow(Altitude, 2) - 3e-5 * Altitude + 1;
                return (1 / fa);
            }

        }

        /// <summary>A function to calculate ballistic retardation values based on standard drag functions.</summary>
        /// <returns>Returns the projectile drag retardation velocity, in ft/s per second.</returns>
        /// <param name="DragFunction">DragFunction:  G1, G2, G3, G4, G5, G6, G7, or G8.  All are enumerated above.</param>
        /// <param name="DragCoefficient">DragCoefficient:  The coefficient of drag for the projectile for the given drag function.</param>
        /// <param name="Vi">DragCoefficient:  The coefficient of drag for the projectile for the given drag function.</param>
        internal static double retard(DragFunctions DragFunction, double DragCoefficient, double Vi)
        {
            double vp = Vi;
            double val = -1;
            double A = -1;
            double M = -1;
            switch (DragFunction)
            {
                case DragFunctions.G1:
                    if (vp > 4230) { A = 1.477404177730177e-04; M = 1.9565; }
                    else if (vp > 3680) { A = 1.920339268755614e-04; M = 1.925; }
                    else if (vp > 3450) { A = 2.894751026819746e-04; M = 1.875; }
                    else if (vp > 3295) { A = 4.349905111115636e-04; M = 1.825; }
                    else if (vp > 3130) { A = 6.520421871892662e-04; M = 1.775; }
                    else if (vp > 2960) { A = 9.748073694078696e-04; M = 1.725; }
                    else if (vp > 2830) { A = 1.453721560187286e-03; M = 1.675; }
                    else if (vp > 2680) { A = 2.162887202930376e-03; M = 1.625; }
                    else if (vp > 2460) { A = 3.209559783129881e-03; M = 1.575; }
                    else if (vp > 2225) { A = 3.904368218691249e-03; M = 1.55; }
                    else if (vp > 2015) { A = 3.222942271262336e-03; M = 1.575; }
                    else if (vp > 1890) { A = 2.203329542297809e-03; M = 1.625; }
                    else if (vp > 1810) { A = 1.511001028891904e-03; M = 1.675; }
                    else if (vp > 1730) { A = 8.609957592468259e-04; M = 1.75; }
                    else if (vp > 1595) { A = 4.086146797305117e-04; M = 1.85; }
                    else if (vp > 1520) { A = 1.954473210037398e-04; M = 1.95; }
                    else if (vp > 1420) { A = 5.431896266462351e-05; M = 2.125; }
                    else if (vp > 1360) { A = 8.847742581674416e-06; M = 2.375; }
                    else if (vp > 1315) { A = 1.456922328720298e-06; M = 2.625; }
                    else if (vp > 1280) { A = 2.419485191895565e-07; M = 2.875; }
                    else if (vp > 1220) { A = 1.657956321067612e-08; M = 3.25; }
                    else if (vp > 1185) { A = 4.745469537157371e-10; M = 3.75; }
                    else if (vp > 1150) { A = 1.379746590025088e-11; M = 4.25; }
                    else if (vp > 1100) { A = 4.070157961147882e-13; M = 4.75; }
                    else if (vp > 1060) { A = 2.938236954847331e-14; M = 5.125; }
                    else if (vp > 1025) { A = 1.228597370774746e-14; M = 5.25; }
                    else if (vp > 980) { A = 2.916938264100495e-14; M = 5.125; }
                    else if (vp > 945) { A = 3.855099424807451e-13; M = 4.75; }
                    else if (vp > 905) { A = 1.185097045689854e-11; M = 4.25; }
                    else if (vp > 860) { A = 3.566129470974951e-10; M = 3.75; }
                    else if (vp > 810) { A = 1.045513263966272e-08; M = 3.25; }
                    else if (vp > 780) { A = 1.291159200846216e-07; M = 2.875; }
                    else if (vp > 750) { A = 6.824429329105383e-07; M = 2.625; }
                    else if (vp > 700) { A = 3.569169672385163e-06; M = 2.375; }
                    else if (vp > 640) { A = 1.839015095899579e-05; M = 2.125; }
                    else if (vp > 600) { A = 5.71117468873424e-05; M = 1.950; }
                    else if (vp > 550) { A = 9.226557091973427e-05; M = 1.875; }
                    else if (vp > 250) { A = 9.337991957131389e-05; M = 1.875; }
                    else if (vp > 100) { A = 7.225247327590413e-05; M = 1.925; }
                    else if (vp > 65) { A = 5.792684957074546e-05; M = 1.975; }
                    else if (vp > 0) { A = 5.206214107320588e-05; M = 2.000; }
                    break;

                case DragFunctions.G2:
                    if (vp > 1674) { A = .0079470052136733; M = 1.36999902851493; }
                    else if (vp > 1172) { A = 1.00419763721974e-03; M = 1.65392237010294; }
                    else if (vp > 1060) { A = 7.15571228255369e-23; M = 7.91913562392361; }
                    else if (vp > 949) { A = 1.39589807205091e-10; M = 3.81439537623717; }
                    else if (vp > 670) { A = 2.34364342818625e-04; M = 1.71869536324748; }
                    else if (vp > 335) { A = 1.77962438921838e-04; M = 1.76877550388679; }
                    else if (vp > 0) { A = 5.18033561289704e-05; M = 1.98160270524632; }
                    break;

                case DragFunctions.G5:
                    if (vp > 1730) { A = 7.24854775171929e-03; M = 1.41538574492812; }
                    else if (vp > 1228) { A = 3.50563361516117e-05; M = 2.13077307854948; }
                    else if (vp > 1116) { A = 1.84029481181151e-13; M = 4.81927320350395; }
                    else if (vp > 1004) { A = 1.34713064017409e-22; M = 7.8100555281422; }
                    else if (vp > 837) { A = 1.03965974081168e-07; M = 2.84204791809926; }
                    else if (vp > 335) { A = 1.09301593869823e-04; M = 1.81096361579504; }
                    else if (vp > 0) { A = 3.51963178524273e-05; M = 2.00477856801111; }
                    break;

                case DragFunctions.G6:
                    if (vp > 3236) { A = 0.0455384883480781; M = 1.15997674041274; }
                    else if (vp > 2065) { A = 7.167261849653769e-02; M = 1.10704436538885; }
                    else if (vp > 1311) { A = 1.66676386084348e-03; M = 1.60085100195952; }
                    else if (vp > 1144) { A = 1.01482730119215e-07; M = 2.9569674731838; }
                    else if (vp > 1004) { A = 4.31542773103552e-18; M = 6.34106317069757; }
                    else if (vp > 670) { A = 2.04835650496866e-05; M = 2.11688446325998; }
                    else if (vp > 0) { A = 7.50912466084823e-05; M = 1.92031057847052; }
                    break;

                case DragFunctions.G7:
                    if (vp > 4200) { A = 1.29081656775919e-09; M = 3.24121295355962; }
                    else if (vp > 3000) { A = 0.0171422231434847; M = 1.27907168025204; }
                    else if (vp > 1470) { A = 2.33355948302505e-03; M = 1.52693913274526; }
                    else if (vp > 1260) { A = 7.97592111627665e-04; M = 1.67688974440324; }
                    else if (vp > 1110) { A = 5.71086414289273e-12; M = 4.3212826264889; }
                    else if (vp > 960) { A = 3.02865108244904e-17; M = 5.99074203776707; }
                    else if (vp > 670) { A = 7.52285155782535e-06; M = 2.1738019851075; }
                    else if (vp > 540) { A = 1.31766281225189e-05; M = 2.08774690257991; }
                    else if (vp > 0) { A = 1.34504843776525e-05; M = 2.08702306738884; }
                    break;

                case DragFunctions.G8:
                    if (vp > 3571) { A = .0112263766252305; M = 1.33207346655961; }
                    else if (vp > 1841) { A = .0167252613732636; M = 1.28662041261785; }
                    else if (vp > 1120) { A = 2.20172456619625e-03; M = 1.55636358091189; }
                    else if (vp > 1088) { A = 2.0538037167098e-16; M = 5.80410776994789; }
                    else if (vp > 976) { A = 5.92182174254121e-12; M = 4.29275576134191; }
                    else if (vp > 0) { A = 4.3917343795117e-05; M = 1.99978116283334; }
                    break;

                default:
                    break;

            }

            if (A != -1 && M != -1 && vp > 0 && vp < 10000)
            {
                val = A * Math.Pow(vp, M) / DragCoefficient;
                return val;
            }
            else return -1;
        }

        /// <summary>A function to correct a "standard" Drag Coefficient for differing atmospheric conditions.</summary>
        /// <returns>Returns a ballistic coefficient, corrected for the supplied atmospheric conditions.</returns>
        /// <param name="DragCoefficient">DragCoefficient:  The coefficient of drag for a given projectile.</param>
        /// <param name="Altitude">Altitude:  The altitude above sea level in feet.  Standard altitude is 0 feet above sea level.</param>
        /// <param name="Barometer">Barometer:  The barometric pressure in inches of mercury (in Hg).  This is not "absolute" pressure, it is the "standardized" pressure reported in the papers and news. Standard pressure is 29.53 in Hg.</param>
        /// <param name="Temperature">Temperature:  The temperature in Fahrenheit.  Standard temperature is 59 degrees.</param>
        /// <param name="RelativeHumidity">RelativeHumidity:  The relative humidity fraction.  Ranges from 0.00 to 1.00, with 0.50 being 50% relative humidity.  Standard humidity is 78%.</param>
        internal static double AtmosphereCorrection(double DragCoefficient, double Altitude, double Barometer, double Temperature, double RelativeHumidity)
        {
            double FA = Internal.calcFA(Altitude);
            double FT = Internal.calcFT(Temperature, Altitude);
            double FR = Internal.calcFR(Temperature, Barometer, RelativeHumidity);
            double FP = Internal.calcFP(Barometer);

            // Calculate the atmospheric correction factor
            double CD = (FA * (1 + FT - FP) * FR);
            return DragCoefficient * CD;
        }

        /// <summary>A function to compute the windage deflection for a given crosswind speed, given flight time in a vacuum, and given flight time in real life.</summary>
        /// <returns>Returns the amount of windage correction, in inches, required to achieve zero on a target at the given range.</returns>
        /// <param name="WindSpeed">WindSpeed:  The wind velocity in mi/hr.</param>
        /// <param name="Vi">Vi:  The initial velocity of the projectile (muzzle velocity).</param>
        /// <param name="x">x:  The range at which you wish to determine windage, in feet.</param>
        /// <param name="t">t:  The time it has taken the projectile to traverse the range x, in seconds.</param>
        internal static double Windage(double WindSpeed, double Vi, double x, double t)
        {
            double Vw = WindSpeed * 17.60; // Convert to inches per second.
            return (Vw * (t - x / Vi));
        }

        /// <summary>A function to resolve any wind / angle combination into headwind and crosswind components.</summary>
        /// <returns>Returns the headwind velocity component, in mi/hr.</returns>
        /// <param name="WindSpeed">WindSpeed:  The wind velocity in mi/hr.</param>
        /// <param name="WindAngle">WindAngle:  The angle from which the wind is coming, in degrees.  0 degrees is from straight ahead; 90 degrees is from right to left; 180 degrees is from directly behind; 270 or -90 degrees is from left to right.</param>
        internal static double HeadWind(double WindSpeed, double WindAngle)
        {
            double Wangle = Internal.DegtoRad(WindAngle);
            return (Math.Cos(Wangle) * WindSpeed);
        }

        /// <summary>A function to resolve any wind / angle combination into headwind and crosswind components.</summary>
        /// <returns>Returns the crosswind velocity component, in mi/hr.</returns>
        /// <param name="WindSpeed">WindSpeed:  The wind velocity in mi/hr.</param>
        /// <param name="WindAngle">WindAngle:  The angle from which the wind is coming, in degrees.  0 degrees is from straight ahead; 90 degrees is from right to left; 180 degrees is from directly behind; 270 or -90 degrees is from left to right.</param>
        internal static double CrossWind(double WindSpeed, double WindAngle)
        {
            double Wangle = Internal.DegtoRad(WindAngle);
            return (Math.Sin(Wangle) * WindSpeed);
        }

        /// <summary>A function to determine the bore angle needed to achieve a target zero at Range yards (at standard conditions and on level ground.)</summary>
        /// <returns>Returns the angle of the bore relative to the sighting system, in degrees.</returns>
        /// <param name="DragFunction">DragFunction:  The drag function to use (G1, G2, G3, G5, G6, G7, G8).</param>
        /// <param name="DragCoefficient">DragCoefficient:  The coefficient of drag for the projectile, for the supplied drag function.</param>
        /// <param name="Vi">Vi:  The initial velocity of the projectile (muzzle velocity).</param>
        /// <param name="SightHeight">SightHeight:  The height of the sighting system above the bore centerline, in inches.  Most scopes fall in the 1.6 to 2.0 inch range.</param>
        /// <param name="ZeroRange">ZeroRange:  The range in yards, at which you wish the projectile to intersect yIntercept.</param>
        /// <param name="yIntercept">yIntercept:  The height, in inches, you wish for the projectile to be when it crosses ZeroRange yards.  This is usually 0 for a target zero, but could be any number.  For example if you wish to sight your rifle in 1.5" high at 100 yds, then you would set yIntercept to 1.5, and ZeroRange to 100.</param>
        public static double ZeroAngle(DragFunctions DragFunction, double DragCoefficient, double Vi, double SightHeight, double ZeroRange, double yIntercept)
        {
            // Numerical Integration variables
            double t = 0;
            double dt = 1 / Vi; // The solution accuracy generally doesn't suffer if its within a foot for each second of time.
            double y = -SightHeight / 12;
            double x = 0;
            double da; // The change in the bore angle used to iterate in on the correct zero angle.

            // State variables for each integration loop.
            double v = 0, vx = 0, vy = 0; // velocity
            double vx1 = 0, vy1 = 0; // Last frame's velocity, used for computing average velocity.
            double dv = 0, dvx = 0, dvy = 0; // acceleration
            double Gx = 0, Gy = 0; // Gravitational acceleration

            double angle = 0; // The actual angle of the bore.

            int quit = 0; // We know it's time to quit our successive approximation loop when this is 1.

            // Start with a very coarse angular change, to quickly solve even large launch angle problems.
            da = Aida.Internal.DegtoRad(14);


            // The general idea here is to start at 0 degrees elevation, and increase the elevation by 14 degrees
            // until we are above the correct elevation.  Then reduce the angular change by half, and begin reducing
            // the angle.  Once we are again below the correct angle, reduce the angular change by half again, and go
            // back up.  This allows for a fast successive approximation of the correct elevation, usually within less
            // than 20 iterations.
            for (angle = 0; quit == 0; angle = angle + da)
            {
                vy = Vi * Math.Sin(angle);
                vx = Vi * Math.Cos(angle);
                Gx = GRAVITY * Math.Sin(angle);
                Gy = GRAVITY * Math.Cos(angle);

                for (t = 0, x = 0, y = -SightHeight / 12; x <= ZeroRange * 3; t = t + dt)
                {
                    vy1 = vy;
                    vx1 = vx;
                    v = Math.Pow((Math.Pow(vx, 2) + Math.Pow(vy, 2)), 0.5);
                    dt = 1 / v;

                    dv = Aida.retard(DragFunction, DragCoefficient, v);
                    dvy = -dv * vy / v * dt;
                    dvx = -dv * vx / v * dt;

                    vx = vx + dvx;
                    vy = vy + dvy;
                    vy = vy + dt * Gy;
                    vx = vx + dt * Gx;

                    x = x + dt * (vx + vx1) / 2;
                    y = y + dt * (vy + vy1) / 2;

                    // Break early to save CPU time if we won't find a solution.
                    if (vy < 0 && y < yIntercept)
                    {
                        break;
                    }
                    if (vy > 3 * vx)
                    {
                        break;
                    }
                }

                if (y > yIntercept && da > 0)
                {
                    da = -da / 2;
                }

                if (y < yIntercept && da < 0)
                {
                    da = -da / 2;
                }

                if (Math.Abs(da) < Aida.Internal.MOAtoRad(0.01)) quit = 1; // If our accuracy is sufficient, we can stop approximating.
                if (angle > Aida.Internal.DegtoRad(45)) quit = 1; // If we exceed the 45 degree launch angle, then the projectile just won't get there, so we stop trying.

            }


            return Aida.Internal.RadtoDeg(angle); // Convert to degrees for return value.
        }

        /// <summary>A function to generate a ballistic solution table in 1 yard increments, up to __BCOMP_MAXRANGE__.</summary>
        /// <returns>This function returns an integer representing the maximum valid range of the solution.  This also indicates the maximum number of rows in the solution matrix, and should not be exceeded in order to avoid a memory segmentation fault.</returns>
        /// <param name="DragFunction">DragFunction:  The drag function to use (G1, G2, G3, G5, G6, G7, G8).</param>
        /// <param name="DragCoefficient">DragCoefficient:  The coefficient of drag for the projectile, for the supplied drag function.</param>
        /// <param name="Vi">Vi:  The initial velocity of the projectile (muzzle velocity).</param>
        /// <param name="SightHeight">SightHeight:  The height of the sighting system above the bore centerline, in inches.  Most scopes fall in the 1.6 to 2.0 inch range.</param>
        /// <param name="ShootingAngle">ShootingAngle:  The uphill or downhill shooting angle, in degrees.  Usually 0, but can be anything from 90 (directly up), to -90 (directly down).</param>
        /// <param name="ZeroAngle">The angle of the sighting system relative to the bore, in degrees.  This can be easily computed using the ZeroAngle() function documented above.</param>
        /// <param name="WindSpeed">WindSpeed:  The wind velocity in mi/hr.</param>
        /// <param name="WindAngle">WindAngle:  The angle from which the wind is coming, in degrees.  0 degrees is from straight ahead; 90 degrees is from right to left; 180 degrees is from directly behind; 270 or -90 degrees is from left to right.</param>
        /// <param name="Solution">Solution:	A pointer provided for accessing the solution after it has been generated.  Memory for this pointer is allocated in the function, so the user does not need to worry about it.  This solution can be passed to the retrieval functions to get useful data from the solution.</param>
        public static int Solve(DragFunctions DragFunction, double DragCoefficient, double Vi, double SightHeight, double ShootingAngle, double ZeroAngle, double WindSpeed, double WindAngle)
        {
            double[] ptr = new double[10 * BCOMP_MAXRANGE + 2048];

            double t = 0;
            double dt = 0.5 / Vi;
            double v = 0;
            double vx = 0, vx1 = 0, vy = 0, vy1 = 0;
            double dv = 0, dvx = 0, dvy = 0;
            double x = 0, y = 0;

            double headwind = Aida.HeadWind(WindSpeed, WindAngle);
            double crosswind = Aida.CrossWind(WindSpeed, WindAngle);

            double Gy = GRAVITY * Math.Cos(Internal.DegtoRad((ShootingAngle + ZeroAngle)));
            double Gx = GRAVITY * Math.Sin(Internal.DegtoRad((ShootingAngle + ZeroAngle)));

            vx = Vi * Math.Cos(Internal.DegtoRad(ZeroAngle));
            vy = Vi * Math.Sin(Internal.DegtoRad(ZeroAngle));

            y = -SightHeight / 12;

            int n = 0;
            for (t = 0; ; t = t + dt)
            {

                vx1 = vx;
                vy1 = vy;
                v = Math.Pow(Math.Pow(vx, 2) + Math.Pow(vy, 2), 0.5);
                dt = 0.5 / v;

                // Compute acceleration using the drag function retardation	
                dv = retard(DragFunction, DragCoefficient, v + headwind);
                dvx = -(vx / v) * dv;
                dvy = -(vy / v) * dv;

                // Compute velocity, including the resolved gravity vectors.	
                vx = vx + dt * dvx + dt * Gx;
                vy = vy + dt * dvy + dt * Gy;



                if (x / 3 >= n)
                {
                    ptr[10 * n + 0] = x / 3;    // Range in yds
                    ptr[10 * n + 1] = y * 12;	// Path in inches
                    ptr[10 * n + 2] = -Aida.Internal.RadtoMOA(Math.Atan(y / x));    // Correction in MOA
                    ptr[10 * n + 3] = t + dt;   // Time in s
                    ptr[10 * n + 4] = Aida.Windage(crosswind, Vi, x, t + dt);    // Windage in inches
                    ptr[10 * n + 5] = Aida.Internal.RadtoMOA(Math.Atan(ptr[10 * n + 4]));   // Windage in MOA
                    ptr[10 * n + 6] = v;    // Velocity (combined)
                    ptr[10 * n + 7] = vx;   // Velocity (x)
                    ptr[10 * n + 8] = vy;	// Velocity (y)
                    ptr[10 * n + 9] = 0;	// Reserved
                    n++;
                }

                // Compute position based on average velocity.
                x = x + dt * (vx + vx1) / 2;
                y = y + dt * (vy + vy1) / 2;

                if (Math.Abs(vy) > Math.Abs(3 * vx)) break;
                if (n >= BCOMP_MAXRANGE + 1) break;
            }

            ptr[10 * BCOMP_MAXRANGE + 1] = (double)n;

            sln = ptr;

            return n;
        }

        /// <summary>A function to load serialized data into the AidaDataSet</summary>
        /// <returns>Return the XmlReadMode object returned from the dataset's ReadXml() function.</returns>
        /// <param name="DragFunction">Path:  A string containing the fulle or relative path of the file to load.</param>
        public static XmlReadMode LoadAidaDataset(string path)
        {
            return (Aida.myDataset.ReadXml(path));
        }

        /// <summary>A function to write serialized AidaDataSet to disk.</summary>
        /// <returns>Nothing.</returns>
        /// <param name="DragFunction">Path:  A string containing the fulle or relative path of the file to which to save the dataset as xml.</param>
        public static void SerializeAidaDataset(string path)
        {
            Aida.myDataset.WriteXml(path);
        }


    }
}

