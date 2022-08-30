using System.Threading.Tasks;
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
        Task<EBookDto> CreateEbookDtoFromUrlAsync(string uri); 

    }
}
