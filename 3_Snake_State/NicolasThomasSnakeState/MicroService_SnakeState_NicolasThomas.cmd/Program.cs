using System;
using MicroService_SnakeState_NicolasThomas.Data.Models;
using MicroService_SnakeState_NicolasThomas.Data.Repositories;
namespace MicroService_SnakeState_NicolasThomas.cmd


{
    internal class Program
    {
        static void Main(string[] args)
        {
            ISnakeStateRepository repository = new APISnakeStateRepository("http://localhost:8003/api");
            EnvoieListPosition(repository);

            Console.ReadLine();
        }

        private static void EnvoieListPosition(ISnakeStateRepository repository)
        {
            int x;
            Console.WriteLine("Entrer x : ");
            string input = Console.ReadLine();
            int.TryParse(input, out x);
            int y;
            Console.WriteLine("Entrer y : ");
            input = Console.ReadLine();
            int.TryParse(input, out y);
            var NouveauPoint = (0, 0);
            string direction = "up";
            var Snake = new List <(int x, int y)>();
            bool isEating= false;
            Snake.Add((x, y));
            Snake.Add((x, y+1));
            Console.WriteLine($"Liste : {Snake[0]} , {Snake[1]}");
            switch (direction)
            {
                case "up":
                    y = Snake[0].y - 1;
                    NouveauPoint = (x, y);
                    Snake.Prepend(NouveauPoint);
                    if (isEating == false)
                    {
                        Snake.Remove(Snake[Snake.Count - 1]);
                    }
                    break;
                case "down":
                    y = Snake[0].y + 1;
                    NouveauPoint = (x, y);
                    Snake.Prepend(NouveauPoint);
                    if (isEating == false)
                    {
                        Snake.Remove(Snake[Snake.Count - 1]);
                    }
                    break;
                case "left":
                    x = Snake[0].x - 1;
                    NouveauPoint = (x, y);
                    Snake.Prepend(NouveauPoint);
                    if (isEating == false)
                    {
                        Snake.Remove(Snake[Snake.Count - 1]);
                    }
                    break;
                case "right":
                   x = Snake[0].x + 1;
                    NouveauPoint = (x, y);
                    Snake.Prepend(NouveauPoint);
                    if (isEating == false)
                    {
                        Snake.Remove(Snake[Snake.Count - 1]);
                    }
                    break;
                case "none":
                    x = x + (Snake[0].x - Snake[1].x);
                    y = y + (Snake[0].y - Snake[1].y);
                    NouveauPoint = (x, y);
                    Snake.Prepend(NouveauPoint);
                    if (isEating == false) {
                        Snake.Remove(Snake[Snake.Count-1]);
                    }
                    break;
                default:
                    break;
            }

            var SnakeState = new SnakeStateModel() { Snake=Snake};
            SnakeState = repository.EnvoieListPosition(SnakeState).Result;
            if (SnakeState == null)
            {
                Console.WriteLine("Bug");
            }

            else
            {
                Console.WriteLine($"Liste : {SnakeState.Snake[0]} , {SnakeState.Snake[1]}");
            }

        }
    }
}
