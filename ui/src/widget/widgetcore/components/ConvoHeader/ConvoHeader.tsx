import { Card, Fade, makeStyles, Tooltip } from "@material-ui/core";
import React, { useContext, useEffect, useRef, useState } from "react";
import FaceIcon from "@material-ui/icons/Face";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "../../context/WidgetContext";
import { openUserDetails } from "@store-dispatcher";
import classNames from "classnames";
import '@widgetcore/widget/widget.module.scss';
import { useWidgetStyles } from "@widgetcore/widget/Widget";

export interface ConvoHeaderProps {
    titleAvatar?: string;
}

const useStyles = makeStyles(theme => ({
    header: (props: WidgetPreferences) => ({
        backgroundColor: props.headerColor,
        color: props.headerFontColor,
        textAlign: "left",
        padding: ".5rem",
        wordWrap: "break-word",
        borderRadius: "0px",
        minHeight: "60px",
        // flex: "0 1 auto",
    }),
    flexProperty: {
        flexDirection: "column",
        textAlign: "center",
        borderRadius: "0px",
        display: "flex",
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
        boxShadow: "none",
        // textAlign: "left",
        wordWrap: "break-word",
        // padding: "0rem",
        width: "100%",
        wordBreak: "normal",
        // minHeight: "60px",
    },
    paper: {
        boxShadow: "none",
    },
    avatar: {
        width: "40px",
        height: "40px",
        borderRadius: "100%",
        marginRight: "10px",
        verticalAlign: "middle",
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
    const wcls = useWidgetStyles();
    return (
        <div className={classNames(wcls.pwrow, wcls.pheader, cls.header)}>
            {chatStarted && (
                <Fade in>
                    <Tooltip open={tipOpen} title="Update your contact details" placement="left">
                        <FaceIcon ref={ref as any} className={cls.settingsIcon} onClick={openUserDetails} />
                    </Tooltip>
                </Fade>
            )}
            {titleAvatar && <img src={titleAvatar} className="pcw-avatar" alt="profile" />}
            <div className={classNames(cls.headerBehavior, "palavyr-header")} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </div>
    );
};
