using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class Network
{
    private int numElements;
    private Dictionary<int, HashSet<int>> connections;

    public Network(int numElements)
    {
        if (numElements <= 0)
            throw new ArgumentException("O número de elementos deve ser positivo.");

        this.numElements = numElements;
        connections = new Dictionary<int, HashSet<int>>();

        for (int i = 1; i <= numElements; i++)
        {
            connections[i] = new HashSet<int>();
        }
    }

    public void Connect(int a, int b) 
    {
        validateElement(a, b);
        connections[a].Add(b);
        connections[b].Add(a);
    }

    public void Disconnect(int a, int b) 
    {
        validateElement(a, b);
        connections[a].Remove(b);
        connections[b].Remove(a);
    }

    public bool Query(int a, int b)
    {
        validateElement(a, b);
        return AreConnected(a, b, new HashSet<int>());
    }

    public int LevelConnection(int a, int b)
    {
        validateElement(a, b);
        if (a == b) return 0;

        Queue<(int node, int level)> queue = new Queue<(int, int)>();
        HashSet<int> visited = new HashSet<int>();
        queue.Enqueue((a, 0));
        visited.Add(a);

        while (queue.Count > 0)
        {
            var (current, level) = queue.Dequeue();

            if (current == b)
                return level;

            foreach (var neighbor in connections[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, level + 1));
                }
            }
        }

        return 0;
    }

    private void validateElement(int elementA, int elementB)
    {
        if (elementA < 1 || elementA > numElements || elementB < 1 || elementB > numElements)
            throw new ArgumentException("Elementos inválidos.");
    }

    private bool AreConnected(int elementA, int elementB, HashSet<int> visited)
    {
        if (elementA == elementB) return true;

        visited.Add(elementA);

        foreach (var neighbor in connections[elementA])
        {
            if (!visited.Contains(neighbor))
            {
                if (AreConnected(neighbor, elementB, visited))
                    return true;
            }
        }

        return false;
    }

    public static void Main()
    {
        Network network = new Network(8);

        network.Connect(1, 2);
        network.Connect(6, 2);
        network.Connect(2, 4);
        network.Connect(5, 8);

        Console.WriteLine("1 e 6 conectados? " + network.Query(1, 6)); // True
        Console.WriteLine("6 e 4 conectados? " + network.Query(6, 4)); // True
        Console.WriteLine("7 e 4 conectados? " + network.Query(7, 4)); // False
        Console.WriteLine("5 e 6 conectados? " + network.Query(5, 6)); // False

        Console.WriteLine("Nível de conexão 1-6: " + network.LevelConnection(1, 6)); // 2
        Console.WriteLine("Nível de conexão 6-4: " + network.LevelConnection(6, 4)); // 2
        Console.WriteLine("Nível de conexão 7-4: " + network.LevelConnection(7, 4)); // 0
        Console.WriteLine("Nível de conexão 5-8: " + network.LevelConnection(5, 8)); // 1
    }
}
