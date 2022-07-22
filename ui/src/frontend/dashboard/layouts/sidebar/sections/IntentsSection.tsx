import React, { memo, useState } from "react";
import { DashboardContext } from "../../DashboardContext";
import { SidebarSectionHeader } from "./sectionComponents/SidebarSectionHeader";
import { SidebarLinkItem } from "./sectionComponents/SideBarLinkItem";
import { List, Collapse, Divider, makeStyles, Tooltip } from "@material-ui/core";
import PowerSettingsNewIcon from "@material-ui/icons/PowerSettingsNew";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import { useHistory } from "react-router-dom";
import { IntentLinkItem } from "./sectionComponents/IntentLinkItem";
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
    intentNameDetails: IntentNameDetails;
    menuOpen: boolean;
}

export const IntentsSection = memo(({ isActive, currentPage, intentNameDetails, menuOpen }: IntentsSectionProps) => {
    const [configureOpen, setConfigureOpen] = useState<boolean>(true);
    const { checkIntentCount, planTypeMeta, repository } = React.useContext(DashboardContext);

    const history = useHistory();
    const cls = useStyles();

    const enableIntentsOnClick = () => {
        history.push("/dashboard/set-intents");
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
                className={"add-new-intent-tour"}
                text="Add New Intent"
                isActive={isActive}
                onClick={checkIntentCount}
                IconComponent={<AddCircleOutlineIcon className={cls.icon} />}
            />
            <SidebarLinkItem
                toolTipText="Enable / Disable Intents"
                menuOpen={menuOpen}
                className={"enable-disable-intent-tour"}
                text="Enable / Disable Intents"
                isActive={isActive}
                onClick={enableIntentsOnClick}
                IconComponent={<PowerSettingsNewIcon className={cls.icon} />}
            />
            <Collapse className={"configure-your-intent-tour"} in={configureOpen} timeout="auto" unmountOnExit>
                <Divider />
                {intentNameDetails.map(
                    (x: IntentNameDetail, index: number) =>
                        planTypeMeta && (
                            <IntentLinkItem
                                key={index}
                                menuOpen={menuOpen}
                                intentId={x.intentId}
                                isActive={isActive}
                                disabled={index >= planTypeMeta.allowedIntents}
                                currentPage={currentPage}
                                intentName={x.intentName}
                            />
                        )
                )}
            </Collapse>
        </List>
    );
});
