import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import { List, Collapse, makeStyles } from "@material-ui/core";
import Auth from "auth/Auth";
import SettingsIcon from "@material-ui/icons/Settings";
import GetAppIcon from "@material-ui/icons/GetApp";
import ExitToAppIcon from "@material-ui/icons/ExitToApp";
import { GeneralSettingsLoc } from "@Palavyr-Types";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface OtherSectionProps {
    isActive: boolean;
}

export const OtherSection = ({ isActive }: OtherSectionProps) => {
    const [otherOpen, setOtherOpen] = useState<boolean>(true);
    const { setViewName } = React.useContext(DashboardContext);

    const cls = useStyles();
    const history = useHistory();

    const getStartedOnClick = () => {
        setViewName("Welcome!");
        history.push("/dashboard/welcome");
    };

    const generalSettingsOnClick = () => {
        setViewName("General Settings");
        history.push(`/dashboard/settings/email?tab=${GeneralSettingsLoc.email}`);
    };

    const getWidgetOnClick = () => {
        setViewName("Get Widget");
        history.push("/dashboard/getwidget/");
    };

    const logoutOnClick = () => Auth.PerformLogout(() => history.push("/"));

    return (
        <List>
            <SidebarSectionHeader title="Other" onClick={() => setOtherOpen(!otherOpen)} currentState={otherOpen} />
            <Collapse in={otherOpen} timeout="auto" unmountOnExit>
                <SidebarLinkItem text="Get Started" isActive={isActive} onClick={getStartedOnClick} IconComponent={<PlayArrowIcon className={cls.icon} />} />
                <SidebarLinkItem text="Settings" isActive={isActive} onClick={generalSettingsOnClick} IconComponent={<SettingsIcon className={cls.icon} />} />
                <SidebarLinkItem text="Get Widget" isActive={isActive} onClick={getWidgetOnClick} IconComponent={<GetAppIcon className={cls.icon} />} />
            </Collapse>
            <SidebarLinkItem text="Logout" isActive={true} onClick={logoutOnClick} IconComponent={<ExitToAppIcon className={cls.icon} />} />
        </List>
    );
};
