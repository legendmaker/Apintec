using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Robots
{
    public class Coordinate
    {
      
        private double _x;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        private double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        private double _z;

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        private double _r;

        public double R
        {
            get { return _r; }
            set { _r = value; }
        }
        public Coordinate()
        {

        }
        public Coordinate(double x,double y,double z,double r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
        }

        public Coordinate Round(int bitNum)
        {
            Coordinate coord = new Coordinate();
            coord.X = Double.Parse(X.ToString("F" + bitNum.ToString()));
            coord.Y = Double.Parse(Y.ToString("F" + bitNum.ToString()));
            coord.Z = Double.Parse(Z.ToString("F" + bitNum.ToString()));
            coord.R = Double.Parse(R.ToString("F" + bitNum.ToString()));
            return coord;
        }
    }
}
