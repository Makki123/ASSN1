using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerGraph
{
    // 3 marks
    private class WebServer
    {
        public string Name;
        public List<WebPage> P;
        //...
    }

    private WebServer[] V;
    private bool[,] E;
    private int NumServers;
    //...

    // 2 marks
    // Create an empty server graph
    public ServerGraph()
    {
        // ask about it if it should be 1 or 0
        V = new WebServer[0];
        E = new bool[0, 0];
        NumServers = 0;
    }

    // 2 marks
    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        for (int i = 0; i < NumServers; i++)
        {
            if (V[i].Name.Equals(name))
            { 
                return i;
            }
        }
        return -1;
    }

    // 3 marks
    // Double the capacity of the server graph with the respect to web servers
    private void DoubleCapacity()
    {  
        int capacity = V.Length * 2;

        WebServer[] oldV = V;
        WebServer[] V = new WebServer[capacity];

        bool[,] oldE = E;
        bool[,] E = new bool[capacity, capacity];

        for (int i = 0; i < oldV.Length; i++)
        {
            V[i] = oldV[i];
        }

        for (int i = 0; i < oldV.Length; i++)
        {
            for (int j = 0; j < oldV.Length; j++)
            {
                E[i,j] = oldE[i,j];
            }
        }
    }

    // 3 marks
    // Add a server with the given name and connect it to the other server
    // Return true if successful; otherwise return false
    public bool AddServer(string name, string other)
    {
        if (NumServers == V.Length)
        {
            DoubleCapacity();
        }

        if (FindServer(other) == -1)
        {
            Console.WriteLine("Other server not found.");
            return false;
        }
        else if (FindServer(name) > -1)
        {
            Console.WriteLine("Server already exists.");
            return false;
        }
        else
        {
            V[NumServers].Name = name;
            for (int i = 0; i <= NumServers; i++)
            {
                E[i, NumServers] = false;
                E[NumServers, i] = false;
            }

            E[NumServers,FindServer(other)] = true;
            E[FindServer(other),NumServers] = true;

            NumServers++;
            return true;

        }
    }

    // 3 marks
    // Add a webpage to the server with the given name
    // Return true if successful; otherwise return false
    public bool AddWebPage(WebPage w, string name)
    {
        if (FindServer(name) == -1)
        {
            Console.WriteLine("Server does not exist");
            return false;
        }
        else
        {
            V[FindServer(name)].P.Add(w);
            return true;
        }
    }

    // 4 marks
    // Remove the server with the given name by assigning its connections
    // and webpages to the other server
    // Return true if successful; otherwise return false
    public bool RemoveServer(string name, string other)

    // 3 marks (Bonus)
    // Remove the webpage from the server with the given name
    // Return true if successful; otherwise return false
    public bool RemoveWebPage(string webpage, string name)

    // 3 marks
    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server
    public bool AddConnection(string from, string to)

    // 10 marks
    // Return all servers that would disconnect the server graph into
    // two or more disjoint graphs if ever one of them would go down
    // Hint: Use a variation of the depth-first search
    public string[] CriticalServers()

    // 6 marks
    // Return the shortest path from one server to another
    // Hint: Use a variation of the breadth-first search
    public int ShortestPath(string from, string to)

    // 4 marks
    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public void PrintGraph()
    
}

// 5 marks
public class WebPage
{
    public string Name { get; set; }
    public string Server { get; set; }
    public List<WebPage> E { get; set; }
    //...
    public WebPage(string name, string host)
    {
        Name = name;
        Server = host;
        E = new List<WebPage>();
    }

    public int FindLink(string name)
    {
        for (int i = 0; i < E.Count; i++)
        {
            if (E[i].Name.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }
}

public class WebGraph
{
    private List<WebPage> P;
    //...

    // 2 marks
    // Create an empty WebGraph
    public WebGraph()
    {
        P = new List<WebPage>();
    }

    // 2 marks
    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {
        for (int i = 0; i < P.Count; i++)
        {
            if (P[i].Name.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }

    // 4 marks
    // Add a webpage with the given name and store it on the host server
    // Return true if successful; otherwise return false
    public bool AddPage(string name, string host, ServerGraph S)
    {
        if (FindPage(name) > -1)
        {
            Console.WriteLine("Page already exists");
            return false;
        }
        else
        {
            WebPage newPage = new WebPage(name, host);
            bool status = S.AddWebPage(newPage, name);
            if (status == true)
            {
                P.Add(newPage);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // 8 marks
    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false
    public bool RemovePage(string name, ServerGraph S)
    {

    }
    // 3 marks
    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
        if (FindPage(from) == -1)
        {
            Console.WriteLine("Webpage doesn't exist");
            return false;
        }
        else if (FindPage(to) == -1)
        {
            Console.WriteLine("Target webpage doesn't exist");
            return false;
        }
        else
        {
            for (int i = 0; i < P[FindPage(from)].E.Count; i++)
            {

            }
        }
    }

    // 3 marks
    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        if (FindPage(from) == -1)
        {
            Console.WriteLine("Start webpage doesn't exist");
            return false;
        }
        else if (FindPage(to) == -1)
        {
            Console.WriteLine("Target webpage doesn't exist");
            return false;
        }
        else
        {
            for (int i = 0; i < P[FindPage(from)].E.Count; i++)
            {
                if (P[FindPage(from)].E[i].Name.Equals(to) == true)
                {
                    P[FindPage(from)].E.RemoveAt(i);
                    return true;
                }
            }
            Console.WriteLine("There is no link to remove");
            return false;
        }
    }

    // 6 marks
    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph
    public float AvgShortestPaths(string name, ServerGraph S)
    {

    }

    // 3 marks
    // Print the name and hyperlinks of each webpage
    public void PrintGraph()
    {
        for (int i = 0; i < )
    }
}

namespace COIS_3020_Assignment_1
{
    class Assignment1
    {
        static void Main(string[] args)
        {

        }
    }
}
