using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EpubBook.Util;

namespace EpubBook.Models
{
    public class EBookDto
    {

        /// <summary>
        /// 作者
        /// </summary>
        /// <value></value>
        public string Author { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        /// <value></value>
        public string Creator { get; set; }

        /// <summary>
        /// 書名
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// 簡述
        /// </summary>
        /// <value></value>
        public string Describe { get; set; }

        /// <summary>
        /// 出版社
        /// </summary>
        /// <value></value>
        public string Publisher { get; set; }

        /// <summary>
        /// 目錄
        /// </summary>
        /// <value></value>
        public IEnumerable<AgendaDto> Agenda { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        /// <value></value>
        public IEnumerable<ContentDto> Contents { get; set; }


        public string ToEpub3File()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var saveFileName = Path.ChangeExtension(Path.Combine(rootPath, Title), ".epub");
            var templatePath = Path.Combine(rootPath, "EpubTemplate");
            var tempPath = Path.Combine(rootPath, "Temp");

            Extension.CopyDirectory(templatePath, tempPath, true);
            GenerateTempEpubFiles(tempPath);
            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }
            ZipFile.CreateFromDirectory(tempPath, saveFileName, CompressionLevel.Fastest, false);
            return saveFileName;
        }

        private void GenerateTempEpubFiles(string path)
        {
            Console.WriteLine("Start GenerateTempEpubFiles");
            // 建立 navigation-documents.xhtml
            Console.WriteLine("Start create navigation-documents.xhtml");
            GenerateNavigationDocumentsFile(path);
            Console.WriteLine("End create navigation-documents.xhtml");
            // 建立 cover.xhtml
            Console.WriteLine("Start create cover.xhtml");
            GenerateCoverXHtmlFile(path);
            Console.WriteLine("End create cover.xhtml");
            // 建立 titlepage.xhtml
            Console.WriteLine("Start create titlepage.xhtml");
            GenerateTitlePageXHtmlFile(path);
            Console.WriteLine("End create titlepage.xhtml");
            // 建立 toc.xhtml
            Console.WriteLine("Start create toc.xhtml");
            GenerateTocXHtmlFile(path);
            Console.WriteLine("End create toc.xhtml");
            // 建立 p-xxx.xhtml
            Console.WriteLine("Start create p-xxx.xhtml");
            GenerateContentXHtmlFile(path);
            Console.WriteLine("End create p-xxx.xhtml");

            // 建立 colophon.xhtml
            Console.WriteLine("Start create colophon.xhtml");
            GenerateColophonXHtmlFile(path);
            Console.WriteLine("End create colophon.xhtml");
            // 建立 standard.opf
            Console.WriteLine("Start create standard.opf");
            GenerateStandardFile(path);
            Console.WriteLine("End create standard.opf");
        }

        private void GenerateStandardFile(string path)
        {
            var fileName = Path.Combine(path, "item/standard.opf");
            Console.WriteLine("GenerateStandardFile : " + fileName);
            string text = File.ReadAllText(fileName);
            text = text.Replace("{{book_title}}", this.Title);
            text = text.Replace("{{book_author}}", this.Author);
            text = text.Replace("{{book_publisher}}", this.Publisher);
            text = text.Replace("{{book_uuid}}", Guid.NewGuid().ToString());
            text = text.Replace("{{book_update_at}}", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", DateTimeFormatInfo.InvariantInfo));

            // TODO: use files  
            // write xhtml files to standard
            Regex reg = new(@"p-[\d]{3}.xhtml");
            DirectoryInfo dir = new(Path.Combine(path, "item"));
            List<string> xhtmlFiles = dir.EnumerateFiles("*.xhtml", SearchOption.AllDirectories).Where(x => reg.IsMatch(x.Name)).Select(x => x.Name).ToList();
            StringBuilder sb_xhtml = new();
            StringBuilder sb_itemrefs = new();
            foreach (var file in xhtmlFiles.OrderBy(o => o))
            {
                // file remove extension
                var xhtmlFileName = Path.GetFileNameWithoutExtension(file);
                sb_xhtml.AppendLine(string.Format("<item media-type=\"application/xhtml+xml\" id=\"{0}\"  href=\"xhtml/{1}\"/>", xhtmlFileName, file));
                sb_itemrefs.AppendLine(string.Format("<itemref linear=\"yes\" idref=\"{0}\"/>", xhtmlFileName));
            }

            // write images to standard
            var sb_images = new StringBuilder();
            var imageFiles = new DirectoryInfo(Path.Combine(path, "item", "image")).EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => x.Name != "cover.jpg").ToList();
            foreach (var file in imageFiles.OrderBy(x => x.CreationTime))
            {
                var imageFileName = Path.GetFileNameWithoutExtension(file.Name);
                sb_images.AppendLine(string.Format("<item media-type=\"image/{0}\" id=\"i_{1}\" href=\"{2}\"/>", file.Extension.Replace(".", ""), Path.GetFileNameWithoutExtension(file.Name), Path.Combine("image", file.Directory.Name, file.Name)));
            }


            text = text.Replace("{{book_xhtmls}}", sb_xhtml.ToString());
            text = text.Replace("{{book_itemrefs}}", sb_itemrefs.ToString());
            text = text.Replace("{{book_images}}", sb_images.ToString());
            File.WriteAllText(fileName, text);
        }

