import React from "react";
import { AppBar, Toolbar, IconButton, Typography, makeStyles, useTheme } from "@material-ui/core";
import MenuIcon from '@material-ui/icons/Menu';
import classNames from "classnames";
import HelpIcon from '@material-ui/icons/Help';

const drawerWidth: number = 240;

interface DashboardHeaderProps {
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
}

const useStyles = makeStyles(theme => ({
    topbar: {
        backgroundColor: "green",
        position: "fixed",
    },
    appBar: {
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    toolbar: {
        color: "#c7ecee",
        width: "100%",
        display: "flex",
        justifyContent: "space-between",
    },


    helpIcon: {
        marginRight: "2rem",
        paddingRIght: "5rem"
    },
    appBarShift: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    hide: {
        display: "none",
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    helpMenuButton: {
        marginLeft: theme.spacing(5),
        alignSelf: "right",
        textAlign: "right",
        // marginRight: "1rem"
    },
}))


export const DashboardHeader = ({ open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {

    const classes = useStyles();
    const themr = useTheme();
    console.log("Header: " + themr.mixins.toolbar.minHeight);
    return (
        <AppBar
            position="absolute"
            className={
                classNames(
                    classes.topbar,
                    classes.appBar,
                    { [classes.appBarShift]: open })
            }
        >
            <Toolbar className={classes.toolbar}>
                <div>
                    <div style={{ float: "left" }}>
                        <IconButton color="inherit" aria-label="open drawer" onClick={() => handleDrawerOpen()} edge="start" className={classNames(classes.menuButton, open && classes.hide)}>
                            <MenuIcon />
                        </IconButton>
                    </div>
                    <div style={{ float: "right", paddingTop: "5px", verticalAlign: "middle" }}>
                        <Typography variant="h4">
                            {title}
                        </Typography>
                    </div>
                </div>
                <div className={classes.helpIcon}>
                    <IconButton color="inherit" aria-label="open help drawer" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(classes.helpMenuButton, helpOpen && classes.hide)}>
                        <HelpIcon />
                    </IconButton>
                </div>
            </Toolbar>
        </AppBar >

    );
};
