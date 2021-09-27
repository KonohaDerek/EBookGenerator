using System.Collections.Generic;


namespace EpubBook.Models
{
    public class ContentDto
    {
        /// <summary>
        /// 頁面名稱
        /// </summary>
        /// <value></value>
        public string ContentName { get; set; }

        /// <summary>
        /// Html 內容
        /// </summary>
        /// <value></value>
        public string HtmlContent {get;set;}

        /// <summary>
        /// 圖片資訊
        /// </summary>
        /// <value></value>
        public List<ImageDto> Images {get;set;}
    }
}
