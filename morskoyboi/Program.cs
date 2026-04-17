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
    class GameField
    {
        public int Size = 10;
        public Ship[,] grid;
        public List<Ship> ships;

        public GameField() 
        {
            List<Ship> ships = new List<Ship>();
            Ship[,]grid = new Ship[Size, Size];
        }
        public bool AddShip(Ship ship)
        {
            int x = ship.StartX;
            int y = ship.StartY;
            int lenght = ship.Length;
            bool horizontal = ship.IsHorizontal;

            if (horizontal)
            {   
                if (y < 1 || y > Size || x < 1 || x + lenght - 1 > Size)
                {
                    return false;
                }

            }
            else
            {
                if (x < 1 || x > Size || y< 1 || y + lenght - 1 > Size)
                {
                    return false;
                }
            }
            
            for (int i = 0; i < ship.Length; i++)
            {
                if (horizontal)
                {
                    x = ship.StartX + i;
                    y = ship.StartY;
                }
                else
                {
                    x = ship.StartX;
                    y = ship.StartY + i;
                }
                if (grid[x - 1, y - 1] != null)
                {
                    return false ;
                }
            }
            for (int i = 0; i < ship.Length; i++)
            {
                if (horizontal)
                {
                    x = ship.StartX + i;
                    y = ship.StartY;
                }
                else
                {
                    x = ship.StartX;
                    y = ship.StartY + i;
                }
                grid[x - 1, y - 1] = ship;
            }
            ships.Add(ship);
            return true;
        }
        public bool ReceiveShot(int x, int y)
        {
            if (x < 1 || x > Size || y < 1 || y > Size)
            {
                Console.WriteLine("Координаты не в пределах поля!");
                return false;
            }
            Ship ship = grid[x - 1, y - 1];
            if (ship == null)
            {
                Console.WriteLine("Промах!");
                return false;
            }
            bool popal = ship.Hit();
            Console.WriteLine("Попадание!");
            if (popal)
            {
                Console.WriteLine($"Корабль {ship.Type} уничтожен!");
            }
            return true;
        }
    }
}
