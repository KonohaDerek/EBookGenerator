namespace EpubBook.Models
{
  public class AgendaDto
  {
    public string Title { get; private set; }

    public string Url { get; private set; }

    public AgendaDto(string title , string url){
        this.Title = title;
        this.Url = url;
    }
  }
}
