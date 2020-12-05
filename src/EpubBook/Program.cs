using System.Collections.Generic;
using System;
using RestSharp;
using HtmlAgilityPack;
using System.Linq;
using EpubBook.Models;

namespace EpubBook
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      GetIThomeIronmanHtml("https://ithelp.ithome.com.tw/users/20120491/ironman/2538");
    }

    /// <summary>
    /// 讀取鐵人賽HTML內容
    /// </summary>
    /// <param name="q"></param>
    static void GetIThomeIronmanHtml(string uri)
    {
      var client = new RestClient();
      var request = new RestRequest(uri);
      var response = client.Get(request);
      Console.WriteLine(response);
      var document = new HtmlDocument();
      document.LoadHtml(response.Content);
      var documents = new List<HtmlDocument>();
      documents.Add(document);
      documents.AddRange(GetTotalPaging(document));
      var creator = "德瑞克";
      Console.WriteLine($"creator : {creator}");
      GetAuthor(document);
      GetTitle(document);
      GetDescription(document);
      
      foreach(var doc in documents){
        GetAgenda(doc);
      }
    }

   
    /// <summary>
    /// 所有分頁
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    private static IEnumerable<HtmlDocument> GetTotalPaging(HtmlDocument document)
    {
      var documents = new List<HtmlDocument>();
      var node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[2]/ul");
      // 裝載第一層查詢結果 
      HtmlDocument hdc = new HtmlDocument();
      // XPath 來解讀它
      hdc.LoadHtml(node.InnerHtml);
      HtmlNodeCollection htnode = hdc.DocumentNode.SelectNodes(@"//li/a");
      List<string> page_urls = new List<string>();
      foreach (HtmlNode currNode in htnode)
      {
        string currLink = currNode.SelectSingleNode(".").Attributes["href"].Value;
        if (!page_urls.Any(o => o == currLink))
        {
          page_urls.Add(currLink);
        }
      }
        foreach(var url in page_urls) {
            Console.WriteLine($"url : {url}");
            var client = new RestClient();
            var request = new RestRequest(url);
            var response = client.Get(request);
            Console.WriteLine(response);
            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);
            yield return doc;
        }
    }

    private static void GetAuthor(HtmlDocument document)
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// 取得標題
    /// </summary>
    /// <param name="document"></param>
    private static void GetTitle(HtmlDocument document)
    {
      //標題
      var titleNode = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/h3");
      var title = titleNode.InnerText.Replace("\n", "").Trim();
      Console.WriteLine($"title : {title}");
    }

    /// <summary>
    /// 取得描述
    /// </summary>
    /// <param name="document"></param>
    private static void GetDescription(HtmlDocument document)
    {
      // 簡述
      var title_desc_Node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/p");
      var title_desc = title_desc_Node.InnerText.Trim();
      Console.WriteLine($"title_desc : {title_desc}");
    }


    /// <summary>
    /// 取得目錄
    /// </summary>
    /// <param name="document"></param>
    private static IEnumerable<AgendaDto> GetAgenda(HtmlDocument document)
    {
      var agenda_nodes = document.DocumentNode.SelectNodes("//h3[contains(@class, 'qa-list__title')]");
      foreach (var agenda_node in agenda_nodes.ToList())
      {
        HtmlDocument hda = new HtmlDocument();
        // XPath 來解讀它
        hda.LoadHtml(agenda_node.InnerHtml);
        var agenda_title_node = hda.DocumentNode.SelectSingleNode(@"//a");
        if (agenda_title_node != null)
        {
          var agenda_title = agenda_title_node.InnerHtml.Trim();
          var agenda_url = agenda_title_node.SelectSingleNode(".").Attributes["href"].Value.Trim();
          Console.WriteLine($"agenda_title_node : {agenda_title}");
          Console.WriteLine($"agenda_url : {agenda_url}");
          yield return new AgendaDto(agenda_title ,agenda_url );
        }
      }
    }

   private static void ImageFormat(){
       var imageName = "123.png";
       var imageHtml = $"<div style=\"text-indent:0;text-align:center;margin-right:auto;margin-left:auto;width:99%;page-break-before:auto;page-break-inside:avoid;page-break-after:auto;\"><div style=\"margin-left:0;margin-right:0;text-align:center;text-indent:0;width:100%;\"><p style=\"display:inline-block;text-indent:0;width:100%;\"><img alt=\"aladore 1\" src=\"../Images/{imageName}\" style=\"width:99%;\"/></p></div></div>";


   }

  }
}
