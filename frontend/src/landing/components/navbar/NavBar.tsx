import React from "react";
import { AppBar, Toolbar, Typography, Hidden, IconButton, Button, makeStyles } from "@material-ui/core";
import { menuItems } from "./NavMenuItems";
import { Link } from "react-router-dom";

export interface INavBar {
    openRegisterDialog: any;
    openLoginDialog: any;
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    mobileDrawerOpen: any;
    selectedTab: string | null;
    setSelectedTab: any;
}

const useStyles = makeStyles({
    menuButton: {

    },
    clear: {
        border: "0px solid white",

    },
    appBar: {
        height: "6rem",
        position: "sticky",
        boxShadow: "0 0 black",
        marginBottom: "3rem"
    },
    toolbar: {
        height: "100%",
        display: "flex",
        justifyContent: "space-between"
    },
    menuButtonText: {
        fontSize: "large",
        color: "white",
        backgroundColor: "#3e5f82",
        marginRight: "1rem",
        '&:hover': {
            backgroundColor: "white",
            color: "#3e5f82"
        }
    },
    brandText: {
        // color: "black",
        fontSize: 64,
        fontWeight: "bolder",
        color: "black"
    },
    brandTextSmall: {
        fontWeight: "bolder",
        color: "black"
    },
    noDecoration: {
        textDecoration: "none !important"
    },
    logowrap: {
        border: "1px solid black",
        display: "flex"
    },
    logo: {
        position: "absolute",
        border: "2px solid black",
        height: "150px",
        width: "150px"
    }
});

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, selectedTab, setSelectedTab }: INavBar) => {

    const classes = useStyles();

    return (
        <AppBar position="fixed" className={classes.appBar} color="transparent" classes={{ root: classes.clear }}>
            <Toolbar className={classes.toolbar}>
                <Typography variant="body2" className={classes.brandText} display="inline" >
                    Palavyr
                </Typography>
                <Typography variant="body2" className={classes.brandTextSmall} display="inline">
                    .com
                </Typography>
                <div>
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
                                            disableElevation
                                            variant="contained"
                                            size="large"
                                            className={classes.menuButtonText}
                                        >
                                            {element.name}
                                        </Button>
                                    </Link>
                                );
                            }
                            return (
                                <Button
                                    disableElevation
                                    variant="contained"
                                    size="large"
                                    onClick={element.onClick}
                                    className={classes.menuButtonText}
                                    key={element.name}
                                >
                                    {element.name}
                                </Button>
                            );
                        })
                    }
                </div>
            </Toolbar>
        </AppBar>
    );
};
