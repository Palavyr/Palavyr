import { makeStyles, Typography } from "@material-ui/core";
import React from "react";


const useStyles = makeStyles<{}>((theme: any) => ({
    primaryText: {
        color: theme.palette.success.main,
    },
}));

export interface TitleTypographyProps {
    children: React.ReactNode;
    display?: "initial" | "block" | "inline" | undefined;
}

export const TitleTypography = ({ children, display = "inline" }: TitleTypographyProps) => {
    const cls = useStyles();
    return (
        <Typography display={display} align="center" variant="h1" className={cls.primaryText}>
            {children}
        </Typography>
    );
};
