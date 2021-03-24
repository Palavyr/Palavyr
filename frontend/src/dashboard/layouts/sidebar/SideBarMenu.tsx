import React, { useState } from "react";
import { useHistory, NavLink } from "react-router-dom";
import { List, ListItem, ListItemIcon, ListItemText, Collapse, Divider, makeStyles, FormControlLabel } from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ExpandLessIcon from "@material-ui/icons/ExpandLess";
import ChatIcon from "@material-ui/icons/Chat";
import Auth from "auth/Auth";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import InboxIcon from "@material-ui/icons/Inbox";
import SettingsIcon from "@material-ui/icons/Settings";
import CompareIcon from "@material-ui/icons/Compare";
import GetAppIcon from "@material-ui/icons/GetApp";
import SubscriptionsIcon from "@material-ui/icons/Subscriptions";
import ExitToAppIcon from "@material-ui/icons/ExitToApp";
import { AuthContext, DashboardContext } from "../DashboardContext";
import { GeneralSettingsLoc, PurchaseTypes } from "@Palavyr-Types";
import { IOSSwitch } from "@common/components/IOSSwitch";
import PaymentIcon from "@material-ui/icons/Payment";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";

export interface ISideBarMenu {
    areaIdentifiers: Array<string>;
    areaNames: Array<string>;
    widgetIsActive: boolean | undefined;
    updateWidgetIsActive(): void;
    createCustomerPortalSession(): void;
}
type StyleProps = {
    complete: boolean;
};

const createNavLink = (areaIdentifier: string) => {
    return `/dashboard/editor/email/${areaIdentifier}?tab=${0}`;
};

const useStyles = makeStyles((theme) => ({
    SideBarList: {
        color: "navy",
        backgroundColor: "transparent",
    },
    icon: {
        color: "navy",
    },
    rightIcon: (props: StyleProps) => ({
        color: props.complete ? "navy" : "red",
    }),
    navlink: {
        textDecoration: "none",
        color: "#c7ecee",
    },
    sidebarText: {
        fontWeight: "normal",
        fontSize: "14px",
        color: "navy",
    },
}));

