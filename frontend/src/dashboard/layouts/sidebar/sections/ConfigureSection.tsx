import React, { useState } from "react";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { List, Collapse, Divider, makeStyles } from "@material-ui/core";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import { useHistory } from "react-router-dom";
import { AreaLinkItem } from "./sectionComponents/AreaLinkItem";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ConfigureSectionProps {
    isActive: boolean;
    areaIdentifiers: string[];
    currentPage: string;
    areaNames: string[];
}

export const ConfigureSection = ({ isActive, areaIdentifiers, currentPage, areaNames }: ConfigureSectionProps) => {
    const [configureOpen, setConfigureOpen] = useState<boolean>(true);
    const { checkAreaCount, setViewName, numAreasAllowed } = React.useContext(DashboardContext);

    const history = useHistory();
    const cls = useStyles();

    const enableAreasOnClick = () => {
        setViewName("Enable / Disable Areas");
        history.push("/dashboard/set-areas");
    };

    return (
        <List>
            <SidebarSectionHeader title="Configure" onClick={() => setConfigureOpen(!configureOpen)} currentState={configureOpen} />
            <Collapse in={configureOpen} timeout="auto" unmountOnExit>
                {areaIdentifiers.map((areaIdentifier, index) => (
                    <AreaLinkItem areaIdentifier={areaIdentifier} isActive={isActive} index={index} numAreasAllowed={numAreasAllowed} currentPage={currentPage} areaNames={areaNames} />
                ))}
                <Divider />
                <SidebarLinkItem text="Add New Area" isActive={isActive} onClick={checkAreaCount} IconComponent={<AddCircleOutlineIcon className={cls.icon} />} />
                <SidebarLinkItem text="Enable / Disable Areas" isActive={isActive} onClick={enableAreasOnClick} IconComponent={<PowerSettingsNewIcon className={cls.icon} />} />
            </Collapse>
        </List>
    );
};
