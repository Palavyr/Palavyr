import { Card, makeStyles, Tooltip } from "@material-ui/core";
import React, { useContext } from "react";
import FaceIcon from "@material-ui/icons/Face";
import ReplayIcon from "@material-ui/icons/Replay";
import "./style.scss";
import { openUserDetails } from "@store-dispatcher";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "widget/context/WidgetContext";

export interface ConvoHeaderProps {
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
            cursor: "pointer",
        },
    }),
    replayIcon: {
        color: theme.palette.common.white,
        position: "fixed",
        right: "5px",
        bottom: "5px",
        height: "1.2rem",
        width: "1.2rem",
        "&:hover": {
            cursor: "pointer",
        },
    },
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        paddingBottom: "2rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "10%",
    },
}));

export const ConvoHeader = ({ titleAvatar }: ConvoHeaderProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return (
        <Card className={cls.header}>
            <Tooltip title="Update your contact details" placement="left">
                <FaceIcon className={cls.settingsIcon} onClick={openUserDetails} />
            </Tooltip>
            <Tooltip title="Restart this chat" placement="left">
                <ReplayIcon className={cls.replayIcon} onClick={() => window.location.reload()} />
            </Tooltip>
            {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </Card>
    );
};
