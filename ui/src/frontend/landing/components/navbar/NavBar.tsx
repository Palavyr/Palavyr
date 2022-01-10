import React from "react";
import { AppBar, Toolbar, makeStyles } from "@material-ui/core";
import { BrandName } from "@landing/branding/BrandName";
import { NavigationDrawer } from "./NavigationDrawer";

const useStyles = makeStyles(theme => ({
    clear: {
        border: "0px solid white",
    },
    appBar: {
        position: "sticky",
        boxShadow: "0 0 black",
        marginTop: "2rem",
        marginBottom: "2rem",
    },
    toolbar: {
        height: "100%",
        display: "flex",
        justifyContent: "center",
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
