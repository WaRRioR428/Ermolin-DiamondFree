class Edge : IComparable
{
    public int weight;
    public int from;
    public int to;

    public Edge(int weight, int from, int to)
    {
        this.weight = weight;
        this.from = from;
        this.to = to;
    }

    public int CompareTo(object? o)
    {
        if (o is Edge edge) return weight.CompareTo(edge.weight);
        else throw new ArgumentException("Некорректное значение параметра");
    }
}

class Point
{
    public int a;
    public int b;

    public Point(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public int calculateDistance(Point to)
    {
        return Math.Abs(a - to.a) + Math.Abs(b - to.b);
    }
}

class ShineBrightLikeADiamond
{
    private static int graphWeight;
    private static int edgeCount;
    private static int verticeCount;
    private static bool[,,] c;

    public static void crazyDiamond()
    {
        graphWeight = 0;
        edgeCount = 0;
        verticeCount = 0;


        while (!Program.checkFile()) { }

        foreach (int key in Program.vertices.Keys)
        {
            if (!(Program.vertices[key].Count == 0))
            {
                verticeCount++;
                foreach (int value in Program.vertices[key].Keys)
                {
                    if (key < value)
                    {
                        edgeCount++;
                        graphWeight += Program.points[key - 1].calculateDistance(Program.points[value - 1]);
                    }
                }
            }
        }

        StreamWriter writer = new StreamWriter("..\\..\\..\\Graphs\\Answers\\GreatoDaze" + Program.graphSize + ".txt");
        writer.WriteLine("c Вес подграфа = " + graphWeight);
        writer.WriteLine("p edge " + verticeCount + " " + edgeCount);
        List<int> sortedKeys = new List<int>();
        foreach (int key in Program.vertices.Keys)
        {
            foreach (int value in Program.vertices[key].Keys)
            {
                sortedKeys.Add(value);
            }
            sortedKeys.Sort();
            foreach (int value in sortedKeys)
            {
                if (key < value)
                {
                    writer.WriteLine("e " + key + " " + value);
                }
            }
            sortedKeys.Clear();
        }

        writer.Close();
    }
}

class Program
{
    public static int graphSize;
    public static Point[] points;
    public static Dictionary<int, Dictionary<int, int>> vertices;
    public static List<Edge> edges;

    static void parseFile(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);

        String line = reader.ReadLine();
        graphSize = Int32.Parse(line.Split("=")[1].Trim());
        points = new Point[graphSize];
        vertices = new Dictionary<int, Dictionary<int, int>>();
        edges = new List<Edge>();
        line = reader.ReadLine();

        for (int i = 0; line != null; i++)
        {

            string[] coordinates = line.Split("\t");

            Point point = new Point(Int32.Parse(coordinates[0].Trim()), Int32.Parse(coordinates[1].Trim()));
            points[i] = point;
            vertices.Add(i + 1, new Dictionary<int, int>());

            line = reader.ReadLine();
        }
        reader.Close();

        fillEdgesAndVertices();
    }

    static void fillEdgesAndVertices()
    {
        for (int i = 0; i < graphSize; i++)
        {
            for (int j = 0; j < graphSize; j++)
            {
                if (i == j)
                {
                    continue;
                }

                Edge edge = new Edge(points[i].calculateDistance(points[j]), i + 1, j + 1);

                if (i > j)
                {
                    edges.Add(edge);
                    vertices[edge.from].Add(edge.to, edge.to);
                    vertices[edge.to].Add(edge.from, edge.from);
                }
            }
        }
        edges.Sort();
        edges.Reverse();
    }

    public static bool checkFile()
    {
        bool check1;
        bool check2;
        bool check3;

        bool result = true;

        int v1;
        int v2;
        int v3;

        int value;

        foreach (int vertice in vertices.Keys)
        {
            int[] keys = vertices[vertice].Keys.ToArray();

            if (keys.Length < 3)
            {
                continue;
            }

            for (int i = 0; i < keys.Length - 2; i++)
            {
                if (keys[i] == 0) continue;
                v1 = keys[i];
                int d1 = points[vertice - 1].calculateDistance(points[v1 - 1]);
                for (int j = i + 1; j < keys.Length - 1; j++)
                {
                    if (keys[j] == 0) continue;
                    v2 = keys[j];
                    int d2 = points[vertice - 1].calculateDistance(points[v2 - 1]);
                    for (int k = j + 1; k < keys.Length; k++)
                    {
                        if (keys[k] == 0) continue;
                        v3 = keys[k];

                        List<Edge> temp = new List<Edge>();
                        temp.Add(new Edge(d1, vertice, v1));
                        temp.Add(new Edge(d2, vertice, v2));
                        temp.Add(new Edge(points[vertice - 1].calculateDistance(points[v3 - 1]), vertice, v3));

                        check1 = ((vertices[v1].TryGetValue(v3, out value)) && (vertices[v1].TryGetValue(v2, out value)));
                        check2 = ((vertices[v2].TryGetValue(v3, out value)) && (vertices[v2].TryGetValue(v1, out value)));
                        check3 = ((vertices[v3].TryGetValue(v1, out value)) && (vertices[v3].TryGetValue(v2, out value)));

                        if (check1 || check2 || check3)
                        {
                            result = false;

                            if (check1)
                            {
                                temp.Add(new Edge(points[v1 - 1].calculateDistance(points[v3 - 1]), v1, v3));
                                temp.Add(new Edge(points[v1 - 1].calculateDistance(points[v2 - 1]), v1, v2));
                            }
                            else if (check2)
                            {
                                temp.Add(new Edge(points[v2 - 1].calculateDistance(points[v3 - 1]), v2, v3));
                                temp.Add(new Edge(points[v2 - 1].calculateDistance(points[v1 - 1]), v2, v1));
                            }
                            else
                            {
                                temp.Add(new Edge(points[v3 - 1].calculateDistance(points[v1 - 1]), v3, v1));
                                temp.Add(new Edge(points[v3 - 1].calculateDistance(points[v2 - 1]), v3, v2));
                            }
                            temp.Sort();
                            Edge e = temp.First();


                            if (e.from == vertice)
                            {
                                if (e.to == v1)
                                {
                                    keys[i] = 0;
                                }
                                else if (e.to == v2)
                                {
                                    keys[j] = 0;
                                }
                                else
                                {
                                    keys[k] = 0;
                                }
                            }
                            vertices[e.from].Remove(e.to);
                            vertices[e.to].Remove(e.from);
                        }
                    }
                }
            }
        }

        return result;
    }

    static void Main(string[] args)
    {
        int[] fileSizes = { 64, 128, 512, 2048, 4096 };
        foreach (int size in fileSizes)
        {
            parseFile("..\\..\\..\\Graphs\\Benchmark\\Taxicab_" + size + ".txt");
            Console.WriteLine(size + " Start");
            ShineBrightLikeADiamond.crazyDiamond();
            vertices.Clear();
            edges.Clear();
        }
    }
}
