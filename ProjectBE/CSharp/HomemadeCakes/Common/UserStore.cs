using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using HomemadeCakes.Common.Config;

namespace HomemadeCakes.Common
{
    public static class UserStore
    {
        private static object objLock = new object();

        private static string regionUser = "tkurs";

        public static PJUser SA => new PJUser
        {
            UserID = "sa",
            BUID = "EASY",
            Language = PJConfig.Settings.Language,
            FirstName = "Administrator",
            LastName = "System",
            UserName = "System Administrator",
            SecurityKey = default(Guid).ToString(),
            Token = default(Guid).ToString(),
            Administrator = true,
            FunctionAdmin = true,
            SystemAdmin = true,
            NeverExpire = true
        };

        public static void Set(PJUser user)
        {
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Language))
                {
                    user.Language = PJConfig.Settings.Language;
                }

                user.Logon = DateTime.Now;
                if (user.ExpireOn == DateTime.MinValue)
                {
                    user.ExpireOn = DateTime.Now.AddDays(1.0);
                }

                Dictionary<string, PJUser> loginUsers = GetLoginUsers(user.UserID);
                if (loginUsers.Count > 0 && loginUsers.ContainsKey(user.SecurityKey))
                {
                    loginUsers[user.SecurityKey] = user;
                }
                else
                {
                    loginUsers.Add(user.SecurityKey, user);
                }

                SetCaches(user.UserID, loginUsers);
            }
        }

        public static PJUser Get(string userid, string securityKey)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(securityKey))
            {
                return null;
            }

            Dictionary<string, PJUser> loginUsers = GetLoginUsers(userid);
            if (loginUsers.Count == 0 || !loginUsers.ContainsKey(securityKey))
            {
                return null;
            }

            PJUser PJUser = loginUsers[securityKey];
            if (PJUser.NeverExpire && PJUser.ExpireOn <= new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Today.Hour, 0, 0))
            {
                loginUsers.Remove(securityKey);
                PJUser = null;
                SetCaches(userid, loginUsers);
            }

            return PJUser;
        }

        public static void Remove(string userid, string securityKey)
        {
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(securityKey))
            {
                Dictionary<string, PJUser> loginUsers = GetLoginUsers(userid);
                if (loginUsers.Count != 0 && loginUsers.ContainsKey(securityKey))
                {
                    loginUsers.Remove(securityKey);
                    SetCaches(userid, loginUsers);
                }
            }
        }

        public static void RemoveAll()
        {
            CacheManager.Distributed.RemoveRegionAsync(regionUser);
        }

        public static IList<PJUser> GetSessions(string userid, string dbName, string type = "")
        {
            Dictionary<string, PJUser> loginUsers = GetLoginUsers(userid);
            List<PJUser> lstReturns = new List<PJUser>();
            ConcurrentDictionary<string, PJUser> dic = new ConcurrentDictionary<string, PJUser>();
            Parallel.ForEach(loginUsers, delegate (KeyValuePair<string, PJUser> x)
            {
                if (x.Value != null)
                {
                    PJUser value = x.Value;
                    if (value.NeverExpire || value.ExpireOn > DateTime.Now)
                    {
                        dic.TryAdd(value.SecurityKey, value);
                        if (value.ConnectionName == dbName && (type == "" || (type == "1" && !value.IsMobile) || (type == "2" && value.IsMobile)))
                        {
                            lstReturns.Add(value);
                        }
                    }
                }
            });
            SetCaches(userid, dic.ToDictionary((KeyValuePair<string, PJUser> x) => x.Key, (KeyValuePair<string, PJUser> y) => y.Value));
            return lstReturns;
        }

        private static Dictionary<string, PJUser> GetLoginUsers(string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return new Dictionary<string, PJUser>();
            }

            userid = userid.ToLower();
            Dictionary<string, PJUser> dictionary = CacheManager.Distributed.Get<Dictionary<string, PJUser>>(userid, regionUser);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, PJUser>();
                lock (objLock)
                {
                    CacheManager.Distributed.Add(userid, dictionary, regionUser);
                }
            }

            return dictionary;
        }

        private static void SetCaches(string userid, Dictionary<string, PJUser> loginUsers)
        {
            if (string.IsNullOrEmpty(userid) || loginUsers == null)
            {
                return;
            }

            lock (objLock)
            {
                CacheManager.Distributed.Add(userid.ToLower(), loginUsers, regionUser);
            }
        }
    }
}
