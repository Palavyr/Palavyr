import React, { memo, useState } from "react";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { List, Collapse, Divider, makeStyles, Tooltip } from "@material-ui/core";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import { useHistory } from "react-router-dom";
import { AreaLinkItem } from "./sectionComponents/AreaLinkItem";
import { AreaNameDetail, AreaNameDetails } from "@Palavyr-Types";
import classNames from "classnames";
import InsertEmoticonIcon from "@material-ui/icons/InsertEmoticon";

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface ConfigureSectionProps {
    isActive: boolean;
    currentPage: string;
    areaNameDetails: AreaNameDetails;
    menuOpen: boolean;
}

export const ConfigureSection = memo(({ isActive, currentPage, areaNameDetails, menuOpen }: ConfigureSectionProps) => {
    const [configureOpen, setConfigureOpen] = useState<boolean>(true);
    const { checkAreaCount, planTypeMeta, repository } = React.useContext(DashboardContext);

    const history = useHistory();
    const cls = useStyles();

    const enableAreasOnClick = () => {
        history.push("/dashboard/set-areas");
    };

    const pushToIntro = async () => {
        const introductionId = await repository.Settings.Account.getIntroductionId();
        history.push(`/dashboard/editor/conversation/intro/${introductionId}`);
    };

    return (
        <List className={classNames("configure-tour")}>
            <SidebarSectionHeader menuOpen={menuOpen} title="Configure" onClick={() => setConfigureOpen(!configureOpen)} currentState={configureOpen} />
            <SidebarLinkItem
                toolTipText="Introduction Sequence"
                menuOpen={menuOpen}
                className={"intro-sequence-tour"}
                text="Intro"
                isActive={isActive}
                onClick={pushToIntro}
                IconComponent={<InsertEmoticonIcon className={cls.icon} />}
            />
            <SidebarLinkItem
                toolTipText="Add New Area"
                menuOpen={menuOpen}
                className={"add-new-area-tour"}
                text="Add New Area"
                isActive={isActive}
                onClick={checkAreaCount}
                IconComponent={<AddCircleOutlineIcon className={cls.icon} />}
            />
            <SidebarLinkItem
                toolTipText="Enable / Disable Areas"
                menuOpen={menuOpen}
                className={"enable-disable-area-tour"}
                text="Enable / Disable Areas"
                isActive={isActive}
                onClick={enableAreasOnClick}
                IconComponent={<PowerSettingsNewIcon className={cls.icon} />}
            />
            <Collapse className={"configure-your-area-tour"} in={configureOpen} timeout="auto" unmountOnExit>
                <Divider />
                {areaNameDetails.map(
                    (x: AreaNameDetail, index: number) =>
                        planTypeMeta && (
                            <AreaLinkItem
                                key={index}
                                menuOpen={menuOpen}
                                areaIdentifier={x.areaIdentifier}
                                isActive={isActive}
                                disabled={index >= planTypeMeta.allowedAreas}
                                currentPage={currentPage}
                                areaName={x.areaName}
                            />
                        )
                )}
            </Collapse>
        </List>
    );
});