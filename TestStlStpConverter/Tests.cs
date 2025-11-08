using System;
using Bolsover.Converter;
using NUnit.Framework;

namespace TestStlToStp
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            StepWriter stepWriter = new StepWriter();
            stepWriter.ReadStep("bucket.stp");
        }
    }
}