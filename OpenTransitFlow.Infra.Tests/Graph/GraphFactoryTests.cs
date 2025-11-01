using NUnit.Framework;
using OpenTransitFlow.Infra.Graph;

namespace OpenTransitFlow.Infra.Tests.Graph
{
    internal class GraphFactoryTests
    {
        [Test]
        public void GraphInitTest()
        {
            GraphFactory factory = new GraphFactory();
            factory.AddTestTrack();
            var result = GraphPngRenderer.Renderer(factory.GetGraph());
            Assert.That("digraph G {\r\n0;\r\n1;\r\n2;\r\n3;\r\n4;\r\n5;\r\n0 -> 4;\r\n1 -> 4;\r\n1 -> 5;\r\n2 -> 4;\r\n3 -> 5;\r\n4 -> 0;\r\n4 -> 1;\r\n5 -> 1;\r\n5 -> 2;\r\n5 -> 3;\r\n}", Is.EqualTo(result.ToString()));
        }
    }
}
