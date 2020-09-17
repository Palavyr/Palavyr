import React, { useState } from "react";
import { useHistory, NavLink } from "react-router-dom";
import { List, ListItem, ListItemIcon, ListItemText, Collapse, Divider } from "@material-ui/core";
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ExpandLessIcon from '@material-ui/icons/ExpandLess';
import ChatIcon from '@material-ui/icons/Chat';
import DesktopWindowsIcon from '@material-ui/icons/DesktopWindows';
import Auth from "auth/Auth";
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import InboxIcon from '@material-ui/icons/Inbox';
import SettingsIcon from '@material-ui/icons/Settings';
import CompareIcon from '@material-ui/icons/Compare';
import GetAppIcon from '@material-ui/icons/GetApp';
import SubscriptionsIcon from '@material-ui/icons/Subscriptions';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import { uuid } from 'uuidv4';


export interface ISideBarMenu {
    areaIdentifiers: Array<string>;
    areaNames: Array<string>;
    toggleModal: () => void;
    active: boolean;
}

const createNavLink = (areaIdentifier: string, contentType: string) => {
    return `/dashboard/${contentType}/${areaIdentifier}`;
};

const SideBarMenu = ({ active, areaIdentifiers, areaNames, toggleModal }: ISideBarMenu) => {
    const [convosOpen, setConvosOpen] = useState(true);
    console.log("Is active? " + active)
    const history = useHistory();

    return (
        <>
            <List>
                <ListItem button onClick={() => setConvosOpen(!convosOpen)}>
                    <ListItemIcon>
                        <DesktopWindowsIcon />
                    </ListItemIcon>
                    <ListItemText
                        primary={"Convo Areas"}
                        onClick={() => {
                            setConvosOpen(!convosOpen);
                        }}
                    />
                    {convosOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={convosOpen} timeout="auto" unmountOnExit>
                    {areaIdentifiers.map((areaIdentifier, index) => {
                        return (
                           <NavLink key={areaIdentifier} to={createNavLink(areaIdentifier, 'editor')}>
                                <ListItem disabled={!active} button key={areaIdentifier}>
                                    <ListItemIcon>
                                        <ChatIcon />
                                    </ListItemIcon>
                                    <ListItemText primary={areaNames[index]} />
                                </ListItem>
                            </NavLink>
                        );
                    })}
                    <ListItem disabled={!active} button key={"New Area"} onClick={toggleModal}>
                        <ListItemIcon onClick={toggleModal}>
                            <AddCircleOutlineIcon />
                        </ListItemIcon>
                        <ListItemText primary={"Add New Area"} />
                    </ListItem>
                </Collapse>

                <Divider />


                <ListItem disabled={!active} button onClick={() => history.push('/dashboard/enquiries/')}>
                    <ListItemIcon>
                        <InboxIcon key={uuid()} />
                    </ListItemIcon>
                    <ListItemText primary={"Check Enquiries"} />
                </ListItem>
                <ListItem disabled={!active} button onClick={() => history.push('/dashboard/demo/')}>
                    <ListItemIcon>
                        <CompareIcon key={uuid()} />
                    </ListItemIcon>
                    <ListItemText primary={"Chat Demo"} />
                </ListItem>
            </List>

            <Divider />

            <List>
                <ListItem disabled={!active} button onClick={() => history.push('/dashboard/settings/')}>
                    <ListItemIcon>
                        <SettingsIcon key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Settings"} />
                </ListItem>
                <ListItem disabled={!active} button onClick={() => history.push('/dashboard/getwidget/')}>
                    <ListItemIcon>
                        <GetAppIcon key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Get Widget"} />
                </ListItem>
                <ListItem disabled={!active} button onClick={() => history.push('/dashboard/subscribe/')}>
                    <ListItemIcon>
                        <SubscriptionsIcon key={0} />
                    </ListItemIcon>
                    <ListItemText primary={"Subscribe"} />
                </ListItem>
                <ListItem
                    button
                    key={1003}
                    onClick={() => {
                        console.log("Logging Out");
                        Auth.logout(() => history.push("/"));
                    }}
                >
                    <ListItemIcon>
                        <ExitToAppIcon />
                    </ListItemIcon>
                    <ListItemText primary={"Log Out"} />
                </ListItem>
            </List>
        </>
    );
};

export default SideBarMenu;
