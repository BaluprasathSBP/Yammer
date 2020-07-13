using System;
using System.Collections.Generic;
using TeBSYammer.Model;

namespace TeBSYammer.Views.Feed
{
    public class FeedUsers
    {
        public FeedUsers()
        {
        }

        private static FeedUsers _feedUsers;
        public static FeedUsers Instance
        {
            get
            {
                if(_feedUsers== null)
                {
                    _feedUsers = new FeedUsers();
                }
                return _feedUsers;
            }
        }
        public Dictionary<string, YammerUser> Users = new Dictionary<string, YammerUser>();
        
    }
}
