using NUnit.Framework;
using PayDay2SaveView.Entities;
using PayDay2SaveView.Utils;

namespace Tests
{
    [TestFixture]
    public class EnumUtilsTests
    {
        [Test]
        public void It_Should_Not_Raise_Exception_For_Every_Villain()
        {
            foreach (Villain field in System.Enum.GetValues(typeof(Villain)))
                EnumUtils.GetString(field);
        }
    }
}
