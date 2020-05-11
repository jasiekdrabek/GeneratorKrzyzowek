using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GeneratorKrzyzowekUnitTest
{
    public class a
    {
        static void Main() { }
    }
    public class GeneratorKrzyzowekTests
    {
        [Test]
        public void GetPasswordLenght_WhenCalled_ShowStrToInt()
        {
            var cos = new GeneratorKrzyzowek.TrackClueList();
            var res = cos.GetPasswordLenght("3");
            Assert.AreEqual(3, res);
            res =  cos.GetPasswordLenght("4");
            Assert.AreEqual(4, res);
            res = cos.GetPasswordLenght("5");
            Assert.AreEqual(5, res);
            res = cos.GetPasswordLenght("156");
            Assert.AreEqual(156, res);
            res = cos.GetPasswordLenght("not number");
            Assert.Greater(res, 2);
            Assert.Less(res, 16);
            res = cos.GetPasswordLenght(null);
            Assert.Greater(res,2);
            Assert.Less(res,16);
            res = cos.GetPasswordLenght("");
            Assert.Greater(res,2);
            Assert.Less(res,16);

        }
        [Test]
        public void NextIndex_WhenCalled_ShowNextIndex()
        {
            var cos = new GeneratorKrzyzowek.TrackClueList();
            var res = cos.NextIndex(2, 7);
            Assert.AreEqual(3, res);
            res = cos.NextIndex(2, 3);
            Assert.AreEqual(0, res);
            res = cos.NextIndex(7, 7);
            Assert.AreEqual(1, res);
        }
    }
}
