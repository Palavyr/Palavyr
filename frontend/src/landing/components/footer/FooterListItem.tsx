import { makeStyles, Typography } from "@material-ui/core";
import React from "react";

export interface IFooterListItem {
    children: React.ReactNode;
    onClick?: any;
}

const useStyles = makeStyles((theme) => ({
    menuButtonText: {
        color: theme.palette.common.white,
        "&:hover": {
            color: theme.palette.success.main,
            cursor: "pointer",
        },
    },
}));

export const FooterListItem = ({ children, onClick = () => null }: IFooterListItem) => {
    const cls = useStyles();
    return (
        <li>
            <Typography variant="body1" onClick={onClick} className={cls.menuButtonText}>
                {children}
            </Typography>
        </li>
    );
};
