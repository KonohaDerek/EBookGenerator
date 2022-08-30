using System;

namespace EpubBook.Models
{
  /// <summary>
  /// 目錄
  /// </summary>
  public class AgendaDto
  {
     /// <summary>
    ///  索引
    /// </summary>
    /// <value></value>
    public int Idx {get;set;}
    
    /// <summary>
    /// 標題
    /// </summary>
    /// <value></value>
    public string Title { get; private set; }
 
    /// <summary>
    /// 網址
    /// </summary>
    /// <value></value>
    public string Url { get; private set; }

    /// <summary>
    /// 內容
    /// </summary>
    /// <value></value>
    public ContentDto Content { get; set;}
    
    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="title"></param>
    /// <param name="url"></param>
    public AgendaDto(string title , string url, int idx)
    {
        Title = title;
        Url = url;
        Idx = idx;
    }

    public string ToNavigationTocHtmlString(){
        return string.Format("<li><a href=\"xhtml/p-{0:000}.xhtml#toc_index_{0}\"><span>{1}</span></a></li>", Idx,Title); ;
    }
  }
}
