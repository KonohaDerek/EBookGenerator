using System.Collections.Generic;
using System;
using RestSharp;
using HtmlAgilityPack;
using System.Linq;

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
        static void GetIThomeIronmanHtml(string uri) {
            List<string> page_urls = new List<string>();
            Dictionary<string,string> dic = new Dictionary<string, string>();

            page_urls.Add(uri);
            var client = new RestClient();
            var request = new RestRequest(uri);
            var response =client.Get(request);
            Console.WriteLine(response);
            var document = new HtmlDocument();
            document.LoadHtml(response.Content);

            var creator = "德瑞克";
            Console.WriteLine($"creator : {creator}");
            
            //標題
            var titleNode = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/h3");
            var title = titleNode.InnerText.Replace("\n" , "").Trim();
            Console.WriteLine($"title : {title}");
            // 簡述
            var title_desc_Node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[1]/div[2]/p");
            var title_desc = title_desc_Node.InnerText.Trim();
            Console.WriteLine($"title_desc : {title_desc}");

            var node = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/div[2]/div[2]/ul");
            // 裝載第一層查詢結果 
            HtmlDocument hdc = new HtmlDocument();

            // XPath 來解讀它
            hdc.LoadHtml(node.InnerHtml);

            HtmlNodeCollection htnode = hdc.DocumentNode.SelectNodes(@"//li/a");
            foreach (HtmlNode currNode in htnode)
            {
                string currLink = currNode.SelectSingleNode(".").Attributes["href"].Value;
                if(!page_urls.Any(o=>o == currLink)){
                    page_urls.Add(currLink);
                }
            }

            page_urls.ForEach(o=> Console.WriteLine($"url : {o}"));

            // 解析目錄
            foreach(var page in page_urls) {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        static Dictionary<string, string> GetBlogContent(string uri) {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            

            return dic;
        }

    }
}
