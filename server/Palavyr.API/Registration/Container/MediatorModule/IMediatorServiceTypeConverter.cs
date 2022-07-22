using System;

namespace Palavyr.API.Registration.Container.MediatorModule
{
    public interface IMediatorServiceTypeConverter
    {
        Type Convert(Type sourceType, ConverterDelegate next);
    }
}