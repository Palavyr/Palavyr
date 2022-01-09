import React from "react";
import { AppBar, Toolbar, makeStyles, IconButton, Hidden } from "@material-ui/core";
import { Align } from "@common/positioning/Align";
import { BrandName } from "@landing/branding/BrandName";
import MenuIcon from "@material-ui/icons/Menu";
import { NavigationDrawer } from "./NavigationDrawer";

const useStyles = makeStyles(theme => ({
    clear: {
        border: "0px solid white",
    },
    appBar: {
        marginTop: "1.5rem",
        height: "6rem",
        position: "sticky",
        boxShadow: "0 0 black",
        marginBottom: "3rem",
    },
    toolbar: {
        height: "100%",
        display: "flex",

        justifyContent: "center",
    },
    newAccountButton: {
        color: theme.palette.common.white,
        backgroundColor: theme.palette.success.dark,
        marginRight: "1rem",
        "&:hover": {
            color: theme.palette.success.dark,
            backgroundColor: theme.palette.common.white,
        },
    },
    loginButton: {
        marginRight: "1rem",
        border: "0px",
    },
    menuButton: {
        color: "white",
        border: "white",
    },
    menuButtonText: {
        color: theme.palette.common.white,
        "&:hover": {
            color: theme.palette.success.main,
        },
    },

    noDecoration: {
        textDecoration: "none !important",
    },
    navButtons: {
        display: "flex",
        justifyContent: "space-evenly",
        verticalAlign: "middle",
    },
    smallContainer: {
        marginTop: "2rem",
        marginBotton: "1rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        textAlign: "center",
        width: "100%",
    },
}));

export interface INavBar {
    openRegisterDialog: any;
    openLoginDialog: any;
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    mobileDrawerOpen: any;
}

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen }: INavBar) => {
    const cls = useStyles();
    return (
        <>
            <AppBar position="fixed" className={cls.appBar} color="transparent" classes={{ root: cls.clear }}>
                <Toolbar className={cls.toolbar}>
                    <BrandName />
                </Toolbar>
            </AppBar>
            <NavigationDrawer anchor="right" open={mobileDrawerOpen} onClose={handleMobileDrawerClose} openRegisterDialog={openRegisterDialog} openLoginDialog={openLoginDialog} />
        </>
    );
};
