import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { List, ListItem, ListItemIcon, Drawer, IconButton, Typography, withWidth, isWidthUp, Toolbar, useTheme, makeStyles, Button } from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";
import { IHaveWidth } from "@Palavyr-Types";
import { DrawerProps } from "@material-ui/core";
import classNames from "classnames";
import { LineSpacer } from "@common/components/typography/LineSpacer";

const useStyles = makeStyles<{}>((theme: any) => ({
    closeIcon: {
        marginRight: theme.spacing(0.5),
    },
    headSection: {
        width: 200,
    },
    blackList: {
        backgroundColor: theme.palette.primary.light,
        height: "100%",
        display: "flex",
        flexDirection: "column",
        textAlign: "center",
    },
    noDecoration: {
        textDecoration: "none !important",
    },
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
        "&:hover": {
            color: theme.palette.success.dark,
            backgroundColor: theme.palette.common.white,
        },
    },
    loginButton: {
        border: "0px",
    },
    menuButtonText: {
        color: theme.palette.common.white,
        "&:hover": {
            color: theme.palette.success.main,
        },
    },

    navButtons: {
        display: "flex",
        justifyContent: "space-evenly",
        verticalAlign: "middle",
    },
    drawer: {
        height: "100%",
    },
}));

export interface NavigationDrawerProps extends IHaveWidth, DrawerProps {
    open: boolean;
    onClose: any;
    openLoginDialog: any;
    openRegisterDialog: any;
}

export const NavigationDrawer = withWidth()(({ width, open, onClose, anchor, openLoginDialog, openRegisterDialog }: NavigationDrawerProps) => {
    const cls = useStyles();
    const theme = useTheme();
    useEffect(() => {
        window.onresize = () => {
            if (isWidthUp("sm", width) && open) {
                onClose();
            }
        };
    }, [width, open, onClose]);
    return (
        <Drawer className={cls.drawer} variant="temporary" open={open} onClose={onClose} anchor={anchor}>
            <Toolbar className={cls.headSection}>
                <ListItem
                    style={{
                        paddingTop: theme.spacing(0),
                        paddingBottom: theme.spacing(0),
                        height: "100%",
                        justifyContent: anchor === "left" ? "flex-start" : "flex-end",
                    }}
                >
                    <ListItemIcon className={cls.closeIcon}>
                        <IconButton onClick={onClose} aria-label="Close Navigation">
                            <CloseIcon color="primary" />
                        </IconButton>
                    </ListItemIcon>
                </ListItem>
            </Toolbar>
            <List className={cls.blackList}>
                <Button disableElevation variant="outlined" size="small" onClick={openLoginDialog} className={classNames(cls.menuButtonText, cls.loginButton)} key="Login">
                    <Typography variant="h6" className={cls.menuButtonText}>
                        Login
                    </Typography>
                </Button>
                <LineSpacer numLines={1} />
                <Button disableElevation variant="contained" size="medium" onClick={openRegisterDialog} className={cls.newAccountButton} key="Register">
                    <Typography variant="h6">Try For Free</Typography>
                </Button>
                <LineSpacer numLines={1} />
                <Link key="Home" to="/" className={cls.noDecoration}>
                    <span>
                        <Typography variant="h6" className={cls.menuButtonText}>
                            Home
                        </Typography>
                    </span>
                </Link>
                <LineSpacer numLines={1} />
                <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                    <span>
                        <Typography variant="h6" className={cls.menuButtonText}>
                            Tutorial
                        </Typography>
                    </span>
                </Link>
                <LineSpacer numLines={1} />
                <Link key="Blog" to="/blog" className={cls.noDecoration}>
                    <span>
                        <Typography variant="h6" className={cls.menuButtonText}>
                            Blog
                        </Typography>
                    </span>
                </Link>
            </List>
        </Drawer>
    );
});
