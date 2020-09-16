import React from "react";
import { CssBaseline, AppBar, Toolbar, IconButton, Typography, makeStyles } from "@material-ui/core";
import MenuIcon from '@material-ui/icons/Menu';
import classNames from "classnames";
import HelpIcon from '@material-ui/icons/Help';
import { UserDetails } from "./UserDetails";

interface DashboardHeaderProps {
    classes: any;
    open: boolean;
    helpOpen: boolean;
    handleDrawerOpen: () => void;
    handleHelpDrawerOpen: () => void;
    title: string;
}

const useStyles = makeStyles(theme => ({
    navbarWrapper: {
        width: "100%",
        display: "flex",
        justifyContent: "space-between"
    },
    helpIcon: {
        marginRight: "2rem",
        paddingRIght: "5rem"
    }
}))


export const DashboardHeader = ({ classes, open, handleDrawerOpen, title, handleHelpDrawerOpen, helpOpen }: DashboardHeaderProps) => {

    const navclasses = useStyles();

    return (
        <>
            <CssBaseline />
            <AppBar
                position="fixed"
                className={classNames(classes.appBar, {
                    [classes.appBarShift]: open,
                })}
            >
                <Toolbar className={navclasses.navbarWrapper}>
                    <div>
                        <div style={{float: "left"}}>
                            <IconButton color="inherit" aria-label="open drawer" onClick={() => handleDrawerOpen()} edge="start" className={classNames(classes.menuButton, open && classes.hide)}>
                                <MenuIcon />
                            </IconButton>
                        </div>
                        <div style={{ float: "right", paddingTop: "5px", verticalAlign:"middle" }}>
                            <Typography variant="h4">
                                {title}
                            </Typography>
                        </div>
                    </div>
                    <div className={navclasses.helpIcon}>
                        <UserDetails />
                        <IconButton color="inherit" aria-label="open help drawer" onClick={() => handleHelpDrawerOpen()} edge="end" className={classNames(classes.helpMenuButton, helpOpen && classes.hide)}>
                            <HelpIcon />
                        </IconButton>
                    </div>
                </Toolbar>
            </AppBar >
        </>
    );
};
