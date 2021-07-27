import { makeStyles } from "@material-ui/core";
import React from "react";
import { NavBar } from "../navbar/NavBar";

const useStyles = makeStyles((theme) => ({
    container: {
        paddingLeft: "15%",
        paddingRight: "15%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        backgroundColor: theme.palette.primary.dark,
        paddingBottom: "3rem",
        textAlign: "center",
    },
    card: {
        boxShadow: "0 0 white",
        background: "none",
        border: "none",
        paddingTop: "2rem",
        paddingBottom: "2rem",
        minWidth: "60%",
    },
}));

interface IHeader {
    openRegisterDialog: any;
    openLoginDialog: any;
    children?: React.ReactNode;
}

export const Header = ({ openRegisterDialog, openLoginDialog, children }: IHeader) => {
    const cls = useStyles();
    return (
        <div className={cls.container}>
            <NavBar openRegisterDialog={openRegisterDialog} openLoginDialog={openLoginDialog} />
            {children}
        </div>
    );
};
