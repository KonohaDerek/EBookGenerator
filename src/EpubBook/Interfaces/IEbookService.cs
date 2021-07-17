using EpubBook.Models;

namespace EpubBook.Interfaces
{
    public interface IEBookService
    {
        /// <summary>
        /// 建立EBookDto
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        EBookDto CreateEbookDtoFromUrl(string uri); 

    }
}
