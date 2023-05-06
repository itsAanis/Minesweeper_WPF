using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    class Levels
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int Mines { get; set; }



        public void LevelsSwitch(string level)
        {
            switch (level)
            {
                case "Beginner":
                   this.Columns = 9;
                    this.Rows = 9;
                    this.Mines = 10;
                    break;
                case "Intermediate":
                    this.Columns = 16;
                    this.Rows = 16;
                    this.Mines = 40;
                    break;
                case "Advanced":
                    this.Columns = 30;
                    this.Rows = 16;
                    this.Mines = 99;
                    break;
                default:
                    this.Columns = 9;
                    this.Rows = 9;
                    this.Mines = 10;
                    break;
            }
        }

      



    }
}
