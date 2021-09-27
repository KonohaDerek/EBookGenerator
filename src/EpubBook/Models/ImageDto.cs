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

    public HtmlNode Node {get;set;}

    public async Task SaveAsync()
    {
      var basePath = "";
      var fullPath = System.IO.Path.Combine(basePath, Path, Name);
      using var mem = new MemoryStream(ImageByte);
      using var stream = File.Create(fullPath);
      await mem.CopyToAsync(stream);
    }


    public static async IAsyncEnumerable<ImageDto> LoadFromHtmlAsync(string html)
    {
      // Load the Html into the agility pack
      HtmlDocument doc = new HtmlDocument();
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
        yield return new ImageDto
        {
          Name = System.IO.Path.GetFileName(node.Attributes["src"].Value) ,
          Path = node.Attributes["src"].Value,
          ImageByte = await new HttpClient().GetByteArrayAsync(new Uri(node.Attributes["src"].Value)),
          Node = node
        };
      }
    }
  }
}
