using System.Collections.Generic;
using System;
using EpubBook.Services;
using System.Threading.Tasks;

namespace EpubBook
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var ithome = new IThomeService();
      await ithome.CreateEbookDtoFromUrlAsync("https://ithelp.ithome.com.tw/users/20120491/ironman/2538");
    }
  }
}
