using System;
using System.IO;


namespace Jenky.IO
{
    public class MapReader
    {

        //////////////////////////////
        // TO BE REPLACED WITH JSON //
        //////////////////////////////




        //Reads in a 2d array from a comma-separated value file and returns it
        public int[,] LoadMap(int columns, int rows, string mapPath)
        {
            int[,] terrain = new int[rows, columns];

            try
            {
                //File stream to read our file
                StreamReader sr = new StreamReader(mapPath);

                int currentRow = 0;
                string line;

                //While the reader has not hit the end of file
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitString = line.Split(',');

                    //For each columnin the current row, read the value into the 2d array
                    for (int i = 0; i < columns; i++)
                    {
                        terrain[currentRow, i] = Convert.ToInt32(splitString[i]);
                    }
                    
                    currentRow++;
                }

                //Close the file stream
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return terrain;
        }
    }
}