        private void GenerateNavigationDocumentsFile(string path)
        {
            string fileName = Path.Combine(path, "item/navigation-documents.xhtml");
            string text = File.ReadAllText(fileName);
            StringBuilder sb_toc = new();
            foreach (AgendaDto agenda in Agenda?.OrderBy(o => o.Idx))
            {
                sb_toc.AppendLine(string.Format("<li><a href=\"xhtml/p-{0:000}.xhtml#toc_index_{0}\"><span>{1}</span></a></li>", agenda.Idx, agenda.Title));
            }
            text = text.Replace("{{book_toc}}", sb_toc.ToString());
            File.WriteAllText(fileName, text);
        }

        private void GenerateCoverXHtmlFile(string path)
        {
            string fileName = Path.Combine(path, "item/xhtml/p-cover.xhtml");
            string text = File.ReadAllText(fileName);
            text = text.Replace("{{book_title}}", Title);
            File.WriteAllText(fileName, text);
        }


        private void GenerateTitlePageXHtmlFile(string path)
        {
            string fileName = Path.Combine(path, "item/xhtml/p-titlepage.xhtml");
            string text = File.ReadAllText(fileName);
            text = text.Replace("{{book_title}}", Title);
            text = text.Replace("{{bool_author}}", Author);
            File.WriteAllText(fileName, text);
        }

        private void GenerateTocXHtmlFile(string path)
        {
            string fileName = Path.Combine(path, "item/xhtml/p-toc.xhtml");
            string text = File.ReadAllText(fileName);
            StringBuilder sb_toc = new();
            foreach (AgendaDto agenda in Agenda?.OrderBy(o => o.Idx))
            {
                sb_toc.AppendLine(string.Format("<p><a href=\"p-{0:000}.xhtml#toc_index_{0}\"><span>{1}</span></a></p>", agenda.Idx, agenda.Title));
            }
            text = text.Replace("{{book_content_href}}", sb_toc.ToString());
            File.WriteAllText(fileName, text);
        }

        private void GenerateColophonXHtmlFile(string path)
        {
            string fileName = Path.Combine(path, "item/xhtml/p-colophon.xhtml");
            string text = File.ReadAllText(fileName);
            text = text.Replace("{{book_title}}}", Title);
            text = text.Replace("{{book_create_at}}", DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo));
            File.WriteAllText(fileName, text);
        }

        private void GenerateContentXHtmlFile(string path)
        {
            string fileName = Path.Combine(path, "item/xhtml/p-000.xhtml");
            string text = File.ReadAllText(fileName);
            foreach (AgendaDto agenda in Agenda?.OrderBy(o => o.Idx))
            {
                var tempText = text;
                tempText = tempText.Replace("{{book_title}}", Title);
                tempText = tempText.Replace("{{book_topic}}", agenda.Title);
                tempText = tempText.Replace("{{book_content}}", agenda.Content.GenerateXhtml(path, agenda.Idx));
                tempText = tempText.Replace("<br>", Environment.NewLine);
                tempText = tempText.Replace("<hr>", "<hr />");
                File.WriteAllText(Path.Combine(path, string.Format("item/xhtml/p-{0:000}.xhtml", agenda.Idx)), tempText);
            }
            // remove p-000.xhtml
            File.Delete(fileName);
        }
    }
}
