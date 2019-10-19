using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Graphs
{
    class OrGraph
    {
        

        Dictionary<object, Dictionary<object, int>> Nodes; //структура для хранения списка смежности
        Dictionary<object, bool> check;//словарь просмотров вершин
        Dictionary<object, Dictionary<object, int>> NodesTwo; //структура для хранения списка смежности
        public Dictionary<object, Dictionary<object, int>> getNodes
        {
            get
            {
                return Nodes;
            }
        }
     
        public OrGraph()   ///конструктор по умолчанию ,без параметров, создающий пустой граф
        {
            Nodes = new Dictionary<object, Dictionary<object, int>>();
           
            check = new Dictionary<object, bool>();

        }

        public OrGraph(OrGraph ToCopyGraph)   //конструктор-копия
        {
            Nodes = new Dictionary<object, Dictionary<object, int>>(ToCopyGraph.Nodes);
           
            check = new Dictionary<object, bool>(ToCopyGraph.check);
        }

        public OrGraph(string InputFileString)     //конструктор, заполняющий данные графа из файла
        {
            Nodes = new Dictionary<object, Dictionary<object, int>>();
           
            using (StreamReader inputfile = new StreamReader(InputFileString))
            {
                string kind = inputfile.ReadLine(); //какой это граф 
                while (!inputfile.EndOfStream) //пока вершины не закончились
                {
                    string name = inputfile.ReadLine().Remove(0, 1);  
                    name = name.Remove(name.Length - 1, 1);  //убрали границы '||'
                    Nodes.Add(name, new Dictionary<object, int>());  //мы теперь знаем, что вершина есть, с кем смежна пока не знаем
                    while (!inputfile.EndOfStream && inputfile.Peek() != '|') //пока мы не дошли до конца или до новой вершины
                    {
                        
                        if (kind == "1")
                        {
                            string[] thing = inputfile.ReadLine().Split(' ');
                            Nodes[name].Add(thing[0], int.Parse(thing[1]));    //добавляем пару в список смежности
                        }
                        else
                        {
                            Nodes[name].Add(inputfile.ReadLine(), 1);

                        }
                    }
                }
            }

            check = new Dictionary<object, bool>(); 
            foreach (var item in  Nodes)
            {
                check.Add(item.Key, false);
            }
        }
        public void AddNode(object Name, object[] Neighbors)   //добавить вершину для невзвешенного
        {
            bool b = true; 
            if (Nodes.ContainsKey(Name))
            {
                Console.WriteLine("Такая вершина уже существует!");
            }
            else
            {
                foreach(object item in Neighbors)
                {
                    if (!Nodes.ContainsKey(item))
                    {
                        b = false;
                        break;
                    }
                }
                if (b)
                {
                    Dictionary<object, int> temp = new Dictionary<object, int>();
                    for (int i = 0; i < Neighbors.Length; i++)    //пока не закончились соседи
                    {
                        temp.Add(Neighbors[i], 1);   //добавляем соседей соответственно
                    }
                    Nodes.Add(Name, temp);   //добавляем вершину в список
                } else
                {
                    Console.WriteLine("Присутствуют не все соседи!");
                }
            }
        }
        public void AddNode(object Name, object[] Neighbors, int[] Weights)  //добавить вершину для взвешенного
        {
            bool b = true;
            if (Nodes.ContainsKey(Name))
            {
                Console.WriteLine("Такая вершина уже существует!");
            }
            else
            {
                foreach (object item in Neighbors)
                {
                    if (!Nodes.ContainsKey(item))
                    {
                        b = false;
                        break;
                    }
                }
                if (b)
                {
                    Dictionary<object, int> temp = new Dictionary<object, int>();
                    for (int i = 0; i < Neighbors.Length; i++)    //пока не закончились соседи
                    {
                        temp.Add(Neighbors[i], Weights[i]);   //добавляем пары "cосед - вес" соответственно
                    }
                    Nodes.Add(Name, temp);   //добавляем вершину в список
                } else
                {
                    Console.WriteLine("Присутствуют не все соседи!");
                }
            }
        }
        public void Add(object FromNode, object ToNode)  //добавить  дугу для невзвешенного 
        {
            if (Nodes.ContainsKey(FromNode) & Nodes.ContainsKey(ToNode) )
            {
                if (!Nodes[FromNode].ContainsKey(ToNode))
                {
                    Nodes[FromNode].Add(ToNode, 1);
                }
                else
                {
                    Console.WriteLine("В графе уже есть такая дуга");
                }
            } else
            {
                Console.WriteLine("Нет одной из составляющей дуги");
            }
        }

        public void AddWeight(object FromNode, object NodeTo, int Weight) //добавить дугу для взвешенного
        {
            if (Nodes.ContainsKey(FromNode) & Nodes.ContainsKey(NodeTo))
            {
                if (!Nodes[FromNode].ContainsKey(NodeTo))
                {

                    Nodes[FromNode].Add(NodeTo, Weight);
                } else
                {
                    Console.WriteLine("такая дуга уже есть!");
                }
            } else
            {
                Console.WriteLine("Не уедем или не приедем!");

            }
        }
        public void RemoveNode(object Name)    //удалить вершину
        {
            if (Nodes.ContainsKey(Name))
            {
                foreach (var item in Nodes)
                {
                    if (Nodes[item.Key].ContainsKey(Name))
                    {
                        Remove(item.Key ,Name);
                    }
                    if (Nodes[Name].ContainsKey(item.Key))
                    {
                        Remove(Name, item.Key);
                    }
                }
               

                Nodes.Remove(Name);
            } else
            {
                Console.WriteLine("Такой вершины не существует!");
            }
        }

        public void Remove(object FromNode, object NodeTo)   //удалить дугу(для ориентированного)
        {
            if (Nodes.ContainsKey(FromNode) & Nodes.ContainsKey(NodeTo))
            {
                if (Nodes[FromNode].ContainsKey(NodeTo))
                {
                    Nodes[FromNode].Remove(NodeTo);
                }
                else
                {
                    Console.WriteLine("Такой дуги не существует!");
                }
            }
            else
            {
                Console.WriteLine("Какой-то вершины не существует!");
            }
        }
        

        public void PrintToFile(string OutputFileString)
        {
            using (StreamWriter outputfile = new StreamWriter(OutputFileString))
            {
                foreach (var item in Nodes)
                {
                    outputfile.WriteLine("|" + item.Key.ToString() + "|");
                    Console.WriteLine("|" + item.Key.ToString() + "|");
                    foreach (var item2 in item.Value)
                    {
                        outputfile.WriteLine(item2.Key.ToString() );
                        Console.WriteLine(item2.Key.ToString());
                    }
                }
            }
        }
        public void PrintToFileWeight(string OutputFileString)
        {
            using (StreamWriter outputfile = new StreamWriter(OutputFileString))
            {
                foreach (var item in Nodes)
                {
                    outputfile.WriteLine("|" + item.Key.ToString() + "|");
                    Console.WriteLine("|" + item.Key.ToString() + "|");
                    foreach (var item2 in item.Value)
                    {
                        outputfile.WriteLine(item2.Key.ToString() + " " + item2.Value.ToString());
                        Console.WriteLine(item2.Key.ToString() + " " + item2.Value.ToString());
                    }
                }
            }
        }
        public void PrintPower(object v)
       {
           if (Nodes.Keys.Contains(v))
           {
               foreach (var item in Nodes)
               {

                   if (Equals(item.Key, v))
                   {
                       Console.WriteLine(item.Value.Count);
                   }

               }
           } else
           {
               Console.WriteLine("Такой вершины не существует");
           }

       }
       public void PrintPetli()
       {
           bool k = false;
           foreach(var item in Nodes)
           {
               if (item.Value.ContainsKey(item.Key))
               {
                   Console.WriteLine(item.Key);
                   k = true;
               }
           }
           if (!k)
           {
               Console.WriteLine("Петель нет");
           }
       }
        public bool Dfs(object v, bool IsTrue)
        {
            IsTrue = false;
            foreach (var item in Nodes)
            {
                if (item.Value.ContainsKey(v))
                {
                    if (!check[item.Key])
                    {
                        check[item.Key] = true;
                      
                        Dfs(item.Key, IsTrue);
                    } else
                    {
                        IsTrue = true;
                        return IsTrue; 
                    }
                }
            }
            return IsTrue;
        }

          
    
    }
}
