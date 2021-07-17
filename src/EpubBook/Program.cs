using System.Collections.Generic;
using System;
using EpubBook.Services;

namespace EpubBook
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var ithome = new IThomeService();
      ithome.CreateEbookDtoFromUrl("https://ithelp.ithome.com.tw/users/20120491/ironman/2538");
    }
  }
}
