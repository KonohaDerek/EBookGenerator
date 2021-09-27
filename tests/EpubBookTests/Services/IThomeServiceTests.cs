using EpubBook.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubBookTests.Services
{
  [TestClass]
  public class IThomeServiceTests
  {
    public string Content = @"";

    [TestMethod]
    public void GetCreateEbookDtoFromUrlTest()
    {
      var url = "https://ithelp.ithome.com.tw/users/20120491/ironman/2538";
      var ithome = new IThomeService();
      var actual = ithome.CreateEbookDtoFromUrl(url);
      Assert.AreEqual(actual.Creator, "德瑞克");
      Assert.AreEqual(actual.Author, "Neil (neil605164)");
      Assert.AreEqual(actual.Title, "就是「懶」才更需要重視DevOps 系列");
    }

    [TestMethod]
    public void GetContentTest()
    {
        var url = "https://ithelp.ithome.com.tw/articles/10215725";

        var ithome = new IThomeService();
        var actual = ithome.GetContent(url);
        Assert.AreEqual(actual , "<div class=\"markdown\">\n                        <div class=\"markdown__style\">\n                                                            <h2>前言</h2>\n<p>終於鼓起勇氣挑戰了第一次的IT鐵人賽，之所以選擇寫 DevOps 主題的原因其實很簡單。<br>\n倘若需要一次性的管理一千多台虛擬機，也許有人會寫一些 shell 作為維運的工具，但每天仍有層出不窮、各式各樣的虛擬機問題找上門，總是一而再、再而三的通靈處理機器問題，最終促使我奔向 docker 的懷抱，只為了減少各式虛擬機的疑難雜症，讓我可以專心攻略其他新技術；怎知隨著對技術研究的深入，連同 drone、K8S 也一併栽坑了。</p>\n<h2>未來的30天</h2>\n<p>未來的30天內，在觀念方面的著墨會比技術層面來得稍微多一些，盡量分享我與團隊在邁向 DevOps 中，實行以來碰到的問題。<br>\n雖然在技術轉換的過程中較為艱辛，不過卻能夠為團隊帶來更大的效益，讓工程師能夠專注於開發；至於繁雜的系統層面則交由專業的 MIS 去煩惱。<br>\n而面對複雜的專案架構，也可以透過容器與微服務的概念，將複雜的架構簡單化。</p>\n<h2>何謂 DevOps</h2>\n<p>從許多文章上可以瞭解到「DevOps」為「Development+Operations」，「DevOps」可以說是將開發與運維融會貫通後的一種技術能力體現，曾經看到一篇文章寫著「 Dev+Ops 是指\"開發者多去想維運面，維運人員多去想開發面\"」。<br>\n我覺得這是所有工程師都需要具備的能力，不論服務是否上線，在後續的維護上不僅方便許多，也能從另一方面降低與MIS溝通上的落差。</p>\n<p><img src=\"https://i.imgur.com/F15FJej.png\" alt=\"\"><br>\n(取自於網路圖片)<br>\n如上圖，「開發」+「運維」+「測試」三者的交集就構成了「DevOps」。</p>\n<p>只是，時間終究有限，要一個人同時完成這三項作業簡直是不可能的任務，因此這時候就需要 「自動化」 協助「DevOps」人員。</p>\n<p>由於每次程式上線流程都大同小異，透過\"自動佈署 + 自動測試\"的自動化流程，除了解決這繁瑣又 routine 的動作之外，還能讓工程師在日常維運之餘，也能追求新技術，不至於被日常工作消磨掉了時間與精力。</p>\n <br>\n                                                    </div>\n                    </div>");
    }
  }
}
