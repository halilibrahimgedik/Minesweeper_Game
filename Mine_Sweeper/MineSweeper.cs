using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mine_Sweeper
{
    public class MineSweeper
    {
        Size _field;

        List<Mine> MineList; // mayınlar

        int _numberOfFillMine;

        

        Random rnd = new Random();



        public MineSweeper(Size field, int numberOfFillMine)
        {
            _numberOfFillMine = numberOfFillMine; // Tarladaki dolu mayın sayısını nesne oluşurken isteyelim
            MineList = new List<Mine>();
            _field = field;

            for (int x = 0; x < field.Width; x += 30)
            {
                for (int y = 0; y < field.Height; y += 30)
                {
                    Mine mine = new Mine(new Point(x, y));
                    AddMine(mine);
                }
            }
            FillMines();
        }

        public Size SizeOfMineSweeper
        {
            get { return _field; }
        }

        public void AddMine(Mine m)
        {
            MineList.Add(m);
        }

        public Mine GetMineAccordingToLocation(Point loc) //mayin_al_loc
        {
            foreach (Mine m in MineList)
            {
                if (m.GetLocation == loc)
                {
                    return m;
                }
            }
            return null;
        }


        public List<Mine> GetAllMines
        {
            get { return MineList; }
        }

        // bomba ile doldurduk mayınlarımızı
        private void FillMines()
        {
            int number = 0;
            while (number < _numberOfFillMine)
            {
                int i = rnd.Next(0, MineList.Count);
                Mine item = MineList[i];

                if (item.IsThereMine == false)
                {
                    item.IsThereMine = true;
                    number++;
                }
            }
        }


        public int AllArea
        {
            get
            {
                //return /*(_field.Width * _field.Height) / 30*/;
                return 100;
            }
        }

        public int CountOfFillMine
        {
            get
            {
                return _numberOfFillMine;
            }
        }


    }
}
