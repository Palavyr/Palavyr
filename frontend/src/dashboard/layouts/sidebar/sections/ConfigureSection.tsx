import React, { memo, useState } from "react";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { List, Collapse, Divider, makeStyles } from "@material-ui/core";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import { useHistory } from "react-router-dom";
import { AreaLinkItem } from "./sectionComponents/AreaLinkItem";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { AreaNameDetail, AreaNameDetails } from "@Palavyr-Types";
import BarChartIcon from "@material-ui/icons/BarChart";
import TrendingUpIcon from "@material-ui/icons/TrendingUp";
import TrendingUp from "@material-ui/icons/TrendingUp";

const useStyles = makeStyles((theme) => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ConfigureSectionProps {
    isActive: boolean;
    currentPage: string;
    areaNameDetails: AreaNameDetails;
}

export const ConfigureSection = memo(({ isActive, currentPage, areaNameDetails }: ConfigureSectionProps) => {
    const [configureOpen, setConfigureOpen] = useState<boolean>(true);
    const { checkAreaCount, setViewName, planTypeMeta } = React.useContext(DashboardContext);

    const history = useHistory();
    const cls = useStyles();

    const enableAreasOnClick = () => {
        setViewName("Enable / Disable Areas");
        history.push("/dashboard/set-areas");
    };

    const dashboardOnClick = () => {
        setViewName("Data Dashboard");
        history.push("/dashboard/data");
    };

    return (
        <List>
            <SidebarLinkItem text="Activity" primaryTypographyProps={{ variant: "h5" }} isActive={isActive} onClick={dashboardOnClick}>
                <TrendingUpIcon />
            </SidebarLinkItem>
            <SidebarSectionHeader title="Configure" onClick={() => setConfigureOpen(!configureOpen)} currentState={configureOpen} />
            <SidebarLinkItem text="Add New Area" isActive={isActive} onClick={checkAreaCount} IconComponent={<AddCircleOutlineIcon className={cls.icon} />} />
            <SidebarLinkItem text="Enable / Disable Areas" isActive={isActive} onClick={enableAreasOnClick} IconComponent={<PowerSettingsNewIcon className={cls.icon} />} />
            <Collapse in={configureOpen} timeout="auto" unmountOnExit>
                <Divider />
                {areaNameDetails.map(
                    (x: AreaNameDetail, index: number) =>
                        planTypeMeta && (
                            <AreaLinkItem key={index} areaIdentifier={x.areaIdentifier} isActive={isActive} disabled={index >= planTypeMeta.allowedAreas} currentPage={currentPage} areaName={x.areaName} />
                        )
                )}
            </Collapse>
        </List>
    );
});
