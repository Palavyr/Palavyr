import React from "react";
import { AppBar, Toolbar, Typography, Button, makeStyles } from "@material-ui/core";
import { Link, useHistory } from "react-router-dom";
import classNames from "classnames";
import { Align } from "dashboard/layouts/positioning/Align";
import { BrandName } from "@landing/branding/BrandName";

export interface INavBar {
    openRegisterDialog: any;
    openLoginDialog: any;
}

const useStyles = makeStyles((theme) => ({
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
}));

export const NavBar = ({ openRegisterDialog, openLoginDialog }: INavBar) => {
    const cls = useStyles();
    const history = useHistory();

    return (
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
    );
};
