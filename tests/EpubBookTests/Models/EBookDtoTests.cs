using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using EpubBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubBookTests.Models
{
    [TestClass]
    public class EBookDtoTests
    {
        [TestMethod]
        public void ToEpubFileTest()
        {
            var dto = new EBookDto()
            {
                Title = "測試",
                Agenda = new List<AgendaDto>().AsEnumerable(),
            };
            var file = dto.ToEpub3File();
            var fileInfo = new FileInfo(file);

            Assert.AreEqual(fileInfo.Name, "測試.epub");
        }
    }
}
