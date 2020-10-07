import React from "react";
import { AppBar, Toolbar, Typography, Hidden, IconButton, Button, makeStyles } from "@material-ui/core";
import { menuItems } from "./NavMenuItems";
import { Link } from "react-router-dom";
import MenuIcon from '@material-ui/icons/Menu';

export interface INavBar {
    openRegisterDialog: any;
    openLoginDialog: any;
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    mobileDrawerOpen: any;
    selectedTab: string | null;
    selectTab: any;
}

const useStyles = makeStyles({
    root: {},
    menuButton: {},
    appBar: {
        height: "6rem",
        position: "static",
        // boxShadow: theme.shadows[6],
    },
    toolbar: {
        backgroundColor: "black",
        height: "100%",
        display: "flex",
        justifyContent: "space-between"
    },
    menuButtonText: {
        fontSize: "large",
        // fontSize: theme.typography.body1.fontSize,
        // fontWeight: theme.typography.h6.fontWeight
    },
    brandText: {
        fontWeight: "bolder",
        marginLeft: "5rem",
        color: "#c7ecee"
    },
    noDecoration: {
        textDecoration: "none !important"
    }
});

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, selectedTab, selectTab }: INavBar) => {

    const classes = useStyles();

    return (
        <div className={classes.root}>
            <AppBar position="fixed" className={classes.appBar}>
                <Toolbar className={classes.toolbar}>
                    <div>
                        <Typography
                            variant="h3"
                            className={classes.brandText}
                            display="inline"
                            color="primary"
                        >
                            Palavyr
                        </Typography>

                    </div>
                    <div>
                        <Hidden mdUp>
                            <IconButton
                                className={classes.menuButton}
                                onClick={handleMobileDrawerOpen}
                                aria-label="Open Navigation"
                            >
                                <MenuIcon color="primary" />
                            </IconButton>
                        </Hidden>
                        <Hidden smDown>
                            {
                                menuItems(openRegisterDialog, openLoginDialog).map(element => {
                                    if (element.link) {
                                        return (
                                            <Link
                                                key={element.name}
                                                to={element.link}
                                                className={classes.noDecoration}
                                                onClick={handleMobileDrawerClose}
                                            >
                                                <Button
                                                    style={{color: "white"}}
                                                    size="large"
                                                    classes={{ text: classes.menuButtonText }}
                                                >
                                                    {element.name}
                                                </Button>
                                            </Link>
                                        );
                                    }
                                    return (
                                        <Button
                                            style={{color:"white"}}
                                            size="large"
                                            onClick={element.onClick}
                                            classes={{ text: classes.menuButtonText }}
                                            key={element.name}
                                        >
                                            {element.name}
                                        </Button>
                                    );
                                })
                            }
                        </Hidden>
                    </div>

                </Toolbar>
            </AppBar>
            {/* <NavigationDrawer
                menuItems={menuItems}
                anchor="right"
                open={mobileDrawerOpen}
                selectedItem={selectedTab}
                onClose={handleMobileDrawerClose}
            /> */}
        </div>
    );
};
