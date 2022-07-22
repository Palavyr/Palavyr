using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Palavyr.API.Registration.Container.MediatorModule
{
    public static class MediatorServiceFactory
    {
        public static ServiceFactory Wrap(
            ServiceFactory serviceFactory,
            IEnumerable<IMediatorServiceTypeConverter> converters)
        {
            return serviceType =>
            {
                Type NoConversion() => serviceType;
                var convertServiceType = converters
                    .Reverse()
                    .Aggregate((ConverterDelegate)NoConversion, (next, c) => () => c.Convert(serviceType, next));
                return serviceFactory(convertServiceType());
            };
        }
    }
}