import { Card, Fade, makeStyles, Tooltip } from "@material-ui/core";
import React, { useContext, useEffect, useRef, useState } from "react";
import FaceIcon from "@material-ui/icons/Face";
import "./style.scss";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "../../context/WidgetContext";

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
        // padding: "15px 0 25px",
    },
    settingsIcon: (props: WidgetPreferences) => ({
        color: theme.palette.getContrastText(props.headerColor),
        position: "relative",
        float: "right",
        top: "0px",
        margin: "0.2rem",
        height: "2rem",
        width: "2rem",
        "&:hover": {
            cursor: "pointer",
        },
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

export const ConvoHeader = ({ titleAvatar }: ConvoHeaderProps) => {
    const [tipOpen, setTipOpen] = useState<boolean>(false);
    const ref = useRef<HTMLDivElement>(null);

    const { preferences, chatStarted } = useContext(WidgetContext);
    useEffect(() => {
        if (chatStarted) {
            setTipOpen(true);
            setTimeout(() => {
                setTipOpen(false);
            }, 3000);
        }

        if (ref && ref.current) {
            ref.current.addEventListener("mouseover", () => {
                setTipOpen(true);
            });
            ref.current.addEventListener("mouseout", () => {
                setTipOpen(false);
            });
        }
        return () => {
            if (ref && ref.current) {
                ref.current.removeEventListener("mouseover", () => setTipOpen(false));
                ref.current.removeEventListener("mouseout", () => setTipOpen(false));
            }
        };
    }, [chatStarted]);

    const cls = useStyles(preferences);
    return (
        <Card className={cls.header}>
            <Fade in>
                <Tooltip open={tipOpen} title="Update your contact details" placement="left">
                    <FaceIcon ref={ref as any} className={cls.settingsIcon} />
                </Tooltip>
            </Fade>
            {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </Card>
    );
};
