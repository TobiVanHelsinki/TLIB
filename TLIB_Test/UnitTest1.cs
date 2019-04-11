using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLIB;

namespace TLIB_Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMathExtensions()
        {
            int Lower = 5;
            int Higher = 6;
            Assert.AreEqual(Lower, Lower.UpperB(Higher));
            Assert.AreEqual(Lower, Lower.UpperB(Lower));
            Assert.AreEqual(Higher, Higher.LowerB(Higher));
            Assert.AreEqual(Lower, Higher.UpperB(Lower));

            Lower = 5;
            Higher = 6;
            Assert.AreEqual(Higher, Lower.LowerB(Higher));
            Assert.AreEqual(Lower, Lower.LowerB(Lower));
            Assert.AreEqual(Higher, Higher.LowerB(Higher));
            Assert.AreEqual(Higher, Higher.LowerB(Lower));

            Lower = 5;
            Higher = 6;
            Assert.AreEqual(Lower, Higher.Min(Lower));
            Assert.AreEqual(Lower, Lower.Min(Lower));
            Assert.AreEqual(Higher, Higher.Min(Higher));
            Assert.AreEqual(Lower, Lower.Min(Higher));

            Lower = 5;
            Higher = 6;
            Assert.AreEqual(Higher, Higher.Max(Lower));
            Assert.AreEqual(Lower, Lower.Max(Lower));
            Assert.AreEqual(Higher, Higher.Max(Higher));
            Assert.AreEqual(Higher, Lower.Max(Higher));

            int Fallback = 99;
            Lower = 5;
            Higher = 6;
            Assert.AreEqual(Lower, Lower.UpperB(Higher, Fallback));
            Assert.AreEqual(Fallback, Lower.UpperB(Lower, Fallback));
            Assert.AreEqual(Fallback, Higher.LowerB(Higher, Fallback));
            Assert.AreEqual(Fallback, Higher.UpperB(Lower, Fallback));

            Lower = 5;
            Higher = 6;
            Assert.AreEqual(Fallback, Lower.LowerB(Higher, Fallback));
            Assert.AreEqual(Fallback, Lower.LowerB(Lower, Fallback));
            Assert.AreEqual(Fallback, Higher.LowerB(Higher, Fallback));
            Assert.AreEqual(Higher, Higher.LowerB(Lower, Fallback));
        }
    }
}
