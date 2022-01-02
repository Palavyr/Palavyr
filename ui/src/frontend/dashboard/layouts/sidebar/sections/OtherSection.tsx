import React, { memo, useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import Auth from "@auth/Auth";
import GetAppIcon from "@material-ui/icons/GetApp";
import ExitToAppIcon from "@material-ui/icons/ExitToApp";
// import SettingsIcon from "@material-ui/icons/Settings";
// import { GeneralSettingsLoc } from "@Palavyr-Types";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { GoogleLogout } from "react-google-login";
import { googleOAuthClientId } from "@common/client/clientUtils";
import EmojiPeopleIcon from "@material-ui/icons/EmojiPeople";

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface OtherSectionProps {
    isActive: boolean;
    menuOpen: boolean;
}

export const OtherSection = memo(({ isActive, menuOpen }: OtherSectionProps) => {
    const [otherOpen, setOtherOpen] = useState<boolean>(true);
    const { setViewName } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const getStartedOnClick = () => {
        history.push("/dashboard/welcome");
    };

    // const generalSettingsOnClick = () => {
    //     setViewName("General Settings");
    //     history.push(`/dashboard/settings/email?tab=${GeneralSettingsLoc.email}`);
    // };

    const getWidgetOnClick = () => {
        setViewName("Get Widget");
        history.push("/dashboard/getwidget");
    };

    const takeToursOnClick = () => {
        setViewName("Palavyr Product Tours");
        history.push("/dashboard/tour");
    };

    const logoutOnClick = () => Auth.PerformLogout(() => history.push("/"));

    return (
        <List>
            <SidebarSectionHeader menuOpen={menuOpen} className={"other-sidebar-tour"} title="Other" onClick={() => setOtherOpen(!otherOpen)} currentState={otherOpen} />
            <Collapse in={otherOpen} timeout="auto" unmountOnExit>
                {/* <SidebarLinkItem
                    toolTipText=" General Settings"
                    menuOpen={menuOpen}
                    className={"settings-sidebar-tour"}
                    text="Settings"
                    isActive={isActive}
                    onClick={generalSettingsOnClick}
                    IconComponent={<SettingsIcon className={cls.icon} />}
                /> */}
                <SidebarLinkItem
                    toolTipText="How To Get The Widget"
                    menuOpen={menuOpen}
                    className={"get-widget-sidebar-tour"}
                    text="Get The Widget"
                    isActive={isActive}
                    onClick={getWidgetOnClick}
                    IconComponent={<GetAppIcon className={cls.icon} />}
                />
                <SidebarLinkItem
                    toolTipText="Quick Start Guide"
                    menuOpen={menuOpen}
                    className={"quick-start-sidebar-tour"}
                    text="Quick Start Guide"
                    isActive={isActive}
                    onClick={getStartedOnClick}
                    IconComponent={<PlayArrowIcon className={cls.icon} />}
                />
                <SidebarLinkItem
                    toolTipText="Palavyr Tours"
                    menuOpen={menuOpen}
                    className={"take-tours-sidebard-tour"}
                    text="Palavyr Tours"
                    isActive={isActive}
                    onClick={takeToursOnClick}
                    IconComponent={<EmojiPeopleIcon className={cls.icon} />}
                />
            </Collapse>
            <GoogleLogout
                onLogoutSuccess={logoutOnClick}
                clientId={googleOAuthClientId}
                render={renderProps => {
                    return (
                        <SidebarLinkItem
                            toolTipText="Log out"
                            menuOpen={menuOpen}
                            text="Log out"
                            isActive={true}
                            onClick={renderProps.onClick}
                            disabled={renderProps.disabled}
                            IconComponent={<ExitToAppIcon className={cls.icon} />}
                        />
                    );
                }}
            />
        </List>
    );
});
