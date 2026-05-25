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
            GameField gameField = new GameField();
            int shotscount = 0;
            Ship ship1 = new Ship("Линкор", 4, 1, 1, true);
            Ship ship2 = new Ship("Крейсер", 3, 2, 3, false);
            Ship ship3 = new Ship("Эсминец", 2, 5, 5, true);
            Ship ship4 = new Ship("Эсминец", 2, 7, 8, false);

            gameField.AddShip(ship1);
            gameField.AddShip(ship2);
            gameField.AddShip(ship3);
            gameField.AddShip(ship4);

            Console.WriteLine("Начальное поле:");
            gameField.PrintField(false);

            while (!gameField.AllShipsDestroyed())
            {
                Console.Clear();
                Console.WriteLine($">>>>> МОРСКОЙ БОЙ <<<<<");
                Console.WriteLine($"------------------------");
                Console.WriteLine($"Количество выстрелов: {shotscount}");
                Console.WriteLine();

                gameField.PrintField(true);

                Console.WriteLine($"\n------------------------");
                Console.WriteLine($"Введите координаты (x y)");
                string input = Console.ReadLine();

                string[] p = input.Split(' ');
                if (p.Length != 2)
                {
                    Console.WriteLine($"Ошибка: нужно ввести два числа через пробел");
                    Console.ReadKey();
                    continue;
                }
                if (!int.TryParse(p[0], out int x) || !int.TryParse(p[1], out int y))
                {
                    Console.WriteLine($"Ошибка: введите целые числа");
                    Console.ReadKey();
                    continue;
                }

                shotscount++;
                bool hit = gameField.ReceiveShot(x, y);

                Console.WriteLine($"Нажмите любую клавишу для продолжения");
                Console.ReadKey();
            }

            Console.Clear();
            Console.WriteLine($"Поздравляем вы победили");
            Console.WriteLine($"Вы уничтожили все корабли за {shotscount} выстрелов");
            Console.ReadKey();
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
        public bool Destroyed()
        {
            return Hits == Length;
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
        public bool[,] shots;

        public GameField() 
        {
            ships = new List<Ship>();
            grid = new Ship[Size, Size];
            shots = new bool[Size, Size];
        }
        public bool AddShip(Ship ship)
        {
            int x = ship.StartX;
            int y = ship.StartY;
            int lenght = ship.Length;
            bool horiz = ship.IsHorizontal;
            if (horiz)
            {
                if (y < 1 || y > Size || x < 1 || x + lenght - 1 > Size || x > Size)
                {
                    return false;
                }
            }
            else
            {
                if (x < 1 || x > Size || y < 1 || y + lenght - 1 > Size || y > Size)
                {
                    return false;
                }
            }
            for (int i = 0; i < lenght; i++)
            {
                if (horiz)
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
                    return false;
                }
            }
            for (int i = 0; i < lenght; i++)
            {
                if (horiz)
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
            shots[x - 1, y - 1] = true;
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
        public bool AllShipsDestroyed()
        {
            foreach (Ship ship in ships)
            {
                if (ship.Hits != ship.Length)
                {
                    return false;
                }
            }
            return true;
        }
        public void PrintField(bool hideShips)
        {
            Console.Write($"   ");
            for (int i = 1; i <= Size; i++)
            {
                Console.Write($"{i,3} ");
            }
            Console.WriteLine();
            Console.Write($"    ");
            for (int i = 1; i <= Size; i++)
            {
                Console.Write($">><<");
            }
            Console.WriteLine();
            for (int j = 1; j <= Size; j++)
            {
                Console.Write($"{j,2} |");
                for (int i = 1; i <= Size; i++)
                {
                    Ship ship = grid[i - 1, j - 1];
                    bool shot = shots[i - 1, j - 1];
                    char s = ' ';
                    if (shot)
                    {
                        if (ship == null)
                        {
                            s = 'o';
                        }
                        else if (ship.Destroyed())
                        {
                            s = 'X';
                        }
                        else
                        {
                            s = '#';
                        }
                    }
                    else
                    {
                        if (!hideShips && ship != null)
                        {
                            s = 'S';
                        }
                        else
                        {
                            s = '~';
                        }
                    }
                    Console.Write($" {s}  ");
                }
                Console.WriteLine($"|");
            }
            Console.Write($"    ");
            for (int i = 1; i <= Size; i++)
            {
                Console.Write($">><<");
            }
            Console.WriteLine();
        }   
        
    }
}
