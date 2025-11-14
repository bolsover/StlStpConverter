using NUnit.Framework;

using System.Threading.Tasks;
using Bolsover.Converter;

namespace TestStlToStp
{
    [TestFixture]
    public class StlStpConverterTests
    {


        [Test]
        public async Task TestConvert()
        {
            var result = await StlReader.ConvertToStp("Pencil Case.stl", "Pencil Case.stp", 0.0000001);
            Assert.AreEqual(0, result);
        }
     }  
}