import * as React from 'react';
import { BrandingIcon } from '@common/icons/BrandingIcon';
import { IconSizes } from '@common/icons/IconTypes';
import { Typography } from '@material-ui/core';

export interface IIconBox {
    IconJsx: React.FunctionComponent<React.SVGAttributes<SVGElement>>;
    iconTitle: string;
    iconSize: number;
    children: React.ReactNode;
}

// const safeFontSize = (size: IconSizes) => {
//     if (typeof size === "string") {
//         if (size.toLowerCase() === 'small') {
//             return "10%";
//         } else if (size.toLowerCase() === 'medium') {
//             return "32%";
//         } else if (size.toLowerCase() === 'large') {
//             return "54%";
//         } else if (size.toLowerCase() === 'xlarge') {
//             return "76%";
//         } else if (size.toLocaleLowerCase() === "xxlarge") {
//             return "92%";
//         }
//         return "32%";
//     } else {
//         return (size.toString() + "%")
//     }
// };

export const IconBox = ({ IconJsx, iconSize, iconTitle, children }: IIconBox) => {

    return (
        <div className="text-center">
            <BrandingIcon IconJsx={IconJsx} iconSize={iconSize.toString() + "%"} />
            <Typography component="span" variant="h4">{iconTitle}</Typography>
            <Typography component="span">{children}</Typography>
        </div>
    );
};
