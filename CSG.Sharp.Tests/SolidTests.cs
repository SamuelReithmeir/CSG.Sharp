using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSG.Sharp
{
    [TestClass]
    public class SolidTests
    {
        [TestMethod]
        public void TestSphereCreation()
        {
            var s = Sphere.Create();

            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestCylinderCreation()
        {
            var c = Cylinder.Create();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void TestCubeCreation()
        {
            var c = Cube.Create();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            var cube = Cube.Create(new Vector(10, 10, 10), 10);
            var cylinder = Cylinder.Create(new Vector(10, 10, 10), new Vector(10, 0, 10), 5);
            var polygons=cube.Subtract(cylinder).ToPolygons();
            Assert.AreEqual(55, polygons.Length);
        }
    }
}