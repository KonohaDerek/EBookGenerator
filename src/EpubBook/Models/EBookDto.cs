using System.Collections.Generic;
using System;


namespace EpubBook.Models
{
  public class EBookDto {

    /// <summary>
    /// 作者
    /// </summary>
    /// <value></value>
    public string Author {get;set;}

    /// <summary>
    /// 建立者
    /// </summary>
    /// <value></value>
    public string Creator{get;set;}

    /// <summary>
    /// 書名
    /// </summary>
    /// <value></value>
    public string Title {get;set;}

    /// <summary>
    /// 簡述
    /// </summary>
    /// <value></value>
    public string Describe{get;set;}

    /// <summary>
    /// 目錄
    /// </summary>
    /// <value></value>
    public IEnumerable<AgendaDto> Agenda{get;set;}

    /// <summary>
    /// 內容
    /// </summary>
    /// <value></value>
    public IEnumerable<ContentDto> Contents {get;set;}
  
  }
}
