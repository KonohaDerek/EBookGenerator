using System.IO;
using EpubBook.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubBookTests.Util
{
  [TestClass]
  public class EpubUtilTests
  {
    [TestMethod]
    public void CreateEpub3FileTest()
    {
      var epub = new EpubUtil();
      var path = epub.CreateEpub3File();
      var exist = File.Exists(path);
      Assert.AreEqual(path, "");
      Assert.IsTrue(exist);

    }
  }
}
