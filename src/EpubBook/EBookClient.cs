using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EpubBook.Models;
using EpubBook.Services;

namespace EpubBook
{
    public class EBookClient
    {

        public static async Task<string> CreateEbookFromUrl(string url)
        {

            Dictionary<string, Func<string, Task<Models.EBookDto>>> services = new Dictionary<string, Func<string, Task<EBookDto>>>(){
            { "ithelp.ithome.com.tw",  async (string url)=> {
                var ithome = new IThomeService();
                return  await ithome.CreateEbookDtoFromUrlAsync(url);
              }},
        };
            Uri uri = new Uri(url);
            if (services.ContainsKey(uri.Host))//防呆
            {
                var ebook = await services[uri.Host].Invoke(url);
                var file = ebook.ToEpub3File();
                return file;
            }
            else
            {
                throw new NotSupportedException($"not support this url: {url}");
            }
        }
    }
}
