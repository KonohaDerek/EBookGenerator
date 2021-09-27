using System.Runtime.InteropServices;
using EpubBook.Models;
using EpubBook.Interfaces;
using System.Collections.Generic;
using System;
using RestSharp;
using HtmlAgilityPack;
using System.Linq;
using System.Web;

namespace EpubBook.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class IThomeService : IEBookService
    {
        public IThomeService()
        {

        }

        /// <summary>
        /// 建立EBookDto
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public EBookDto CreateEbookDtoFromUrl(string uri)
        {
            // 讀取網址內容
            var client = new RestClient();
            var request = new RestRequest(uri);
            var response = client.Get(request);

            // 載入網頁內容
            var document = new HtmlDocument();
            document.LoadHtml(response.Content);

            // 取得該文章清單
            var documents = new List<HtmlDocument>();
            documents.Add(document);
            var pagings = GetAllPaging(document);
            documents.AddRange(GetPagingHtmlDocument(pagings));

            // 解析目錄資料
            var creator = "德瑞克";
            Console.WriteLine($"creator : {creator}");
            var author = GetAuthor(document);
            var title = GetTitle(document);
            var desc = GetDescription(document);
            var agenda = new List<AgendaDto>();
            foreach (var doc in documents)
            {
                agenda.AddRange(GetAgenda(doc).ToList());
            }

            // 讀取各內容
            var content = GetContent(agenda.First().Url);
            
            return new EBookDto(){
                Creator = creator,
                Author = author,
                Title = title,
                Describe = desc,
                Agenda = agenda
            };
        }

        /// <summary>
        /// 所有分頁
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private IEnumerable<string> GetAllPaging(HtmlDocument document)
        {
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
                   yield return currLink;
                }
            }
        }

        /// <summary>
        /// 取得所有分頁中的清單資料
        /// </summary>
        /// <param name="page_urls"></param>
        /// <returns></returns>
        private IEnumerable<HtmlDocument> GetPagingHtmlDocument(IEnumerable<string> page_urls) {
            foreach (var url in page_urls)
            {
                Console.WriteLine($"url : {url}");
                var client = new RestClient();
                var request = new RestRequest(url);
                var response = client.Get(request);
                var doc = new HtmlDocument();
                doc.LoadHtml(response.Content);
                yield return doc;
            }
        }

        // <summary>
        /// 取得目錄
        /// </summary>
        /// <param name="document"></param>
        private IEnumerable<AgendaDto> GetAgenda(HtmlDocument document)
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
                    yield return new AgendaDto(agenda_title, agenda_url);
                }
            }
        }

        private string GetAuthor(HtmlDocument document)
        {
            //標題
            var authorNode = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div[1]");
            var author = authorNode.InnerText.Replace("\n", "").Trim();
            Console.WriteLine($"author : {author}");
            return author;
        }


        /// <summary>
        /// 取得標題
        /// </summary>
        /// <param name="document"></param>
        private string GetTitle(HtmlDocument document)
        {
            //標題
            var titleNode = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/h3");
            var title = titleNode.InnerText.Replace("\n", "").Trim();
            Console.WriteLine($"title : {title}");
            return title;
        }

        /// <summary>
        /// 取得描述
        /// </summary>
        /// <param name="document"></param>
        private string GetDescription(HtmlDocument document)
        {
            // 簡述
            var title_desc_Node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/p");
            var title_desc = title_desc_Node.InnerText.Trim();
            Console.WriteLine($"title_desc : {title_desc}");
            return title_desc;
        }

        public string GetContent(string url) 
        {
            var client = new RestClient();
            var request = new RestRequest(url);
            var response = client.Get(request);
            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);
            var content_Node = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[2]/div[3]/div[2]");
            var content = content_Node.InnerHtml.Trim();
            Console.WriteLine($"content : {content}");
            return HttpUtility.HtmlDecode(content);
        }
    }
}
