using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GustoExpress.Web.Modelbinder
{
    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(Decimal) || context.Metadata.ModelType == typeof(Decimal?))
            {
                return new DecimalModelBinder(context.Metadata);
            }

            return null;
        }
    }
}
