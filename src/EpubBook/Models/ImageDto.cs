using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RestSharp;

namespace EpubBook.Models
{
    public class ImageDto
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public byte[] ImageByte { get; set; }

        public HtmlNode Node { get; set; }

        public string SaveAsync(string path, int idx)
        {
            var fullPath = System.IO.Path.Combine(path, string.Format("item/image/p-{0:000}/{1}", idx, Name));

            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
            File.WriteAllBytes(fullPath, ImageByte);
            return fullPath;
        }


        public static async IAsyncEnumerable<ImageDto> LoadFromHtmlAsync(string host, string html)
        {
            // Load the Html into the agility pack
            HtmlDocument doc = new();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectNodes("//img") == null)
            {
                yield break;
            }

            // Now, using LINQ to get all Images
            var imageNodes = (from HtmlNode node in doc.DocumentNode.SelectNodes("//img")
                              where node.Name == "img"
                              select node).ToList();

            foreach (var node in imageNodes)
            {
                var path = node.Attributes["src"].Value;
                // check src is absolute path
                if (!path.StartsWith("http"))
                {
                    path = "https://" + host + path;
                }

                yield return new ImageDto
                {
                    Name = System.IO.Path.GetFileName(node.Attributes["src"].Value),
                    Path = node.Attributes["src"].Value,
                    ImageByte = await new HttpClient().GetByteArrayAsync(new Uri(path)),
                    Node = node
                };
            }
        }
    }
}