export const SideBarMenu = ({ areaIdentifiers, areaNames, widgetIsActive, updateWidgetIsActive, createCustomerPortalSession }: ISideBarMenu) => {

    const [configureOpen, setConfigureOpen] = useState(true);
    const [reviewOpen, setReviewOpen] = useState(true);
    const [billingOpen, setBillingOpen] = useState(true);
    const [otherOpen, setOtherOpen] = useState(true);

    const history = useHistory();
    const { isActive } = React.useContext(AuthContext);
    const { checkAreaCount, setViewName, subscription, numAreasAllowed } = React.useContext(DashboardContext);

    // const searchParams = new URLSearchParams(location.search); // TODO: can we go to same page when switching areas
    // const currentTab = searchParams.get("tab") as string;
    const currentPage =  history.location.pathname + history.location.search;
    const classes = useStyles();

    return (
        <div className={classes.SideBarList}>
            <ListItem style={{ backgroundColor: widgetIsActive === undefined ? "gray" : widgetIsActive ? "green" : "red" }} disabled={!isActive}>
                <ListItemText style={{ color: "white", fontSize: "16px", textAlign: "center" }}>
                    <strong>Widget</strong>
                </ListItemText>
                <FormControlLabel
                    control={<IOSSwitch disabled={!isActive || widgetIsActive === undefined} checked={widgetIsActive === undefined ? true : widgetIsActive ? true : false} onChange={updateWidgetIsActive} name="Active" />}
                    style={{ color: "white", fontWeight: "bolder" }}
                    label={widgetIsActive === undefined ? "loading..." : widgetIsActive ? "Enabled" : "Disabled"}
                />
            </ListItem>
            <List>
                <ListItem button onClick={() => setConfigureOpen(!configureOpen)}>
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Configure"
                        onClick={() => {
                            setConfigureOpen(!configureOpen);
                        }}
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: "black" } }}
                    />
                    {configureOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={configureOpen} timeout="auto" unmountOnExit>
                    {areaIdentifiers.map((areaIdentifier, index) => {
                        return (
                            <NavLink key={areaIdentifier} to={(!isActive || index > numAreasAllowed) ? currentPage : createNavLink(areaIdentifier)} className={classes.navlink}>
                                <ListItem disabled={!isActive || index > numAreasAllowed} button key={areaIdentifier}>
                                    <ListItemIcon className={classes.icon}>
                                        <ChatIcon />
                                    </ListItemIcon>
                                    <ListItemText primary={areaNames[index]} primaryTypographyProps={{ className: classes.sidebarText }} />
                                </ListItem>
                            </NavLink>
                        );
                    })}
                    <Divider />
                    <ListItem
                        disabled={!isActive}
                        button
                        key="New Area"
                        onClick={() => {
                            checkAreaCount();
                        }}
                    >
                        <ListItemIcon onClick={checkAreaCount}>
                            <AddCircleOutlineIcon className={classes.icon} />
                        </ListItemIcon>
                        <ListItemText primary="Add New Area" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            setViewName("Enable / Disable Areas");
                            history.push("/dashboard/set-areas");
                        }}
                    >
                        <ListItemIcon>
                            <PowerSettingsNewIcon className={classes.icon} key={0} />
                        </ListItemIcon>
                        <ListItemText primary="Enable / Disable Areas" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                </Collapse>
            </List>
            <Divider />
            <List>
                <ListItem button onClick={() => setReviewOpen(!reviewOpen)}>
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Review"
                        onClick={() => {
                            setReviewOpen(!reviewOpen);
                        }}
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: "black" } }}
                    />
                    {reviewOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={reviewOpen} timeout="auto" unmountOnExit>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            setViewName("Enquiries");
                            history.push("/dashboard/enquiries/");
                        }}
                    >
                        <ListItemIcon className={classes.icon}>
                            <InboxIcon className={classes.icon} key={"23534hhuip"} />
                        </ListItemIcon>
                        <ListItemText primary={"Check Enquiries"} primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            setViewName("Widget Demo");
                            history.push("/dashboard/demo/");
                        }}
                    >
                        <ListItemIcon>
                            <CompareIcon className={classes.icon} key={"iuhi3453jb"} />
                        </ListItemIcon>
                        <ListItemText primary="Chat Demo" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                </Collapse>
            </List>

            <Divider />

            <List>
                <ListItem button onClick={() => setBillingOpen(!billingOpen)}>
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Billing"
                        onClick={() => {
                            setBillingOpen(!billingOpen);
                        }}
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: "black" } }}
                    />
                    {billingOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={billingOpen} timeout="auto" unmountOnExit>
                    {!(subscription === PurchaseTypes.Premium || subscription === PurchaseTypes.Pro) && (
                        <ListItem
                            disabled={!isActive}
                            button
                            onClick={() => {
                                console.log("Setting Header!");
                                setViewName("Subscriptions");
                                history.push("/dashboard/subscribe/");
                            }}
                        >
                            <ListItemIcon>
                                <SubscriptionsIcon className={classes.icon} key={0} />
                            </ListItemIcon>
                            <ListItemText primary="Subscribe" primaryTypographyProps={{ className: classes.sidebarText }} />
                        </ListItem>
                    )}
                    <ListItem disabled={!isActive || !(subscription === PurchaseTypes.Premium || subscription === PurchaseTypes.Pro)} button onClick={createCustomerPortalSession}>
                        <ListItemIcon>
                            <PaymentIcon className={classes.icon} key={0} />
                        </ListItemIcon>
                        <ListItemText primary="Manage" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                </Collapse>
            </List>
            <Divider />
            <List>
                <ListItem button onClick={() => setOtherOpen(!otherOpen)}>
                    <ListItemText
                        style={{ textAlign: "center" }}
                        primary="Other"
                        onClick={() => {
                            setOtherOpen(!otherOpen);
                        }}
                        primaryTypographyProps={{ className: classes.sidebarText, style: { fontSize: "16pt", color: "black" } }}
                    />
                    {otherOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ListItem>
                <Collapse in={otherOpen} timeout="auto" unmountOnExit>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            console.log("Setting Header!");
                            setViewName("Welcome!");
                            history.push("/dashboard/welcome");
                        }}
                    >
                        <ListItemIcon>
                            <PlayArrowIcon className={classes.icon} key={0} />
                        </ListItemIcon>
                        <ListItemText primary="Get Started" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            setViewName("General Settings");
                            history.push(`/dashboard/settings/password?tab=${GeneralSettingsLoc.password}`);
                        }}
                    >
                        <ListItemIcon className={classes.icon}>
                            <SettingsIcon className={classes.icon} key={0} />
                        </ListItemIcon>
                        <ListItemText primary="Settings" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                    <ListItem
                        disabled={!isActive}
                        button
                        onClick={() => {
                            setViewName("Get Widget");
                            history.push("/dashboard/getwidget/");
                        }}
                    >
                        <ListItemIcon>
                            <GetAppIcon className={classes.icon} key={0} />
                        </ListItemIcon>
                        <ListItemText primary="Get Widget" primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                    <ListItem button key={1003} onClick={() => Auth.PerformLogout(() => history.push("/"))}>
                        <ListItemIcon>
                            <ExitToAppIcon className={classes.icon} />
                        </ListItemIcon>
                        <ListItemText primary={"Log Out"} primaryTypographyProps={{ className: classes.sidebarText }} />
                    </ListItem>
                </Collapse>
            </List>
        </div>
    );
};
