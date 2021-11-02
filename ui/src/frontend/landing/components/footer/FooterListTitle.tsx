import { Typography } from "@material-ui/core";
import React from "react";

export interface IFooterListTitle {
    children: React.ReactNode;
}

export const FooterListTitle = ({ children }: IFooterListTitle) => {
    return (
        <li>
            <Typography display="block" variant="h4">
                {children}
            </Typography>
        </li>
    );
};
