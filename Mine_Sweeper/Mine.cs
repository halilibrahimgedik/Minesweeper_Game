using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mine_Sweeper
{
    public class Mine
    {
        private Point mineLocation;
        private bool isFill; //  başlangıçta false olarak ayarlayalım
        private bool isChecked; // oyunu kazanma durumu içinde kullanabiliriz clicked mayın sayısı == tüm mayınlar - dolu mayınlar;

        public bool IsAddedScore=false;
        public bool Flag;


        public Mine(Point location)
        {
            isFill = false;
            mineLocation = location;

        }

        public Point GetLocation
        {
            get { return mineLocation; }

        }

        public bool IsThereMine
        {
            get
            {
                return isFill;
            }
            set
            {
                isFill = value;
            }
        }

        public bool MineIsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

    }
}
