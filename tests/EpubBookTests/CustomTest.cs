
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubBookTests
{
    [TestClass]
    public class CustomTest
    {
        [TestMethod]
        public void ListDirFilesTest()
        {
            Regex reg = new Regex(@"p-[\d]{3}.xhtml");
            var path = "./Temp2";
            var dir = new DirectoryInfo(Path.Combine(path, "item"));
            var xhtmlFiles = dir.EnumerateFiles("*.xhtml", SearchOption.AllDirectories).Where(x => reg.IsMatch(x.Name)).Select(x => x.Name).ToList();

            foreach (var file in xhtmlFiles.OrderBy(o=>o))
            {
                // file remove extension
                var fileName = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine(string.Format("<item media-type=\"application/xhtml+xml\" id=\"{0}\"  href=\"xhtml/{1}\"/>", fileName, file));
                Console.WriteLine(string.Format("<itemref linear=\"yes\" idref=\"{0}\"/>", fileName));
            }

            // find images from /Temp2/item/image/*/*.*
            var imageFiles = new DirectoryInfo(Path.Combine(path, "item","image")).EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => x.Name != "cover.jpg").ToList();
            foreach (var file in imageFiles.OrderBy(x => x.CreationTime))
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                Console.WriteLine(file.Directory.Name + "\\" + file.Name);
                // <item media-type="image/png" id="tdpf"         href="image/tdpf.png" />
                Console.WriteLine(string.Format("<item media-type=\"image/{0}\" id=\"{1}\" href=\"{2}\"/>", file.Extension.Replace(".",""), fileName,Path.Combine("image",file.Directory.Name , file.Name )));
            }

        }
    }
}
