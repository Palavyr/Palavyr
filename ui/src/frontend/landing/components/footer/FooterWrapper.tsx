import React from "react";
import { makeStyles } from "@material-ui/core";

export interface IFooterWrapper {
    children: React.ReactNode;
    backgroundColor: string;
}

const useStyles = makeStyles((theme) => ({
    wrapper: {
        paddingLeft: "12%",
        paddingRight: "12%",
        paddingTop: "1rem",
        paddingBottom: "1rem",
        color: theme.palette.common.white,
        display: "flex",
        justifyContent: "space-between",
    },
}));

export const FooterWrapper = ({ backgroundColor, children }: IFooterWrapper) => {
    const cls = useStyles();
    return (
        <footer className={cls.wrapper} style={{ backgroundColor }}>
            {children}
        </footer>
    );
};
