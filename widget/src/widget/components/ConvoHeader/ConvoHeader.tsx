import { Card, makeStyles } from "@material-ui/core";
import React from "react";
import SettingsIcon from "@material-ui/icons/Settings";
import FaceIcon from '@material-ui/icons/Face';

import "./style.scss";
import { getWidgetPreferences, openUserDetails } from "@store-dispatcher";
import { WidgetPreferences } from "@Palavyr-Types";

export interface ConvoHeaderProps {
    preferences: WidgetPreferences;
    titleAvatar?: string;
}

const useStyles = makeStyles(theme => ({
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
    settingsIcon: (props: WidgetPreferences) => ({
        color: theme.palette.getContrastText(props.headerColor),
        position: "fixed",
        right: "5px",
        top: "5px",
        height: "2rem",
        width: "2rem",
        "&:hover": {
            cursor: "pointer"
        }
    }),
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        paddingBottom: "2rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "10%",
    },
}));

export const ConvoHeader = ({ preferences, titleAvatar }: ConvoHeaderProps) => {
    const cls = useStyles(preferences);
    return (
        <Card className={cls.header}>
            <FaceIcon className={cls.settingsIcon} onClick={openUserDetails} />
            {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </Card>
    );
};
