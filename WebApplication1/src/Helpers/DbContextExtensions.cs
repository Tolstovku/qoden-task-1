using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Helpers
{
    public static class DbContextExtensions
    {
        public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;

                var isDbSet = setType.IsGenericType &&
                              typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition());

                if (isDbSet)
                    dbSetProperties.Add(property);
            }

            return dbSetProperties;
        }

        public static void ApplyOnModelCreatingFromAllEntities(this DbContext context, ModelBuilder builder)
        {
            var props = context.GetDbSetProperties();
            foreach (var prop in props)
            {
                var methodInfo = prop.PropertyType.GetGenericArguments()[0].GetMethod("OnModelCreating");
                methodInfo?.Invoke(null, new object[] {builder});
            }
        }

        public static void ApplySnakeCase(this DbContext context, ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                foreach (var property in entity.GetProperties())
                    property.Relational().ColumnName = property.Name.ToSnakeCase();

                foreach (var key in entity.GetKeys()) key.Relational().Name = key.Relational().Name.ToSnakeCase();

                foreach (var key in entity.GetForeignKeys())
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();

                foreach (var index in entity.GetIndexes())
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
            }
        }
    }
}