using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace SocialButterFlAi.Data.Identity
{
    public class EntityHelper
    {
        private List<Type> _TypeNames;

        public EntityHelper()
        {
            _TypeNames = new List<Type>();
        }

        /// <summary>
        /// EntityBuilder
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="parentType"></param>
        /// <param name="baseType"></param>
        public void EntityBuilder(
            ModelBuilder modelBuilder,
            Type parentType,
            Type? baseType
        )
        {
            try
            {
                var currentType = baseType ?? parentType;
                if (!_TypeNames.Contains(currentType))
                {

                    var allProps = currentType.GetProperties();
                    var dbProperties = allProps
                        .Where(p => p.PropertyType.IsSubclassOf(typeof(BaseEntity)))
                        .Select((p => p.PropertyType));

                    // Add the IEnumerable<BaseEntityType> as BaseEntityType
                    var dbPropertiesWEnumerables = dbProperties.Concat(
                        allProps
                            .Where(t => t.PropertyType.IsGenericType
                                        && t.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            )
                            .Select(t => t.PropertyType.GetGenericArguments()[0])// get the generic type of the IEnumerable
                            .Where(p => !p.FullName.Contains("System")) // exclude system types like string, int, etc. leave only custom types defined in the project
                    ).ToArray();

                    AdjustProperty(modelBuilder, parentType, currentType);

                    _TypeNames.Add(currentType);

                    foreach (var dbProperty in dbPropertiesWEnumerables)
                        EntityBuilder(modelBuilder, currentType, dbProperty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parentType"></param>
        /// <param name="baseType"></param>
        public void AdjustProperty(
            ModelBuilder builder,
            Type parentType,
            Type baseType
        )
        {
            try
            {
                var props = baseType.GetProperties();
                // for each property, add the configuration for the property type
                props.ToList().ForEach(pi =>
                {
                    var propertyType = pi.PropertyType;

                    //to store enums as strings in the database instead of ints
                    if (propertyType.IsEnum)
                    {
                        builder.Entity(baseType)
                            .Property(pi.Name) //property name
                            .HasConversion<string>(); // convert to string
                    }
                    // to store IEnumerable<EnumType> as strings in the database instead of ints
                    else if (propertyType.IsGenericType
                            && propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            && propertyType.GetGenericArguments()[0].IsEnum
                    )
                    {
                        // var enumType = propertyType.GetGenericArguments()[0];
                    }
                    // Add additional type converters here if we need to
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}