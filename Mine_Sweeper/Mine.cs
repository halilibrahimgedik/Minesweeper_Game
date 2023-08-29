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
        Point MineLocation; //loc

        bool IsFill; //dolu   başlangıçta false olarak ayarlayalım

        bool IsChecked; // oyunu kazanma durumu içinde kullanabiliriz clicked mayın sayısı == tüm mayınlar - dolu mayınlar;

        public bool IsAddedScore=false;

        public bool Flag;

         

        public Mine(Point location)
        {
            IsFill = false;
            MineLocation = location;

        }

        public Point GetLocation
        {
            get { return MineLocation; }

        }

        public bool IsThereMine
        {
            get
            {
                return IsFill;
            }
            set
            {
                IsFill = value;
            }
        }

        public bool MineIsChecked
        {
            get { return IsChecked; }
            set
            {
                IsChecked = value;
            }
        }

    }
}
