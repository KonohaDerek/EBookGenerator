using EpubBook.Models;


namespace EpubBook.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class IThomeService
    {
        private EBookDto Book;

        public IThomeService(string url)
        {
            Book = new EBookDto()
            {

            };
            // var client = new RestClient();
            // var request = new RestRequest(uri);
            // var response = client.Get(request);
            // Console.WriteLine(response);
            // var document = new HtmlDocument();
            // document.LoadHtml(response.Content);
            // var documents = new List<HtmlDocument>();
            // documents.Add(document);
            // documents.AddRange(GetTotalPaging(document));
            // var creator = "德瑞克";
            // Console.WriteLine($"creator : {creator}");
            // GetAuthor(document);
            // GetTitle(document);
            // GetDescription(document);
            // foreach (var doc in documents)
            // {
            //     GetAgenda(doc);
            // }
        }
    }
}
