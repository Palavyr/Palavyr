﻿namespace Palavyr.Core.Models.Configuration.Constant
{
    public enum NodeTypeCodeEnum
    {
        I = 0, 
        
        // ReSharper disable once InconsistentNaming
        II = 1, 
        
        // ReSharper disable once InconsistentNaming
        III = 2, 
        
        // ReSharper disable once InconsistentNaming
        IV = 3,

        V = 4,
        
        // ReSharper disable once InconsistentNaming
        VI = 5, // Anabranch

        // ReSharper disable once InconsistentNaming
        VII = 6, // Loopback Anchor
        
        // ReSharper disable once InconsistentNaming
        VIII = 7, // Loopback terminal
        
        // ReSharper disable once InconsistentNaming
        IX = 8, // Image Node Type

        X = 9, // Multioption with frozen set and not editable (used with PricingStrategy nodes like select category and nested categories 
        
        XI = 10 // Where you set the value options given by the nodeOption (pricing strategy node)
    }
}