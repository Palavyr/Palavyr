import React, { useState } from "react";
import { useHistory, NavLink } from "react-router-dom";
import { List, ListItem, ListItemIcon, ListItemText, Collapse, Divider, makeStyles } from "@material-ui/core";
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ExpandLessIcon from '@material-ui/icons/ExpandLess';
import ChatIcon from '@material-ui/icons/Chat';
import Auth from "auth/Auth";
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import InboxIcon from '@material-ui/icons/Inbox';
import SettingsIcon from '@material-ui/icons/Settings';
import CompareIcon from '@material-ui/icons/Compare';
import GetAppIcon from '@material-ui/icons/GetApp';
import SubscriptionsIcon from '@material-ui/icons/Subscriptions';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import { v4 as uuid } from 'uuid';
import HelpOutlineIcon from '@material-ui/icons/HelpOutline';
import { LocalStorage } from "localStorage/localStorage";
import { AuthContext, DashboardContext } from "../DashboardContext";

export interface ISideBarMenu {
    areaIdentifiers: Array<string>;
    areaNames: Array<string>;
}

const createNavLink = (areaIdentifier: string, contentType: string) => {
    return `/dashboard/${contentType}/${areaIdentifier}`;
};

const useStyles = makeStyles(theme => ({
    SideBarList: {
        color: "navy",
        backgroundColor: "transparent"
    },
    icon: {
        color: "navy"
    },
    navlink: {
        textDecoration: "none",
        color: "#c7ecee",
    },
    sidebarText: {
        fontWeight: "normal",
        fontSize: "14px",
        color: "navy"

    }
}));

export const SideBarMenu = ({ areaIdentifiers, areaNames}: ISideBarMenu) => {

    const classes = useStyles();

    const [convosOpen, setConvosOpen] = useState(true);
    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);
    const { checkAreaCount, setViewName } = React.useContext(DashboardContext);

    return (
        <div className={classes.SideBarList}>
            <List>
                <ListItem button onClick={() => setConvosOpen(!convosOpen)}>
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Configure"
                        onClick={() => {
                            setConvosOpen(!convosOpen);
                        }}
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: 'black' } }}
                    />
                    {convosOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={convosOpen} timeout="auto" unmountOnExit>
                    {areaIdentifiers.map((areaIdentifier, index) => {
                        return (
                            <NavLink key={areaIdentifier} to={createNavLink(areaIdentifier, 'editor')} className={classes.navlink}>
                                <ListItem disabled={!isActive} button key={areaIdentifier}>
                                    <ListItemIcon className={classes.icon}>
                                        <ChatIcon />
                                    </ListItemIcon>
                                    <ListItemText primary={areaNames[index]} primaryTypographyProps={{ className: classes.sidebarText }} />
                                </ListItem>
                            </NavLink>
                        );
                    })}
                    <ListItem disabled={!isActive} button key={"New Area"} onClick={() => {
                        checkAreaCount();

                    }}>
                        <ListItemIcon onClick={checkAreaCount}>
                            <AddCircleOutlineIcon className={classes.icon} />
                        </ListItemIcon>
                        <ListItemText primary={"Add New Area"} primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                </Collapse>

                <Divider />

                <ListItem >
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Review"
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: "black" } }}
                    />
                </ListItem>
                <ListItem disabled={!isActive} button onClick={() => {
                    setViewName("Enquiries");
                    history.push('/dashboard/enquiries/')
                }}>
                    <ListItemIcon className={classes.icon}>
                        <InboxIcon className={classes.icon} key={"23534hhuip"} />
                    </ListItemIcon>
                    <ListItemText primary={"Check Enquiries"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>
                <ListItem disabled={!isActive} button onClick={() => {
                    setViewName("Widget Demo")
                    history.push('/dashboard/demo/')
                }}>
                    <ListItemIcon>
                        <CompareIcon className={classes.icon} key={"iuhi3453jb"} />
                    </ListItemIcon>
                    <ListItemText primary="Chat Demo" primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>
            </List>

            <Divider />

            <List>
                <ListItem disabled={!isActive} button onClick={() => {
                    setViewName("General Settings");
                    history.push('/dashboard/settings/')
                }}>
                    <ListItemIcon className={classes.icon}>
                        <SettingsIcon className={classes.icon} key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Settings"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>
                <ListItem disabled={!isActive} button onClick={() => {
                    setViewName("Get Widget")
                    history.push('/dashboard/getwidget/')
                }}>
                    <ListItemIcon>
                        <GetAppIcon className={classes.icon} key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Get Widget"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>

                <ListItem disabled={!isActive} button onClick={() => {
                    console.log("Setting Header!")
                    setViewName("Subscriptions")
                    history.push('/dashboard/subscribe/')
                }}>
                    <ListItemIcon>
                        <SubscriptionsIcon className={classes.icon} key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Subscribe"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>

                <ListItem disabled={!isActive} button onClick={() => {
                    console.log("Setting Header!")
                    setViewName("Welcome!")
                    history.push('/dashboard/welcome')
                }}>
                    <ListItemIcon>
                        <HelpOutlineIcon className={classes.icon} key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Get Help"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>

                <ListItem
                    button
                    key={1003}
                    onClick={() => {

                        const loginType = LocalStorage.getLoginType();

                        if (loginType === LocalStorage.GoogleLoginType) {
                            Auth.googleLogout(() => history.push("/"));
                        } else {
                            console.log("Logging Out");
                            Auth.logout(() => history.push("/"));
                        }
                    }}

                >
                    <ListItemIcon>
                        <ExitToAppIcon className={classes.icon} />
                    </ListItemIcon>
                    <ListItemText primary={"Log Out"} primaryTypographyProps={{ className: classes.sidebarText }} />
                </ListItem>

            </List>
        </div>
    );
};
