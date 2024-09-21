using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HomemadeCakes.DAL.MogoDB.Conventions
{
    public class EFAttributeConventionPack : IConventionPack
    {
        private class AttributeConvention : ConventionBase, IClassMapConvention, IConvention
        {
            public void Apply(BsonClassMap classMap)
            {
                IgnoreMembersWithNotMappedAttribute(classMap);
            }

            private void IgnoreMembersWithNotMappedAttribute(BsonClassMap classMap)
            {
                foreach (BsonMemberMap item in classMap.DeclaredMemberMaps.ToList())
                {
                    if (item.MemberInfo.GetCustomAttributes(inherit: false).OfType<NotMappedAttribute>().FirstOrDefault() != null)
                    {
                        classMap.UnmapMember(item.MemberInfo);
                    }
                }
            }
        }

        private static readonly EFAttributeConventionPack __attributeConventionPack = new EFAttributeConventionPack();

        private readonly AttributeConvention _attributeConvention;

        public static IConventionPack Instance => __attributeConventionPack;

        public IEnumerable<IConvention> Conventions
        {
            get
            {
                yield return _attributeConvention;
            }
        }

        private EFAttributeConventionPack()
        {
            _attributeConvention = new AttributeConvention();
        }
    }

}
