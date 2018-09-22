using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSystem.Model
{
    public class DragMeasure
    {
        public DragMeasure()
        {
        }


        public DragMeasure(double startx,double starty,double endx,double endy)
        {
            StartX = startx;
            StartY = starty;
            EndX = endx;
            EndY = endy;
        }


        private double _StartX;

        public double StartX
        {
            get { return _StartX; }
            set { _StartX = value; }
        }

        private double _StartY;

        public double StartY
        {
            get { return _StartY; }
            set { _StartY = value; }
        }

        private double _EndX;

        public double EndX
        {
            get { return _EndX; }
            set { _EndX = value; }
        }

        private double _EndY;

        public double EndY
        {
            get { return _EndY; }
            set { _EndY = value; }
        }

        public double DistanceX
        {
            get
            {
                return EndX - StartX;
            }
        }

        public double DistanceY
        {
            get
            {
                return EndY - StartY;
            }
        }

    }
}
