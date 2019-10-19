using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{

    class Program
    {

        static void Main(string[] args)
        {
            OrGraph a = new OrGraph("input.txt");
            Console.WriteLine("Определите граф(через пробел):");
            Console.WriteLine("0 - неориентированный, 1 - ориентированный ");
            Console.WriteLine("0 - невзвешенный, 1 - взвешенный ");
            string[] kind = Console.ReadLine().Split(' ');
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - Проверить граф на ацикличность");
                Console.WriteLine("2 - Определить центр графа");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            bool b = false; 
                            foreach (var item in a.getNodes)
                            {
                                if (a.Dfs(item.Key,b))
                                {
                                    b = true; 
                                    Console.WriteLine("Заданный орграф не является ацикличным, т.е. имеет циклы");
                                    break; 
                                }
                            }
                            if (!b)
                            {
                                Console.WriteLine("Заданный орграф является ацикличным");
                            }

                        }
                        break;
                }
                Console.ReadLine();
            }
        }
    }
}