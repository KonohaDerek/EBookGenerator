using System.Collections.Generic;
using System.IO;

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
        public string HtmlContent { get; set; }

        /// <summary>
        /// 圖片資訊
        /// </summary>
        /// <value></value>
        public List<ImageDto> Images { get; set; }


        internal string GenerateXhtml(string path, int idx)
        {
            var text = HtmlContent;
            foreach (var img in Images)
            {
                _ = img.SaveAsync(path, idx);
                text = text.Replace("alt=\"\">", "alt=\"\"/>");
                text = text.Replace(string.Format("alt=\"{0}\">", img.Path), "alt=\"\"/>");
                text = text.Replace(img.Path, string.Format("../image/p-{0:000}/{1}", idx, img.Name));
            }
            return text;
        }
    }
}
