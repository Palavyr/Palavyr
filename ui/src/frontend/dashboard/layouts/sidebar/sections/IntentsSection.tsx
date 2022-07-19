import React, { memo, useState } from "react";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { List, Collapse, Divider, makeStyles, Tooltip } from "@material-ui/core";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import { useHistory } from "react-router-dom";
import { AreaLinkItem } from "./sectionComponents/AreaLinkItem";
import { IntentNameDetail, IntentNameDetails } from "@Palavyr-Types";
import classNames from "classnames";
import InsertEmoticonIcon from "@material-ui/icons/InsertEmoticon";

const useStyles = makeStyles(theme => ({
    icon: {
        color: theme.palette.secondary.light,
    },
}));

export interface IntentsSectionProps {
    isActive: boolean;
    currentPage: string;
    areaNameDetails: IntentNameDetails;
    menuOpen: boolean;
}

export const IntentsSection = memo(({ isActive, currentPage, areaNameDetails, menuOpen }: IntentsSectionProps) => {
    const [configureOpen, setConfigureOpen] = useState<boolean>(true);
    const { checkAreaCount, planTypeMeta, repository } = React.useContext(DashboardContext);

    const history = useHistory();
    const cls = useStyles();

    const enableAreasOnClick = () => {
        history.push("/dashboard/set-areas");
    };

    const pushToIntro = async () => {
        const introductionId = await repository.Settings.Account.GetIntroductionId();
        history.push(`/dashboard/editor/conversation/intro/${introductionId}`);
    };

    return (
        <List className={classNames("configure-tour")}>
            <SidebarSectionHeader menuOpen={menuOpen} title="Intents" onClick={() => setConfigureOpen(!configureOpen)} currentState={configureOpen} />
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
                toolTipText="Add New Intent"
                menuOpen={menuOpen}
                className={"add-new-area-tour"}
                text="Add New Intent"
                isActive={isActive}
                onClick={checkAreaCount}
                IconComponent={<AddCircleOutlineIcon className={cls.icon} />}
            />
            <SidebarLinkItem
                toolTipText="Enable / Disable Intents"
                menuOpen={menuOpen}
                className={"enable-disable-area-tour"}
                text="Enable / Disable Intents"
                isActive={isActive}
                onClick={enableAreasOnClick}
                IconComponent={<PowerSettingsNewIcon className={cls.icon} />}
            />
            <Collapse className={"configure-your-area-tour"} in={configureOpen} timeout="auto" unmountOnExit>
                <Divider />
                {areaNameDetails.map(
                    (x: IntentNameDetail, index: number) =>
                        planTypeMeta && (
                            <AreaLinkItem
                                key={index}
                                menuOpen={menuOpen}
                                intentId={x.intentId}
                                isActive={isActive}
                                disabled={index >= planTypeMeta.allowedAreas}
                                currentPage={currentPage}
                                areaName={x.intentName}
                            />
                        )
                )}
            </Collapse>
        </List>
    );
});
