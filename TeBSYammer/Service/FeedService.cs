using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Core.Network.Api;
using Newtonsoft.Json;
using TeBSYammer.Model;
using Microsoft.CSharp;
using TeBSYammer.Views.Feed;
using System.Linq;

namespace TeBSYammer.Service
{
    public class FeedService : BaseClient
    {
        public FeedService() : base(null, null, AppSettings.BaseURL, token: AppSettings.Token)
        {

        }

        public IObservable<IEnumerable<YammerFeed>> GetFeed()
        {
            return Observable.Create<IEnumerable<YammerFeed>>(async (o) =>
            {
                try
                {
                    var result = await GetAsync<string>("messages.json", null);

                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                    var JSonDataMessages = data["messages"];

                    List<YammerFeed> resultOrg = Newtonsoft.Json.JsonConvert.DeserializeObject<List<YammerFeed>>(Newtonsoft.Json.JsonConvert.SerializeObject(JSonDataMessages), new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });

                    var feedUsers = FeedUsers.Instance.Users;

                    foreach (var feed in resultOrg)
                    {
                        var userId = feed.CreatedBy;

                        if (feedUsers.ContainsKey(userId))
                        {
                            feed.User = new YammerUser();
                            feed.User.FullName = feedUsers[userId].FullName;
                            feed.User.ImageUrl = feedUsers[userId].ImageUrl;
                        }
                        else
                        {
                            var UserResult = await GetAsync<YammerUser>($"users/{userId}.json", null);
                            if (UserResult != null)
                            {
                                feed.User = new YammerUser();
                                feed.User.FullName = UserResult.FullName;
                                feed.User.ImageUrl = UserResult.ImageUrl;
                                if (!feedUsers.ContainsKey(userId))
                                {
                                    FeedUsers.Instance.Users.Add(userId, UserResult);
                                }
                            }
                        }

                        var feedGroups = FeedUsers.Instance.Users;
                        if (feedGroups.ContainsKey(userId))
                        {
                            feed.Group = new YammerGroup();
                            feed.User.FullName = feedUsers[userId].FullName;
                            feed.User.ImageUrl = feedUsers[userId].ImageUrl;
                        }
                        else
                        {
                            var UserResult = await GetAsync<YammerUser>($"users/{userId}.json", null);
                            if (UserResult != null)
                            {
                                feed.User = new YammerUser();
                                feed.User.FullName = UserResult.FullName;
                                feed.User.ImageUrl = UserResult.ImageUrl;
                                if (!feedUsers.ContainsKey(userId))
                                {
                                    FeedUsers.Instance.Users.Add(userId, UserResult);
                                }
                            }
                        }
                    }
                    o.OnNext(resultOrg);
                    o.OnCompleted();
                }
                catch (Exception e)
                {
                    o.OnError(e);
                }
                return Disposable.Empty;
            });
        }


        public IObservable<YammerUser> GetUser(string userId)
        {
            return Observable.Create<YammerUser>(async (o) =>
            {
                var result = await GetAsync<YammerUser>($"users/{userId}.json", null);
                o.OnNext(result);
                o.OnCompleted();
            });
        }

        public IObservable<YammerUser> GetGroup(string groupId)
        {
            return Observable.Create<YammerUser>(async (o) =>
            {          
                var result = await GetAsync<YammerUser>($"groups/{groupId}.json", null);
                o.OnNext(result);
                o.OnCompleted();
            });
        }
    }
}

