import { Card, makeStyles } from "@material-ui/core";
import React from "react";
import SettingsIcon from "@material-ui/icons/Settings";

import "./style.scss";
import { openUserDetails } from "@store-dispatcher";
import { WidgetPreferences } from "@Palavyr-Types";

export interface ConvoHeaderProps {
    chatHeader: string;
    titleAvatar?: string;
    preferences: WidgetPreferences;
}

const useStyles = makeStyles({
    header: (props: WidgetPreferences) => ({
        backgroundColor: props.headerColor,
        color: props.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        wordWrap: "break-word",
        borderRadius: "0px",
    }),
    flexProperty: {
        flexDirection: "column",
        textAlign: "center",
        borderRadius: "0px",
        display: "flex",
        padding: "15px 0 25px",
    },
    settingsIcon: {
        position: "fixed",
        right: "5px",
        top: "5px",
        height: "2rem",
        width: "2rem",
    },
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        paddingBottom: "2rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "18%",
    },
});

export const ConvoHeader = ({ titleAvatar, preferences }: ConvoHeaderProps) => {
    const cls = useStyles(preferences);
    return (
        <Card className={cls.header}>
            <SettingsIcon className={cls.settingsIcon} onClick={openUserDetails} />
            {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </Card>
    );
};