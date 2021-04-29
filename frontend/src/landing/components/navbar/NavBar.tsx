import React from "react";
import { AppBar, Toolbar, Typography, Hidden, IconButton, Button, makeStyles, Divider } from "@material-ui/core";
import { menuItems } from "./NavMenuItems";
import { Link } from "react-router-dom";
// import Logo from "../../../common/svgs/palavyrBranding/logo.svg";
// import Logo from "../../../common/svgs/palavyrBranding/logo.svg";
// import Logo from "../header/logo2.svg"
// import Logo from "srclandingcomponentsheaderlogo2.svg";

export interface INavBar {
    openRegisterDialog: any;
    openLoginDialog: any;
    handleMobileDrawerOpen: any;
    handleMobileDrawerClose: any;
    mobileDrawerOpen: any;
    selectedTab: string | null;
    setSelectedTab: any;
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
    menuButtonText: {
        fontSize: "large",
        color: "white",
        backgroundColor: theme.palette.success.dark,
        marginRight: "1rem",
        "&:hover": {
            backgroundColor: "white",
            color: theme.palette.common.black,
        },
    },
    brandText: {
        fontSize: 64,
        fontWeight: "bolder",
        color: theme.palette.success.main,
    },
    brandTextSmall: {
        fontWeight: "bolder",
        color: "black",
    },
    noDecoration: {
        textDecoration: "none !important",
    },
    logowrap: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
    },
    logo: {
        display: "flex",
        flexDirection: "row",
        height: "100%",
        paddingRight: "1rem",
        border: `3px solid ${theme.palette.success.main}`,
        color: theme.palette.success.light,
    },
    logotypography: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
        border: `3px solid ${theme.palette.success.light}`,
        padding: "0.4rem",
        borderRadius: "12px"
    },
}));

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, selectedTab, setSelectedTab }: INavBar) => {
    const classes = useStyles();
    return (
        <AppBar position="fixed" className={classes.appBar} color="transparent" classes={{ root: classes.clear }}>
            <Toolbar className={classes.toolbar}>
                <div className={classes.logowrap}>
                    {/* <div className={classes.logo}>
                        <Logo height="225px" width="225px" />
                    </div> */}
                    <div className={classes.logotypography}>
                        <Typography variant="body2" className={classes.brandText} display="inline">
                            Palavyr
                        </Typography>
                    </div>
                </div>
                <div>
                    {menuItems(openRegisterDialog, openLoginDialog).map((element) => {
                        if (element.link) {
                            return (
                                <Link key={element.name} to={element.link} className={classes.noDecoration} onClick={handleMobileDrawerClose}>
                                    <Button disableElevation variant="contained" size="large" className={classes.menuButtonText}>
                                        {element.name}
                                    </Button>
                                </Link>
                            );
                        }
                        return (
                            <Button disableElevation variant="contained" size="large" onClick={element.onClick} className={classes.menuButtonText} key={element.name}>
                                {element.name}
                            </Button>
                        );
                    })}
                </div>
            </Toolbar>
            <Divider light />
        </AppBar>
    );
};
