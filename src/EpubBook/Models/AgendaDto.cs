namespace EpubBook.Models
{
  /// <summary>
  /// 目錄
  /// </summary>
  public class AgendaDto
  {
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
    /// 建構子
    /// </summary>
    /// <param name="title"></param>
    /// <param name="url"></param>
    public AgendaDto(string title , string url)
    {
        this.Title = title;
        this.Url = url;
    }

    

  }
}
