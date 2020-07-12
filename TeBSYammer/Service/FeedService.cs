using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Core.Network.Api;
using Newtonsoft.Json;
using TeBSYammer.Model;
using Microsoft.CSharp;
namespace TeBSYammer.Service
{
    public class FeedService : BaseClient
    {
        public FeedService() : base(null, null, AppSettings.BaseURL, token: AppSettings.Token)
        {

        }

        public IObservable<IObservable<YammerFeed>> GetFeed()
        {
            return Observable.Create<IObservable<YammerFeed>>(async (o) =>
            {
                try
                {
                    var result = await GetAsync<string>("messages.json", null);

                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                    var JSonDataMessages = data["messages"];

                    List<YammerFeed> resultOrg = Newtonsoft.Json.JsonConvert.DeserializeObject<List<YammerFeed>>(Newtonsoft.Json.JsonConvert.SerializeObject(JSonDataMessages), new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });

                    

                    o.OnNext(resultOrg.ToObservable());
                    o.OnCompleted();
                }
                catch (Exception e)
                {
                    o.OnError(e);
                }
                return Disposable.Empty;
            });
        }
    }
}
