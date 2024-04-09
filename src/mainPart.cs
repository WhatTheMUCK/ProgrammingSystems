//Меркулов М8О-207Б-21 2023

                        //Пример для [1, 4, 2, 4, 3]

                        List<Symbol> myV = new List<Symbol>()
                        {
                            new Symbol("T", new List<Symbol>(){ new Symbol("val") }),
                            new Symbol("F", new List<Symbol>(){ new Symbol("val") }),
                            new Symbol("T'", new List<Symbol>(){ new Symbol("syn"), new Symbol("inh") }),
                            new Symbol("D", new List<Symbol>(){ new Symbol("syn"), new Symbol("inh") })
                        }; //Множество нетерминальных символов
                        List<Symbol> T = new List<Symbol>()
                        {
                            new Symbol("*"),
                            new Symbol("digit", new List<Symbol>(){ new Symbol("lexval")})
                        }; //Множество терминальных символов
                        List<List<Production>> R = new List<List<Production>>(); //Множество правил
                        Symbol S0 = new Symbol("T"); //Начальный символ

                        //Пример для [1, 2, 3]

                        /*List<Symbol> myV = new List<Symbol>()
                        {
                            new Symbol("F", new List<Symbol>(){ new Symbol("val") }),
                            new Symbol("A", new List<Symbol>(){ new Symbol("val") }),
                            new Symbol("B", new List<Symbol>(){ new Symbol("val") }),
                        }; //Множество нетерминальных символов
                        List<Symbol> T = new List<Symbol>() 
                        {
                            new Symbol("+"),
                            new Symbol("digit", new List<Symbol>(){ new Symbol("lexval")})
                        }; //Множество терминальных символов
                        List<List<Production>> R = new List<List<Production>>(); //Множество правил
                        Symbol S0 = new Symbol("F"); //Начальный символ*/

                        AttributeGrammar grammar = new AttributeGrammar(myV, T, R, S0);

                        //Для [1, 4, 2, 4, 3]
                        grammar.AddRule(
                            new Production(new Symbol("T"), new List<Symbol>() { new Symbol("F"), new Symbol("T'") }), // T -> FT'
                            new List<Production>()
                            {
                                new Production(new Symbol("T'.inh"), new List<Symbol>() { new Symbol("F.val") }), // T'.inh <- F.val
                                new Production(new Symbol("T.val"), new List<Symbol>() { new Symbol("T'.syn") }) // T.val <- T'.syn
                            }
                        );
                        grammar.AddRule(
                            new Production(new Symbol("T'"), new List<Symbol>() { new Symbol("*"), new Symbol("F"), new Symbol("D")}), // T' -> *FD
                            new List<Production>()
                            {
                                new Production(new Symbol("D.inh"), new List<Symbol>() { new Symbol("T'.inh"), new Symbol("*"), new Symbol("F.val")}), // D.inh <- T.inh * F.val
                                new Production(new Symbol("T'.syn"), new List<Symbol>() { new Symbol("D.syn")}) // T'.syn <- D.syn 
                            }
                        );
                        grammar.AddRule(
                            new Production(new Symbol("D"), new List<Symbol>(){ new Symbol("ε")}), // D -> ε
                            new List<Production>()
                            {
                                new Production(new Symbol("D.syn"), new List<Symbol>() { new Symbol("D.inh")}) // D.syn <- D.inh
                            }
                        );
                        grammar.AddRule(
                            new Production(new Symbol("F"), new List<Symbol>() { new Symbol("digit")}), // F -> digit
                            new List<Production>()
                            {
                                new Production(new Symbol("F.val"), new List<Symbol>() { new Symbol("digit.lexval")}) // F.val <- digit.lexval
                            }
                        );

                        //Для [1, 2, 3]
                        /*grammar.AddRule(
                            new Production(new Symbol("F"), new List<Symbol>() { new Symbol("A"), new Symbol("+"), new Symbol("B") }), // F -> A+B    
                            new List<Production>()
                            {
                                new Production(new Symbol("F.val"), new List<Symbol>() { new Symbol("A.val"), new Symbol("+"), new Symbol("B.val")}) // F.val <- A.val + B.val
                            }
                        );
                        grammar.AddRule(
                            new Production(new Symbol("A"), new List<Symbol>() { new Symbol("digit") }), // A -> digit
                            new List<Production>()
                            {
                                new Production(new Symbol("A.val"), new List<Symbol>() { new Symbol("digit.lexval")}), // A.val <- digit.lexval
                            }
                        );
                        grammar.AddRule(
                            new Production(new Symbol("B"), new List<Symbol>() { new Symbol("digit") }), // B -> digit
                            new List<Production>()
                            {
                                new Production(new Symbol("B.val"), new List<Symbol>() { new Symbol("digit.lexval")}), // B.val <- digit.lexval
                            }
                        );*/


                        Console.WriteLine(grammar.Print());
                        Console.WriteLine("Введите входную цепочку правил через пробел, например: 1 4 2 4 3");
                        string input = Console.ReadLine();
                        Console.WriteLine();
                        string[] inputs = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        List<int> numRules = new List<int>();
                        for (int i = 0; i < inputs.Length; i++)
                        {
                            numRules.Add(Convert.ToInt32(inputs[i]));
                        }
                        List<Vertex> myTree = new List<Vertex>() { new Vertex(grammar.S) };
                        //Алгоритм построения дерева по слоям в виде упорядоченного множества
                        List<List<int>> boundaries = new List<List<int>>();
                        int lastIndex = 0; //Левая граница поиска
                        //Для каждого правила ищем символ, совпадающий с левой частью правила и поэлементно добавляем соответсвующую правую часть в дерево
                        for (int i = 0; i < numRules.Count; i++)
                        {
                            for (int j = lastIndex; j < myTree.Count; j++)
                            {
                               if (grammar.R[numRules[i] - 1][0].LHS.symbol == myTree[j].Symbol.symbol)
                               {
                                    lastIndex = j + 1;
                                    for (int z = 0; z < grammar.R[numRules[i] - 1][0].RHS.Count; z++)
                                    {
                                        myTree.Add(new Vertex(grammar.R[numRules[i] - 1][0].RHS[z]));
                                        //myTree[j].Add(myTree[myTree.Count - 1]);
                                    }
                                    //Запись границ покрытия каждого правила, нужны для дальнейшего поиска
                                    boundaries.Add(new List<int>() { j, myTree.Count - 1 });
                                    break;
                               }
                            }
                        }
                        //Вывод дерева и ссылок вершин
                        /*for(int i = 0; i < myTree.Count; i++)
                        {
                                Console.Write(myTree[i].Symbol.symbol + " ");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        for(int i = 0; i < myTree.Count; i++)
                        {
                            Console.Write(myTree[i].Symbol.symbol + ": ");
                            foreach(Vertex v in myTree[i].Next)
                            {
                                Console.Write(v.Symbol.symbol + " ");
                            }
                            Console.WriteLine();
                        }*/
                        //Алгоритм преобразования атрибутной грамматики в дерево зависимостей вычислений в виде упорядоченного множества
                        List<Vertex> attributes = new List<Vertex>();
                        List<List<int>> attrLinks = new List<List<int>>();
                        List<int> owners = new List<int>();
                        bool found; //Флаг для нахождения элемента
                        //Добавление всех атрибутов в элементы дерева
                        for (int i = 0; i < myTree.Count; i++)
                        {
                            found = false;
                            for (int j = 0; j < grammar.V.Count; j++)
                            {
                                if (myTree[i].Symbol.symbol == grammar.V[j].symbol)
                                {
                                    found = true;
                                    myTree[i].Symbol.attr = grammar.V[j].attr;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                for (int j = 0; j < grammar.T.Count; j++)
                                {
                                    if (myTree[i].Symbol.symbol == grammar.T[j].symbol)
                                    {
                                        found = true;
                                        myTree[i].Symbol.attr = grammar.T[j].attr;
                                        break;
                                    }
                                }
                            }
                        }
                        Symbol hA; //Вспомогательный массив
                        int temp; //Вспомогательная переменная
                        //Добавление всех атрибутов элементов дерева в виде упорядоченного множества в attributes,
                        //а также параллельное заполнение соответствующих индексов в owners 
                        for (int i = 0; i < myTree.Count; i++)
                        {
                            hA = myTree[i].Symbol;
                            if(hA.attr == null)
                            {
                                hA.attr = new List<Symbol>();
                            }
                            for (int j = 0; j < hA.attr.Count; j++)
                            {
                                owners.Add(i);
                                attributes.Add(new Vertex(new Symbol(hA.symbol + "." + hA.attr[j].symbol)));
                            }
                        }
                        //Установление атрибутных ссылок
                        //Для каждого правила мы проходим по всем соответствующим семантическим правилам
                        //и устанавливаем ссылки в виде пар индексов, в которых правый элемент ссылается на левый элемент пары
                        for (int i = 0; i < numRules.Count; i++)
                        {
                            owners.Reverse();
                            temp = attributes.Count - owners.IndexOf(boundaries[i][1]) - 1;// Самое правое вхождение правой границы покрытия i-го правила
                            owners.Reverse();
                            for (int j = 1; j < grammar.R[numRules[i] - 1].Count; j++)
                            {
                                lastIndex = -1;
                                for (int k = owners.IndexOf(boundaries[i][0]); k < Math.Min(temp + 1, attributes.Count); k++)
                                {
                                    if (grammar.R[numRules[i] - 1][j].LHS.symbol == attributes[k].Symbol.symbol)
                                    {
                                        lastIndex = k;
                                        break;
                                    }
                                }
                                for (int k = 0; k < grammar.R[numRules[i] - 1][j].RHS.Count; k++)
                                {
                                    for (int z = Math.Min(temp, attributes.Count - 1); z >= owners.IndexOf(boundaries[i][0]); z--)
                                    {
                                        if (grammar.R[numRules[i] - 1][j].RHS[k].symbol == attributes[z].Symbol.symbol)
                                        {
                                            attributes[z].Add(new Vertex(attributes[lastIndex].Symbol));
                                            attrLinks.Add(new List<int>() { z, lastIndex });
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //Вывод атрибутов с индексами
                        string result = "Упорядоченное множество атрибутов, между которыми мы определяем зависимости:\n";
                        string[] helpers = new string[2];
                        helpers[0] = "Атрибуты: ";
                        helpers[1] = "Индексы:  ";
                        string helper;
                        for (int i = 0; i < attributes.Count; i++)
                        {
                            helper = attributes[i].Symbol.symbol + " ";
                            lastIndex = helper.Length;
                            helpers[0] += helper;
                            helper = i.ToString();
                            while (helper.Length < lastIndex)
                            {
                                helper += " ";
                            }
                            helpers[1] += helper;
                        }
                        result += helpers[0] + "\n";
                        result += helpers[1] + "\n";
                        Console.WriteLine(result);
                        lastIndex = 0;
                        //Вывод атрибутных ссылок
                        Console.WriteLine("Зависимости между атрибутами:");
                        attrLinks.Sort((emp1, emp2) => emp1[0].CompareTo(emp2[0])); //Сортировка по первому индексу
                        for(int i = 0; i < attributes.Count; i++)
                        {
                            foreach(Vertex v in attributes[i].Next)
                            {
                                Console.WriteLine(attributes[i].Symbol.symbol + " -> " + v.Symbol.symbol + " (" + attrLinks[lastIndex][0].ToString() + " -> " + attrLinks[lastIndex][1].ToString() + ")");
                                lastIndex++;
                            }
                        }
                        //Топологическая сортировка
                        List<List<int>> copyAttrLinks = new List<List<int>>();
                        List<int> order = new List<int>();
                        List<bool> check = new List<bool>();
                        found = false;
                        for (int i = 0; i < attrLinks.Count; i++)
                        {
                            copyAttrLinks.Add(new List<int> { attrLinks[i][0], attrLinks[i][1] });
                        }
                        for (int i = 0; i < attributes.Count; i++)
                        {
                            check.Add(false);
                        }
                        while (copyAttrLinks.Count > 0)
                        {
                            for (int i = 0; i < attributes.Count; i++)
                            {
                                if (check[i])
                                {
                                    continue;
                                }
                                found = false;
                                for (int j = 0; j < copyAttrLinks.Count; j++)
                                {
                                    if (copyAttrLinks[j][1] == i)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    check[i] = true;
                                    order.Add(i);
                                    lastIndex = 0;
                                    while (lastIndex < copyAttrLinks.Count)
                                    {
                                        if (copyAttrLinks[lastIndex][0] == i)
                                        {
                                            copyAttrLinks.RemoveAt(lastIndex);
                                            continue;
                                        }
                                        lastIndex++;
                                    }
                                }
                            }
                        }
                        order.Add(0); //Самым последним всегда будет первый символ
                        //Вывод порядка обхода, полученного при помощи топологической сортировки
                        Console.WriteLine();
                        result = "Порядок обхода дерева зависимостей, полученный с помощью топологической сортировки: ";
                        for (int i = 0; i < order.Count - 1; i++)
                        {
                            result += order[i].ToString() + ", ";
                        }
                        result += order[order.Count - 1];
                        result += "\n";
                        Console.WriteLine(result);
                        
                        //Упорядоченное множество вершин дерева
                        result = "Упорядоченное множество вершин дерева, между которыми мы определяем зависимости:\n";
                        helpers = new string[2];
                        helpers[0] = "Вершины: ";
                        helpers[1] = "Индексы: ";
                        for (int i = 0; i < myTree.Count; i++)
                        {
                            helper = myTree[i].Symbol.symbol + " ";
                            lastIndex = helper.Length;
                            helpers[0] += helper;
                            helper = i.ToString();
                            while (helper.Length < lastIndex)
                            {
                                helper += " ";
                            }
                            helpers[1] += helper;
                        }
                        result += helpers[0] + "\n";
                        result += helpers[1] + "\n";
                        Console.WriteLine(result);
                        //Преобразование ссылок между атрибутами в ссылки между вершинами дерева
                        result = "Зависимости между вершинами дерева:\n";
                        for(int i = 0; i < attrLinks.Count; i++)
                        {
                            result += myTree[owners[attrLinks[i][0]]].Symbol.symbol + " -> " + myTree[owners[attrLinks[i][1]]].Symbol.symbol + " (" + owners[attrLinks[i][0]].ToString() + " -> " + owners[attrLinks[i][1]] + ")\n";
                            myTree[owners[attrLinks[i][0]]].Add(new Vertex(myTree[owners[attrLinks[i][1]]].Symbol.symbol));
                        }
