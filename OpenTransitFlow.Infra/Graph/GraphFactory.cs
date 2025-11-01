using QuikGraph;
using QuikGraph.Algorithms;
using System.Numerics;

namespace OpenTransitFlow.Infra.Graph
{
    public class GraphFactory
    {
        /// <summary>
        /// Stores the graph.
        /// </summary>
        private BidirectionalGraph<INetworkGraphVertex, NetworkGraphEdge> _graph;
        public BidirectionalGraph<INetworkGraphVertex, NetworkGraphEdge> GetGraph() => _graph;
        /// <summary>
        /// Stores edges for quicker lookup.
        /// </summary>
        private Dictionary<string, NetworkGraphEdge> _edges;
        /// <summary>
        /// Stores the nodes for quicker lookup.
        /// </summary>
        private Dictionary<string, NetworkGraphVertex> _nodes;

        /// <summary>
        /// Creates empty BiderectionalGraph instances for GraphFactory to work.
        /// Provide False, if no parallel edges should be allowed. Default is true.
        /// </summary>
        public GraphFactory(bool allowParallelEdges = true)
        {
            _graph = new BidirectionalGraph<INetworkGraphVertex, NetworkGraphEdge>(allowParallelEdges);
            _edges = new Dictionary<string, NetworkGraphEdge>();
            _nodes = new Dictionary<string, NetworkGraphVertex>();
        }

        /// <summary>
        /// Add a new Node to the network.
        /// </summary>
        public void AddNode(string uuid, Vector2 pos)
        {
            if (_nodes.ContainsKey(uuid))
            {
#if DEBUG
                Console.WriteLine($"Node with {uuid} already exists");
#endif
                return;
            }
            NetworkGraphVertex newNode = new NetworkGraphVertex(uuid, pos);
            _nodes.Add(newNode.uuid, newNode);
            _graph.AddVertex(newNode);
        }

        /// <summary>
        /// <inheritdoc cref="AddNode"/>
        /// </summary>
        public void AddNode(string uuid, float[] pos)
        {
            AddNode(uuid, new Vector2(pos[0], pos[1]));
        }

        /// <summary>
        /// Creates a new Edge between two already existing nodes.
        /// </summary>
        public void AddEdge(string uuid, string sourceId, string targetId)
        {
            NetworkGraphVertex source, target;
            if (!_nodes.TryGetValue(sourceId, out source) || !_nodes.TryGetValue(targetId, out target) || _edges.ContainsKey(uuid))
            {
#if DEBUG
                if (!_nodes.ContainsKey(sourceId)) Console.WriteLine($"Node with uuid (source) {sourceId} does not exists");
                if (!_nodes.ContainsKey(targetId)) Console.WriteLine($"Node with uuid (target) {targetId} does not exists");
                if (_edges.ContainsKey(uuid)) Console.WriteLine($"Edge with uuid {uuid} already exists");
#endif
                return;
            }

            NetworkGraphEdge newEdge = new NetworkGraphEdge(source, target, uuid);
            _edges.Add(newEdge.UUID, newEdge);
            _graph.AddEdge(newEdge);
        }

        /// <summary>
        /// <inheritdoc cref="AddEdge"/> Can add undirected Edges.
        /// </summary>
        public void AddEdge(string uuid, string sourceId, string targetId, bool makeEdgeUndirected)
        {
            if (makeEdgeUndirected)
            {
                AddEdge(uuid+"I", sourceId, targetId);
                AddEdge(uuid + "II", targetId, sourceId);
            }
        }

