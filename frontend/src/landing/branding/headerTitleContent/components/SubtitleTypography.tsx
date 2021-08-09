import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles((theme) => ({
    secondaryText: {
        color: theme.palette.success.dark,
    },
}));

export interface SubtitleTypographyProps {
    children: React.ReactNode;
}

export const SubtitleTypography = ({ children }: SubtitleTypographyProps) => {
    const cls = useStyles();
    return (
        <Typography align="center" variant="h6" className={cls.secondaryText}>
            {children}
        </Typography>
    );
};
