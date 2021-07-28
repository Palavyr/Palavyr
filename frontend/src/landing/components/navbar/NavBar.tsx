import React from "react";
import { AppBar, Toolbar, Typography, Button, makeStyles } from "@material-ui/core";
import { menuItems } from "./NavMenuItems";
import { Link, useHistory } from "react-router-dom";
import classNames from "classnames";
import { Align } from "dashboard/layouts/positioning/Align";

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
    menuButton: {
        backgroundColor: theme.palette.success.dark,
        marginRight: "1rem",
        "&:hover": {
            backgroundColor: theme.palette.common.white,
        },
    },
    menuButtonText: {
        color: theme.palette.common.white,
        "&:hover": {
            color: theme.palette.success.main,
        },
    },
    brandText: {
        fontSize: 64,
        fontWeight: "bolder",
        color: theme.palette.success.main,
        cursor: "pointer",
    },
    noDecoration: {
        textDecoration: "none !important",
    },
    logowrap: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
    },
    logotypography: {
        display: "flex",
        flexDirection: "row",
        verticalAlign: "middle",
        border: `3px solid ${theme.palette.success.light}`,
        padding: "0.4rem",
        borderRadius: "12px",
    },
    tutButton: {},
    navButtons: {
        display: "flex",
        justifyContent: "space-evenly",
        verticalAlign: "middle",
    },
}));

export const NavBar = ({ openRegisterDialog, openLoginDialog }: INavBar) => {
    const cls = useStyles();
    const history = useHistory();

    const redirectToTutorial = () => history.push("/tutorial");
    return (
        <AppBar position="fixed" className={cls.appBar} color="transparent" classes={{ root: cls.clear }}>
            <Toolbar className={cls.toolbar}>
                <div className={cls.logowrap}>
                    <div className={cls.logotypography} onClick={() => history.push("/")}>
                        <Typography variant="body2" className={cls.brandText} display="inline">
                            Palavyr
                        </Typography>
                    </div>
                </div>
                <div className={cls.navButtons}>
                    <Align verticalCenter>
                        {menuItems(openRegisterDialog, openLoginDialog, redirectToTutorial).map((element) => {
                            if (element.link) {
                                return (
                                    <Link key={element.name} to={element.link} className={cls.noDecoration}>
                                        <Button disableElevation variant="contained" size="medium" className={classNames(cls.menuButtonText, cls.menuButton)}>
                                            <Typography variant="h6" className={cls.menuButtonText}>
                                                {element.name}
                                            </Typography>
                                        </Button>
                                    </Link>
                                );
                            }
                            return (
                                <Button disableElevation variant="contained" size="medium" onClick={element.onClick} className={classNames(cls.menuButtonText, cls.menuButton)} key={element.name}>
                                    <Typography variant="h6" className={cls.menuButtonText}>
                                        {element.name}
                                    </Typography>
                                </Button>
                            );
                        })}
                        <div style={{ marginRight: "1.5rem", marginLeft: "2rem" }}>
                            <Link key="Home" to="/" className={cls.noDecoration}>
                                <span>
                                    <Typography variant="h6" className={cls.menuButtonText}>
                                        Home
                                    </Typography>
                                </span>
                            </Link>
                        </div>
                        <Link key="Tutorial" to="/tutorial" className={cls.noDecoration}>
                            <span className={cls.tutButton}>
                                <Typography variant="h6" className={cls.menuButtonText}>
                                    Tutorial
                                </Typography>
                            </span>
                        </Link>
                    </Align>
                </div>
            </Toolbar>
        </AppBar>
    );
};
