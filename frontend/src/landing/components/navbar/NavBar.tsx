import React from "react";
import { AppBar, Toolbar, Typography, Button, makeStyles, IconButton, Hidden, Divider } from "@material-ui/core";
import { Link } from "react-router-dom";
import classNames from "classnames";
import { Align } from "dashboard/layouts/positioning/Align";
import { BrandName } from "@landing/branding/BrandName";
import MenuIcon from "@material-ui/icons/Menu";
import { NavigationDrawer } from "./NavigationDrawer";
import { memo } from "react";

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
        justifyContent: "space-between",
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

export const NavBar = memo(({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen }: INavBar) => {
    const cls = useStyles();
    return (
        <>
            <Hidden mdUp>
                <div className={cls.smallContainer}>
                    <Align>
                        <BrandName />
                    </Align>
                    <div>
                        <IconButton className={cls.menuButton} onClick={handleMobileDrawerOpen} aria-label="Open Navigation">
                            <MenuIcon className={cls.menuButton} color="primary" />
                        </IconButton>
                    </div>
                </div>
            </Hidden>
            <Hidden smDown>
                <AppBar position="fixed" className={cls.appBar} color="transparent" classes={{ root: cls.clear }}>
                    <Toolbar className={cls.toolbar}>
                        <BrandName />
                        <div className={cls.navButtons}>
                            <Align verticalCenter>
                                <Button disableElevation variant="outlined" size="small" onClick={openLoginDialog} className={classNames(cls.menuButtonText, cls.loginButton)} key="Login">
                                    <Typography variant="h6" className={cls.menuButtonText}>
                                        Login
                                    </Typography>
                                </Button>
                                <Button disableElevation variant="contained" size="medium" onClick={openRegisterDialog} className={cls.newAccountButton} key="Register">
                                    <Typography variant="h6">Try For Free</Typography>
                                </Button>
                                <div style={{ marginRight: "1.5rem" }}>
                                    <Link key="Home" to="/" className={cls.noDecoration}>
                                        <span>
                                            <Typography variant="h6" className={cls.menuButtonText}>
                                                Home
                                            </Typography>
                                        </span>
                                    </Link>
                                </div>
                                <div style={{ marginRight: "1.5rem" }}>
                                    <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                                        <span>
                                            <Typography variant="h6" className={cls.menuButtonText}>
                                                Tutorial
                                            </Typography>
                                        </span>
                                    </Link>
                                </div>
                                <Link key="Blog" to="/blog" className={cls.noDecoration}>
                                    <span>
                                        <Typography variant="h6" className={cls.menuButtonText}>
                                            Blog
                                        </Typography>
                                    </span>
                                </Link>
                            </Align>
                        </div>
                    </Toolbar>
                </AppBar>
            </Hidden>
            <NavigationDrawer anchor="right" open={mobileDrawerOpen} onClose={handleMobileDrawerClose} openRegisterDialog={openRegisterDialog} openLoginDialog={openLoginDialog} />
        </>
    );
});
