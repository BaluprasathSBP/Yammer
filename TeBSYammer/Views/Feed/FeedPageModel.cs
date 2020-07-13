using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using PropertyChanged;
using TeBSYammer.Model;
using TeBSYammer.Service;
using System.Linq;

namespace TeBSYammer.Views.Feed
{
    [AddINotifyPropertyChangedInterface]
    public class FeedPageModel
    {
        public string Test { get; set; }
        public string UserName { get; set; }

        public FeedService FeedService { get; set; }

        public IList<YammerFeed> YammerFeeds { get; set; }

        public FeedPageModel()
        {
            UserName = "Baluprasath";


            FeedService = new FeedService();

            Test = $"Employee Engagement FUN!!!<br>---<br>Hi All,<br><br>As you all are working from home, we thought this would be a good time to add in some employee engagement activities. The good news is this activity will give you the opportunity to work with TeBS employees across all locations ☺<br><br>We will be dividing you into teams and we will appoint a leader. The leader needs to ensure that everyone is able to contribute to the game.  We would be making your groups in teams, pls coordinate with each other and come up with exciting “Quotes” 😊. Once completed pls email your submissions to TeBS_HR@totalebizsolutions.com<span class='yammer-object' data-yammer-object='user:467139928064' data-resource-id='467139928064' data-resource-model='user'><a href='https://www.yammer.com/totalebizsolutions.com/users/467139928064'>TeBS_HR</a></span><br><br>Top 2 teams will win exciting prizes!!!<br><br>THEME : “Life during COVID-19”<br><br>Please read below on what the competition is all about :<br><br><br>                          You and your team need to coordinate and come up with a slogan and create image like below: e [cid:image002.jpg@01D61809.065ABAF0]<br><br>Competition ends: 30th April 2020<br><br>Thank you and Best Regards,<br>Suhita<br>HR/Admin Executive<br>[cid:image001.png@01D4FB4B.452727E0] Total eBiz Solutions Pte. Ltd | 31 Ubi Road 1, <span class='yammer-object' data-yammer-object='tag:40660819968' data-resource-id='40660819968' data-resource-model='tag'>#<a href='https://www.yammer.com/totalebizsolutions.com/topics/46928969728'>06-01</a></span>, Annex Building, Sg 408694 Phone : +65 6290 0203  Fax : +65 6290 0208&lt;tel:+65%206290%200208&gt; | <a class=\"linkified\" href=\"http://www.totalebizsolutions.com\" target=\"_blank\" rel=\"nofollow noreferrer\">http://www.totalebizsolutions.com</a>&lt;<a class=\"linkified\" href=\"http://www.totalebizsolutions.com/\" target=\"_blank\" rel=\"nofollow noreferrer\">http://www.totalebizsolutions.com/</a>&gt; Follow us on: [linkedin] &lt;<a class=\"linkified\" href=\"https://www.linkedin.com/company-beta/13269671/\" target=\"_blank\" rel=\"nofollow noreferrer\">https://www.linkedin.com/company-beta/13269671/</a>&gt; [twitter] &lt;<a class=\"linkified\" href=\"https://twitter.com/Total_ebiz\" target=\"_blank\" rel=\"nofollow noreferrer\">https://twitter.com/Total_ebiz</a>&gt; [facebook] &lt;<a class=\"linkified\" href=\"https://www.facebook.com/totalebiz/\" target=\"_blank\" rel=\"nofollow noreferrer\">https://www.facebook.com/totalebiz/</a>&gt; [google plus] &lt;<a class=\"linkified\" href=\"https://plus.google.com/115890421231938779859\" target=\"_blank\" rel=\"nofollow noreferrer\">https://plus.google.com/115890421231938779859</a>&gt;";

            Test = $"<span style =\"font-size: 18px;\">" + Test + "</span>";

            GetFeeds();
        }

        private void GetFeeds()
        {
            
                
            FeedService.GetFeed()
                        .SubscribeOn(NewThreadScheduler.Default)
                        .ObserveOn(SynchronizationContext.Current)
                        .Subscribe((data) =>
                        {
                            YammerFeeds = data.ToList();

                        });


        }

        private void HandleException(Exception obj)
        {
            throw new NotImplementedException();
        }
    }
}
