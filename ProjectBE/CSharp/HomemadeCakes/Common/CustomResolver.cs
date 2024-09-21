using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Web;

namespace HomemadeCakes.Common
{
    public class CustomResolver : DefaultContractResolver
    {
        protected class HtmlEncodingValueProvider : IValueProvider
        {
            private readonly MemberInfo _memberInfo;

            public HtmlEncodingValueProvider(MemberInfo memberInfo)
            {
                _memberInfo = memberInfo;
            }

            public void SetValue(object target, object value)
            {
                string value2 = HttpUtility.HtmlDecode((string)value);
                switch (_memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        ((FieldInfo)_memberInfo).SetValue(target, value2);
                        break;
                    case MemberTypes.Property:
                        ((PropertyInfo)_memberInfo).SetValue(target, value2);
                        break;
                }
            }

            public object GetValue(object target)
            {
                string text = null;
                switch (_memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        text = ((FieldInfo)_memberInfo).GetValue(target) as string;
                        break;
                    case MemberTypes.Property:
                        text = ((PropertyInfo)_memberInfo).GetValue(target) as string;
                        break;
                }

                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                return HttpUtility.HtmlEncode(text);
            }
        }

        private bool _igoreNotMapped;

        public CustomResolver(bool igoreNotMapped = false)
        {
            _igoreNotMapped = igoreNotMapped;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
            if (_igoreNotMapped && !jsonProperty.Ignored)
            {
                NotMappedAttribute customAttribute = member.GetCustomAttribute<NotMappedAttribute>();
                jsonProperty.Ignored = customAttribute != null;
            }

            return jsonProperty;
        }
    }

}
