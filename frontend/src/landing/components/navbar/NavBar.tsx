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
    menuButton: {
        backgroundColor: theme.palette.success.dark,
        marginRight: "1rem",
        "&:hover": {
            backgroundColor: "white",
            color: theme.palette.common.black,
        },
    },
    menuButtonText: {
        color: "white",
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
        borderRadius: "12px",
    },
}));

export const NavBar = ({ openRegisterDialog, openLoginDialog, handleMobileDrawerOpen, handleMobileDrawerClose, mobileDrawerOpen, selectedTab, setSelectedTab }: INavBar) => {
    const cls = useStyles();
    return (
        <AppBar position="fixed" className={cls.appBar} color="transparent" classes={{ root: cls.clear }}>
            <Toolbar className={cls.toolbar}>
                <div className={cls.logowrap}>
                    <div className={cls.logotypography}>
                        <Typography variant="body2" className={cls.brandText} display="inline">
                            Palavyr
                        </Typography>
                    </div>
                </div>
                <div>
                    {menuItems(openRegisterDialog, openLoginDialog).map((element) => {
                        if (element.link) {
                            return (
                                <Link key={element.name} to={element.link} className={cls.noDecoration} onClick={handleMobileDrawerClose}>
                                    <Button disableElevation variant="contained" size="large" className={cls.menuButton}>
                                        <Typography variant="h6" className={cls.menuButtonText}>
                                            {element.name}
                                        </Typography>
                                    </Button>
                                </Link>
                            );
                        }
                        return (
                            <Button disableElevation variant="contained" size="medium" onClick={element.onClick} className={cls.menuButton} key={element.name}>
                                <Typography variant="h6" className={cls.menuButtonText}>
                                    {element.name}
                                </Typography>
                            </Button>
                        );
                    })}
                </div>
            </Toolbar>
            {/* <Divider light /> */}
        </AppBar>
    );
};
