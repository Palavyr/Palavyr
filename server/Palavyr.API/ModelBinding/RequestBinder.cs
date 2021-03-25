using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Palavyr.API.ModelBinding
{

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class FromRequest : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
            new[] { BindingSource.Path, BindingSource.Query },
            nameof(FromRequest));
    }
}