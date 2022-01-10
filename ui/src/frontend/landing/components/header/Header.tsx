import { makeStyles } from "@material-ui/core";
import React, { useEffect } from "react";
import { NavBar } from "../navbar/NavBar";

const useStyles = makeStyles(theme => ({
    container: {
        display: "flex",
        justifyContent: "center",
        backgroundColor: theme.palette.primary.dark,
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
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    mobileDrawerOpen: any;
    children?: React.ReactNode;
}

export const Header = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, children }: IHeader) => {
    const cls = useStyles();

    return (
        <div className={cls.container}>
            <NavBar
                openRegisterDialog={openRegisterDialog}
                openLoginDialog={openLoginDialog}
                handleMobileDrawerOpen={handleMobileDrawerOpen}
                handleMobileDrawerClose={handleMobileDrawerClose}
                mobileDrawerOpen={mobileDrawerOpen}
            />
            {children}
        </div>
    );
};