        /// <summary>
        /// Method to add a new node to an existing edge.
        /// </summary>
        public void SplitEdge(NetworkGraphEdge edge)
        {
            if (!_edges.ContainsKey(edge.UUID)) return;
            var intermediateNode = new NetworkGraphVertex((_nodes.Count + 1).ToString(), [0, 0]);
            SplitEdge(edge, intermediateNode);
        }
        /// <summary>
        /// <inheritdoc cref="SplitEdge"/> Takes predefined node.
        /// </summary>
        public void SplitEdge(NetworkGraphEdge edge, NetworkGraphVertex node)
        {
            var intermediateNode = node;
            var source = ((IEdge<NetworkGraphVertex>)edge).Source;
            var target = ((IEdge<NetworkGraphVertex>)edge).Target;
            _edges.Remove(edge.UUID);
            _graph.RemoveEdge(edge);

            var firstHalfEdge = new NetworkGraphEdge(source, intermediateNode, source.uuid + "->" + intermediateNode.uuid);
            var secondHalfEdge = new NetworkGraphEdge(intermediateNode, target, intermediateNode.uuid + "->" + target.uuid);

            _nodes.Add(intermediateNode.uuid, intermediateNode);
            _edges.Add(firstHalfEdge.UUID, firstHalfEdge);
            _edges.Add(secondHalfEdge.UUID, secondHalfEdge);

            _graph.AddVertex(intermediateNode);
            _graph.AddEdge(firstHalfEdge);
            _graph.AddEdge(secondHalfEdge);
        }

        public void RunDijkstra(NetworkGraphVertex start, NetworkGraphVertex end)
        {
            var graph = this._graph;
            Func<NetworkGraphEdge, double> edgeCost = edge => edge.weight;

            TryFunc<NetworkGraphVertex, IEnumerable<NetworkGraphEdge>> tryGetPaths = graph.ShortestPathsDijkstra(edgeCost, start);

            // Query path for given vertices
            if (tryGetPaths(end, out IEnumerable<NetworkGraphEdge> path))
            {
                foreach (NetworkGraphEdge edge in path)
                {
                    Console.WriteLine(edge);
                    Console.WriteLine(edgeCost(edge));
                }
            }
        }

        /* 
            Need for new data structure... :-(
            Graph needs to look as follows:
            - All tracks are edges, this allows for an clear saving of distances as weights
            - Switches and trackEnds are nodes.:
    
                        T4.  T5
                     x-----x---H(x)
               T1   /<---- T3      T6     T7  S
            x------x-------x-x-----------x----x
                        T2   [===========]
                               PLATFORM

            Edges: T1,...,T4.
            Nodes: All X

            -If track contains signal, the edge is splitted and the node is inserted.
            This allows to also represent one/multi directional traffic in directed graph

            -If platform is added, two nodes are inserted on the edges, containing information and setting reference to the encapsulated trcks.
            -> This would still allow to place to signal, switches alongside platforms
        */

#if DEBUG
        /// <summary>
        /// Method only available in DEBUG to randomly create arbitrary Factory, that holds a 10 node graph
        /// NOT AVAIL IN Release config.
        /// </summary>
        public static GraphFactory CreateDemoFactory()
        {
            var factory = new GraphFactory();
            for (int i = 0; i < 10; i++) factory.AddNode(i.ToString(), [0f, 0f]);

            for (int i = 0; i < 10; i++)
            {
                var source = new Random().Next(0, 10);
                var target = new Random().Next(0, 10);
                factory.AddEdge(i.ToString() + "-Edge", source.ToString(), target.ToString());
            }

            return factory;
        }
        /// <summary>
        /// Method only available in DEBUG to randomly create arbitrary Factory, that holds a 10 node graph.
        /// NOT AVAIL IN Release config.
        /// </summary>
        /// <returns> 
        /// Returns Graph instance created by <see cref="CreateDemoFactory"/>
        /// </returns>
        public static BidirectionalGraph<INetworkGraphVertex, NetworkGraphEdge> CreateDemoGraph()
        {
            return GraphFactory.CreateDemoFactory().GetGraph();
        }

        /// <summary>
        /// Returns random edge of graph instance, for testing...
        /// NOT AVAIL IN Release config.
        /// </summary>
        public NetworkGraphEdge GetRandomEdge()
        {
            var edge = _edges.ElementAt(new Random().Next(_edges.Values.Count)).Value;
            if (((IEdge<NetworkGraphVertex>)edge).Source.uuid == ((IEdge<NetworkGraphVertex>)edge).Target.uuid) GetRandomEdge();
            return edge;
        }

        public NetworkGraphVertex GetRandomVertex()
        {
            var vertex = _nodes.ElementAt(new Random().Next(_edges.Values.Count)).Value;
            return vertex;
        }
        #endif

        public NetworkGraphVertex GetNode(string uuid)
        {
            return _nodes[uuid];
        }
    }
}
