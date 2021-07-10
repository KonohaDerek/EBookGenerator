using EpubBook.Models;

namespace EpubBook.Interfaces
{
    public interface IEbookService
    {
        /// <summary>
        /// 建立EBookDto
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
         public EBookDto CreateEbookDtoFromUrl(string Url); 

    }
}
