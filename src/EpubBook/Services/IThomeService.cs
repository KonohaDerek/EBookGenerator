using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using EpubBook.Interfaces;
using EpubBook.Models;
using HtmlAgilityPack;
using RestSharp;

namespace EpubBook.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class IThomeService : IEBookService
    {
        /// <summary>
        /// 建立EBookDto
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public async Task<EBookDto> CreateEbookDtoFromUrlAsync(string uri)
        {
            // 取得網址host
            var host = new Uri(uri).Host;

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
            var agendas = new List<AgendaDto>();
            var idx = 0;
            foreach (var doc in documents)
            {
                agendas.AddRange(GetAgenda(doc, idx).ToList());
                idx = agendas.Max(o => o.Idx);
            }

            foreach (var agenda in agendas)
            {
                var content = GetContent(agenda.Url);
                agenda.Content = new ContentDto()
                {
                    HtmlContent = content,
                    ContentName = agenda.Title,
                    Images = new List<ImageDto>()
                };
                await foreach (var img in ImageDto.LoadFromHtmlAsync(host, content))
                {
                    agenda.Content.Images.Add(img);
                }
            }

            return new EBookDto()
            {
                Publisher = "iThome",
                Creator = creator,
                Author = author,
                Title = title,
                Describe = desc,
                Agenda = agendas,
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
        private IEnumerable<HtmlDocument> GetPagingHtmlDocument(IEnumerable<string> page_urls)
        {
            foreach (var url in page_urls)
            {
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
        private IEnumerable<AgendaDto> GetAgenda(HtmlDocument document, int idx)
        {
            var agenda_nodes = document.DocumentNode.SelectNodes("//h3[contains(@class, 'qa-list__title')]");
            foreach (var agenda_node in agenda_nodes.ToList())
            {
                idx++;
                HtmlDocument hda = new HtmlDocument();
                // XPath 來解讀它
                hda.LoadHtml(agenda_node.InnerHtml);
                var agenda_title_node = hda.DocumentNode.SelectSingleNode(@"//a");
                if (agenda_title_node != null)
                {
                    var agenda_title = agenda_title_node.InnerHtml.Trim();
                    var agenda_url = agenda_title_node.SelectSingleNode(".").Attributes["href"].Value.Trim();
                    yield return new AgendaDto(agenda_title, agenda_url, idx);
                }
            }
        }

        private string GetAuthor(HtmlDocument document)
        {
            //標題
            var authorNode = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[1]/div[1]/div[2]/div[1]");
            var author = authorNode.InnerText.Replace("\n", "").Trim();
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
            return content;
        }
    }
}
