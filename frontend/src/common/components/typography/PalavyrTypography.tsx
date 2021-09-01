import { makeStyles, Theme, Typography, TypographyProps } from "@material-ui/core";
import React from "react";

export interface PalavyrTextProps extends TypographyProps {
    children: React.ReactNode | string | number;
}

export const PalavyrText = ({ children, ...rest }) => {
    return <Typography {...rest}>{children}</Typography>;
};
