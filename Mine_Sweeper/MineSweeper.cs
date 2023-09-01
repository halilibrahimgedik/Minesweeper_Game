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
        private Size field;
        private List<Mine> mineList; // mayınlar
        private readonly Random rnd = new Random();
        private readonly int numberOfFillMine;
        

        public MineSweeper(Size field, int numberOfFillMines)
        {
            numberOfFillMine = numberOfFillMines; // Tarladaki dolu mayın sayısını nesne oluşurken isteyelim
            mineList = new List<Mine>();
            this.field = field;

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
            get { return field; }
        }

        public void AddMine(Mine m)
        {
            mineList.Add(m);
        }

        public Mine GetMineAccordingToLocation(Point loc) 
        {
            foreach (Mine m in mineList)
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
            get { return mineList; }
        }

        public int AllArea
        {
            get
            {
                return 100;
            }
        }

        public int CountOfFillMine
        {
            get
            {
                return numberOfFillMine;
            }
        }

        //  mayınlarımızı bomba ile dolduralım
        private void FillMines()
        {
            int number = 0;
            while (number < numberOfFillMine)
            {
                int i = rnd.Next(0, mineList.Count);
                Mine item = mineList[i];

                if (item.IsThereMine == false)
                {
                    item.IsThereMine = true;
                    number++;
                }
            }
        }
    }
}
