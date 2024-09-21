using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System;

namespace HomemadeCakes.Common
{
    public static class PJJsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        public static JsonSerializerSettings JsonSettings
        {
            get
            {
                if (_jsonSettings == null)
                {
                    _jsonSettings = CreateJsonSetting();
                }

                return _jsonSettings;
            }
        }

        public static string Serializer(object objValue, bool ingoreNullValue = false, bool igoreNotMapped = false)
        {
            return JsonConvert.SerializeObject(objValue, CreateJsonSetting(ingoreNullValue, igoreNotMapped));
        }

        public static string Serializer(object objValue, bool isLoop, CultureInfo userCulture, bool igoreNotMapped = false)
        {
            if (userCulture == null)
            {
                userCulture = CultureInfo.CurrentUICulture;
            }

            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore;
            if (isLoop)
            {
                referenceLoopHandling = ReferenceLoopHandling.Serialize;
            }

            return JsonConvert.SerializeObject(objValue, new JsonSerializerSettings
            {
                MaxDepth = 3,
                Culture = userCulture,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = referenceLoopHandling,
                Formatting = Formatting.None,
                ContractResolver = new CustomResolver(igoreNotMapped)
            });
        }

        public static TEntity Deserializer<TEntity>(string jsonValue)
        {
            if (jsonValue == null)
            {
                return default(TEntity);
            }

            return JsonConvert.DeserializeObject<TEntity>(jsonValue, JsonSettings);
        }

        public static object Deserializer(string jsonValue, Type targetType)
        {
            if (jsonValue == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(jsonValue, targetType, JsonSettings);
        }

        public static object Deserializer(string jsonValue)
        {
            if (jsonValue == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(jsonValue, JsonSettings);
        }

        public static object Deserializer(string jsonValue, bool isLoop, CultureInfo userCulture)
        {
            if (userCulture == null)
            {
                userCulture = CultureInfo.CurrentUICulture;
            }

            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore;
            if (isLoop)
            {
                referenceLoopHandling = ReferenceLoopHandling.Serialize;
            }

            return JsonConvert.DeserializeObject(jsonValue, new JsonSerializerSettings
            {
                MaxDepth = 3,
                Culture = userCulture,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = referenceLoopHandling,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                ContractResolver = new CustomResolver()
            });
        }

        public static TEntity Deserializer<TEntity>(string jsonValue, bool isLoop, CultureInfo userCulture)
        {
            if (userCulture == null)
            {
                userCulture = CultureInfo.CurrentUICulture;
            }

            ReferenceLoopHandling referenceLoopHandling = ReferenceLoopHandling.Ignore;
            if (isLoop)
            {
                referenceLoopHandling = ReferenceLoopHandling.Serialize;
            }

            return JsonConvert.DeserializeObject<TEntity>(jsonValue, new JsonSerializerSettings
            {
                MaxDepth = 3,
                Culture = userCulture,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = referenceLoopHandling,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                ContractResolver = new CustomResolver()
            });
        }

        public static bool IsNullOrEmpty(dynamic value)
        {
            return (value as JToken).IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty(this JToken token)
        {
            if (token != null && (token.Type != JTokenType.Array || token.HasValues) && (token.Type != JTokenType.Object || token.HasValues) && (token.Type != JTokenType.String || !(token.ToString() == string.Empty)))
            {
                return token.Type == JTokenType.Null;
            }

            return true;
        }

        public static JsonSerializerSettings CreateJsonSetting(bool igoreNullValue = false, bool igoreNotMapped = false)
        {
            return new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = (igoreNullValue ? NullValueHandling.Ignore : NullValueHandling.Include),
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new CustomResolver(igoreNotMapped)
            };
        }
    }

}
