using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Randomgen
    {

    
        

    public static List<(int,int)> RandomBombs(int numRows, int numCols, int numMines)
    {
            // generate co-ordinates 
            // for x and y 
            // to randomly place bombs
        List<(int, int)> finalMines = new();
        List<(int, int)> tempMines = new();
        Random rMine = new();
        
        bool keepgoing = true;

        while (keepgoing)
        {

            int row = rMine.Next(0, numRows);
            int col = rMine.Next(0, numCols);
            tempMines.Add((row, col));

            finalMines = tempMines.Distinct().ToList();

            if(finalMines.Count == numMines)
            {
                keepgoing = false;
                
            }

        }
        return finalMines;



        

    }



    }

}
