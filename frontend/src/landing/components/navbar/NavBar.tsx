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
    setSelectedTab: any;
}

const useStyles = makeStyles({
    menuButton: {},
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
        // color: "#c7ecee"
    },
    noDecoration: {
        textDecoration: "none !important"
    }
});

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, selectedTab, setSelectedTab }: INavBar) => {

    const classes = useStyles();

    return (
        <AppBar position="fixed" className={classes.appBar} color="transparent" classes={{root: classes.clear}}>
            <Toolbar className={classes.toolbar}>
                <div>
                    <Typography variant="body2" className={classes.brandText} display="inline" >
                        Palavyr
                    </Typography>
                    <Typography variant="body2" className={classes.brandTextSmall} display="inline">
                        .com
                    </Typography>
                </div>
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
                                            // classes={{ text: classes.menuButtonText }}
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
