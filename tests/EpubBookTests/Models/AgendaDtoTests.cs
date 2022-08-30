using System;
using System.IO;
using EpubBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubBookTests.Models
{
    [TestClass]
    public class AgendaDtoTests
    {
        [TestMethod]
        public void ToHtmlStringTest()
        {
            var dto = new AgendaDto("測試","http://ss.com",1);
            var navigationTocHtmlString = dto.ToNavigationTocHtmlString();
            var expected = "<li><a href=\"xhtml/p-001.xhtml#toc_index_1\"><span>測試</span></a></li>";
            Assert.AreEqual(expected, navigationTocHtmlString);
        }
    }
}