using System;
using MediatR;
using Palavyr.Core.Handlers.PricingStrategyHandlers;

namespace Palavyr.API.Registration.Container.MediatorModule
{
    public class TestConverter : IMediatorServiceTypeConverter
    {
        public Type Convert(Type sourceType, ConverterDelegate next)
        {
            var isRequestHandler = sourceType.IsGenericType &&
                                   (sourceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
            // || sourceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>));
            if (!isRequestHandler) return next();

            var requestType = sourceType.GenericTypeArguments[0];
            var shouldConvertType = requestType.IsGenericType &&
                                    (
                                        requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowsRequest<,>)
                                        || requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowTemplateRequest<,>)
                                        || requestType.GetGenericTypeDefinition() == typeof(DeletePricingStrategyTableRequest<,>)
                                        || requestType.GetGenericTypeDefinition() == typeof(SavePricingStrategyTableRequest<,>)
                                    );
            if (!shouldConvertType) return next();
            var returnTypeA = requestType.GenericTypeArguments[0];
            var returnTypeB = requestType.GenericTypeArguments[1];

            if (requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowsRequest<,>))
            {
                return typeof(GetPricingStrategyTableRowsHandler<,>).MakeGenericType(returnTypeA, requestType.GenericTypeArguments[1]);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowTemplateRequest<,>))
            {
                return typeof(GetPricingStrategyTableRowTemplateHandler<,>).MakeGenericType(returnTypeA, requestType.GenericTypeArguments[1]);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(DeletePricingStrategyTableRequest<,>))
            {
                return typeof(DeletePricingStrategyTableHandler<,>).MakeGenericType(returnTypeA);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(SavePricingStrategyTableRequest<,>))
            {
                return typeof(SavePricingStrategyTableHandler<,>).MakeGenericType(returnTypeA, requestType.GenericTypeArguments[1]);
            }
            else
            {
                throw new Exception($"Pricing strategy handler {requestType.GetGenericTypeDefinition().ToString()} not found.");
            }
        }
    }
}