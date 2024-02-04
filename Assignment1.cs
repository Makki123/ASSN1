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
    public bool RemoveServer(string name, string other){
        
            int i, j;
            if ((j = FindServer(other)) > -1)
            {

                if ((i = FindServer(name)) > -1)
                {
                    NumServers--;
                    V[i] = V[NumServers];
                    E[j, i] = E[j, NumServers];
                    E[i, j] = E[NumServers, j];
                    return true;
                }
            }
            
            return false;
            
    }

    // 3 marks (Bonus)
    // Remove the webpage from the server with the given name
    // Return true if successful; otherwise return false
    public bool RemoveWebPage(string webpage, string name);
    // 3 marks
    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server
    public bool AddConnection(string from, string to)
    {
        int i, j;
            if ((i = FindServer(from)) > -1 && (j = FindServer(to)) > -1)
            {
                if (E[i, j] == false)
                {
                    E[i, j] = true;
                    return true;
                }
                
            }
            return false;
    }
    public bool RemoveCrticalServer(string name)
    {
        
            int i;
            
                if ((i = FindServer(name)) > -1)
                {
                    NumServers--;
                    V[i] = V[NumServers];
            
                for (int j = NumServers; j >= 0; j--)
                {
                    E[j, i] = E[j, NumServers];
                    E[i, j] = E[NumServers, j];
                }
                    return true;
                }
            
            return false;
            
    }
    public List<string> CriticalServersHelper(string servername)
    {
        List<string> serverconnections = new List<string>();

        int indexserver = FindServer(servername);
        //iterate throught vertices
        for (int i = 0; i < NumServers; i++)
        {
           if(E[indexserver,i] == true)
           {
             serverconnections.Add(V[i].Name);
            
           }
        }
        return serverconnections;

    }
    // 10 marks
    // Return all servers that would disconnect the server graph into
    // two or more disjoint graphs if ever one of them would go down
    // Hint: Use a variation of the depth-first search
    public string[] CriticalServers()
    {
    List<string> criticalServersList = new List<string>(); //correct

    for (int i = 0; i < NumServers; i++)
    {
        // Temporarily remove the i-th server
        string removedServer = V[i].Name;

        List<string> serverconnections = CriticalServersHelper(removedServer);

        RemoveCrticalServer(removedServer); // not keeping track of the edges
        // Perform DFS to check connectivity
        bool[] visited = new bool[NumServers];

        for (int j = 0; j < NumServers; j++)
        {
            if (!visited[j])
            {
                DFS(j, visited);

            }
        }
        for (int x = 0; x < visited.Length; x++)
        {
            if (visited[x] == false)
            {
                criticalServersList.Add(V[i].Name);
            }
        }
        for (int u = 0; u <serverconnections.Count; u++)
        {
        
        // Add the server back to the graph
        bool flag = AddServer(removedServer, serverconnections[u]);
        if (!flag)
        {
            AddConnection(removedServer,serverconnections[u]);
        }
        }
        

    }

    return criticalServersList.ToArray();
}

private void DFS(int v, bool[] visited)
{
    visited[v] = true;

    for (int i = 0; i < NumServers; i++)
    {
        if (E[v, i] && !visited[i])
        {
            DFS(i, visited);
        }
    }
}


    // 6 marks
    // Return the shortest path from one server to another
    // Hint: Use a variation of the breadth-first search
    public List<string> ShortestPath(string from, string to)
    {
    bool[] visited = new bool[NumServers];
    int[] parent = new int[NumServers];
    Queue<int> queue = new Queue<int>();

    int start = FindServer(from);
    int end = FindServer(to);

    if (start == -1 || end == -1)
    {
        Console.WriteLine("One or both servers not found");
        return new List<string>();
    }

    queue.Enqueue(start);
    visited[start] = true;

    while (queue.Count != 0)
    {
        int currentVertex = queue.Dequeue();

        for (int neighbor = 0; neighbor < NumServers; neighbor++)
        {
            if (!visited[neighbor] && E[currentVertex, neighbor])
            {
                queue.Enqueue(neighbor);
                visited[neighbor] = true;
                parent[neighbor] = currentVertex;
            }
        }
    }

    List<string> path = new List<string>();

    // Reconstruct the path from 'to' to 'from'
    for (int currentVertex = end; currentVertex != start; currentVertex = parent[currentVertex])
    {
        path.Add(V[currentVertex].Name);
    }

    // Add the starting server
    path.Add(from);

    // Reverse the path to get it from 'from' to 'to'
    path.Reverse();

    return path;
}



    // 4 marks
    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public void PrintGraph(){

        for (int i = 0; i < P.Count; i++)
        {
            Console.WriteLine("Website: {0}", P[i].Name);
            Console.WriteLine("Links");
            
            for (int j = 0; j < P[i].E.Count; j++)
            {
                Console.WriteLine(P[i].E[j].Name);
            }
            Console.WriteLine("");
        }

    }
    
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
        if (FindPage(name) == -1)
        {
            Console.WriteLine("WebPage doesn't exist");
            return false;
        }
        else
        {   
            for (int i = 0; i < P.Count; i++)
            {
                for (int j = 0; j < P[i].E.Count; j++)
                {
                    if (P[i].E[j].Name.Equals(name))
                    {
                        RemoveLink(P[i].Name,name);
                    }
                }
            }

            // Removes the webpage from the server
            S.RemoveWebPage(name, P[FindPage(name)].Server);

            // Removes the webpage from the web graph
            P.RemoveAt(FindPage(name));
            return true;
        }
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
