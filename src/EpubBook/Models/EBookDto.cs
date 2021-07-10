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
    public string Name {get;set;}

    /// <summary>
    /// 目錄
    /// </summary>
    /// <value></value>
    public IEnumerable<AgendaDto> Agenda{get;set;}


    

  }
}
