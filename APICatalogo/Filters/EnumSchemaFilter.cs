using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace APICatalogo.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                foreach (var name in Enum.GetNames(context.Type))
                {
                    var enumMember = context.Type.GetMember(name).First();
                    var descriptionAttribute = enumMember.GetCustomAttribute<DescriptionAttribute>();
                    var description = descriptionAttribute != null ? descriptionAttribute.Description : name;
                    schema.Enum.Add(new OpenApiString(description));
                }
            }
        }
    }
}
