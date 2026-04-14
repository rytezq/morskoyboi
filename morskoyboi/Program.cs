using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace morskoyboi
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
    class Ship
    {
        public string Type;
        public int Length;
        public int StartX;
        public int StartY;
        public bool IsHorizontal;
        public int Hits = 0;

        public Ship(string type, int length, int startX, int startY, bool ishorizontal)
        {
            Type = type;
            Length = length;
            StartX = startX;
            StartY = startY;
            IsHorizontal = ishorizontal;
            Hits = 0;
        }

        public bool Hit()
        {
            Hits++;
            if (Length == Hits)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetShipType()
        {
            return Type;
        }
        public void DisplayStatus()
        {
            Console.WriteLine($"{Type}: количество попаданий {Hits} из {Length}.");
        }
    }
    class GameField : Ship
    {
        public static int Size = 10;
        public Ship[,] grid;
        public List<Ship> ships;

        public GameField()
        {
            ships = new List<Ship>();
            grid = new Ship[Size, Size];
        }
        public bool AddShip(Ship ship)
        {
            if (grid[StartX, StartY])
            {

            }
        }
    }
}
