import * as React from 'react';
import { BrandingIcon } from '@common/icons/BrandingIcon';
import { SimpleIconTypes, IconSizes } from '@common/icons/IconTypes';
import { Typography } from '@material-ui/core';


export interface IIconBox {
    iconType: SimpleIconTypes;
    iconTitle: string;
    iconColor?: string;
    iconSize?: number | IconSizes;
    children: React.ReactNode;
}

const safeFontSize = (size: IconSizes) => {
    if (size.toLowerCase() === 'small') {
        return 10;
    } else if (size.toLowerCase() === 'medium') {
        return 32
    } else if (size.toLowerCase() === 'large') {
        return 54;
    } else if (size.toLowerCase() === 'xlarge') {
        return 76;
    } else if (size.toLocaleLowerCase() === "xxlarge") {
        return 92;
    }
    return 32;
};

const defaultIconSize = 20;

export const IconBox = ({ iconType, iconTitle, iconSize, iconColor, children }: IIconBox) => {

    var size = iconSize === undefined ? defaultIconSize : (typeof iconSize === "number" ? iconSize : safeFontSize(iconSize))
    const color = iconColor ?? "lightblue";

    return (
        <div className="text-center">
            <BrandingIcon iconType={iconType} iconColor={color} iconSize={size} />
            <Typography variant="h3">{iconTitle}</Typography>
            <Typography>{children}</Typography>
        </div>
    );
};
