using System;
using MediatR;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Handlers.PricingStrategyHandlers;

namespace Palavyr.API.Registration.Container.MediatorModule
{
    public class TestConverter : IMediatorServiceTypeConverter
    {
        public Type Convert(Type sourceType, ConverterDelegate next)
        {
            if (!IsItAHandlerType(sourceType)) return next();

            var requestType = sourceType.GenericTypeArguments[0];
            if (!ShouldConvertType(requestType)) return next();

            GetGenericTypesFromTheRequest(requestType, out var tEntity, out var tResource, out var tCompiler);

            return MakeGenericType(requestType, tEntity, tResource, tCompiler);
        }

        private static Type MakeGenericType(Type requestType, Type tEntity, Type tResource, Type tCompiler)
        {
            if (requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowsRequest<,,>))
            {
                return typeof(GetPricingStrategyTableRowsHandler<,,>).MakeGenericType(tEntity, tResource, tCompiler);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowTemplateRequest<,,>))
            {
                return typeof(GetPricingStrategyTableRowTemplateHandler<,,>).MakeGenericType(tEntity, tResource, tCompiler);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(DeletePricingStrategyTableRequest<,,>))
            {
                return typeof(DeletePricingStrategyTableHandler<,,>).MakeGenericType(tEntity, tResource, tCompiler);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(SavePricingStrategyTableRequest<,,>))
            {
                return typeof(SavePricingStrategyTableHandler<,,>).MakeGenericType(tEntity, tResource, tCompiler);
            }
            else if (requestType.GetGenericTypeDefinition() == typeof(CreatePricingStrategyTableRequest<,,>))
            {
                return typeof(CreatePricingStrategyTableHandler<,,>).MakeGenericType(tEntity, tResource, tCompiler);
            }
            else
            {
                throw new Exception($"Pricing strategy handler {requestType.GetGenericTypeDefinition()} not found. Add it to the TestConverter.cs");
            }
        }

        private static void GetGenericTypesFromTheRequest(Type requestType, out Type returnTypeA, out Type returnTypeB, out Type returnTypeC)
        {
            returnTypeA = requestType.GenericTypeArguments[0];
            returnTypeB = requestType.GenericTypeArguments[1];
            returnTypeC = requestType.GenericTypeArguments[2];
        }

        private static bool ShouldConvertType(Type requestType)
        {
            return requestType.IsGenericType &&
                   (
                       requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowsRequest<,,>)
                       || requestType.GetGenericTypeDefinition() == typeof(GetPricingStrategyTableRowTemplateRequest<,,>)
                       || requestType.GetGenericTypeDefinition() == typeof(DeletePricingStrategyTableRequest<,,>)
                       || requestType.GetGenericTypeDefinition() == typeof(SavePricingStrategyTableRequest<,,>)
                       || requestType.GetGenericTypeDefinition() == typeof(CreatePricingStrategyTableRequest<,,>)
                   );
        }

        private static bool IsItAHandlerType(Type sourceType)
        {
            return sourceType.IsGenericType &&
                   (sourceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
        }
    }
}