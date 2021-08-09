import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    primaryText: {
        color: theme.palette.success.main,
    },
}));

export interface TitleTypographyProps {
    children: React.ReactNode;
}

export const TitleTypography = ({ children }: TitleTypographyProps) => {
    const cls = useStyles();
    return (
        <Typography align="center" variant="h2" className={cls.primaryText}>
            {children}
        </Typography>
    );
};
