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

    public static void crazyDiamond()
    {
        graphWeight = 0;
        edgeCount = 0;
        verticeCount = 0;

        Dictionary<int, List<int>> tree = new Dictionary<int, List<int>>();

        for (int i = 1; i <= Program.graphSize; i++)
        {
            tree.Add(i, new List<int>());
        }

        foreach (Edge edge in Program.edges)
        {
            if (DORA(tree, edge.from, edge.to) && DORA(tree, edge.to, edge.from))
            {
                tree[edge.from].Add(edge.to);
                tree[edge.to].Add(edge.from);
                edgeCount++;
                graphWeight += edge.weight;
            }
        }

        foreach (int v in tree.Keys)
        {
            if (tree[v].Count > 0)
            {
                verticeCount++;
            }
            DORARA(tree, v);
        }

        StreamWriter writer = new StreamWriter("..\\..\\..\\Graphs\\Answers\\GreatoDaze" + Program.graphSize + ".txt");
        writer.WriteLine("c Вес подграфа = " + graphWeight);
        writer.WriteLine("p edge " + verticeCount + " " + edgeCount);

        foreach (int key in tree.Keys)
        {
            tree[key].Sort();
            foreach (int val in tree[key])
            {
                if (key < val)
                {
                    writer.WriteLine("e " + key + " " + val);
                }
            }
        }

        writer.Close();
    }

    private static bool DORA(Dictionary<int, List<int>> vertices, int from, int to)
    {
        bool check1;
        bool check2;
        bool check3;

        int vertice1;
        int vertice2;

        if (vertices[from].Count < 2)
        {
            return true;
        }

        for (int i = 0; i < vertices[from].Count - 1; i++)
        {
            vertice1 = vertices[from][i];
            for (int j = i + 1; j < vertices[from].Count; j++)
            {
                vertice2 = vertices[from][j];
                check1 = ((vertices[vertice1].Contains(to)) && (vertices[vertice1].Contains(vertice2)));
                check2 = ((vertices[vertice2].Contains(to)) && (vertices[vertice2].Contains(vertice1)));
                check3 = ((vertices[to].Contains(vertice1)) && (vertices[to].Contains(vertice2)));

                if (check1 || check2 || check3)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool DORARA(Dictionary<int, List<int>> vertices, int from)
    {
        bool check1;
        bool check2;
        bool check3;

        int v1;
        int v2;
        int v3;


        if (vertices[from].Count < 3)
        {
            return true;
        }

        for (int i = 0; i < vertices[from].Count - 2; i++)
        {
            v1 = vertices[from][i];
            for (int j = i + 1; j < vertices[from].Count - 1; j++)
            {
                v2 = vertices[from][j];
                for (int k = j + 1; k < vertices[from].Count; k++)
                {
                    v3 = vertices[from][k];
                    check1 = ((vertices[v1].Contains(v3)) && (vertices[v1].Contains(v2)));
                    check2 = ((vertices[v2].Contains(v3)) && (vertices[v2].Contains(v1)));
                    check3 = ((vertices[v3].Contains(v1)) && (vertices[v3].Contains(v2)));

                    if (check1 || check2 || check3)
                    {
                        Console.WriteLine("Проебался дурак: Алмаз " + from + "-" + v1 + "-" + v2 + "-" + v3);
                        return false;
                    }
                }
            }
        }

        return true;
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