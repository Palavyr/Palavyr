import { Fade, makeStyles, Tooltip } from "@material-ui/core";
import React, { useContext, useEffect, useRef, useState } from "react";
import FaceIcon from "@material-ui/icons/Face";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { WidgetContext } from "../../context/WidgetContext";
import classNames from "classnames";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import "@widgetcore/widget/widget.module.scss";
import { OsTypeToggle } from "@frontend/dashboard/content/responseConfiguration/areaSettings/enableAreas/OsTypeToggle";

export interface ConvoHeaderProps {
    titleAvatar?: string;
}

const useStyles = makeStyles(theme => ({
    header: (props: WidgetPreferencesResource) => ({
        backgroundColor: props.headerColor,
        color: props.headerFontColor,
        fontFamily: props.fontFamily,
        textAlign: "left",
        padding: ".5rem",
        wordWrap: "break-word",
        borderRadius: "0px",
        minHeight: "auto",
    }),
    flexProperty: {
        flexDirection: "column",
        textAlign: "center",
        borderRadius: "0px",
        display: "flex",
    },
    settingsIcon: (props: WidgetPreferencesResource) => ({
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
        paddingLeft: "0.8rem",
        paddingRight: "0.8rem",
        wordWrap: "break-word",
        width: "100%",
        wordBreak: "normal",
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
    const { preferences, context } = useContext(WidgetContext);

    useEffect(() => {
        if (context.detailsIconEnabled) {
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
    }, [context.detailsIconEnabled]);

    const cls = useStyles(preferences);
    const wcls = useWidgetStyles();
    console.log("ICON: " + context.detailsIconEnabled);
    return (
        <div className={classNames(wcls.pwrow, wcls.pheader, cls.header)}>
            {context.detailsIconEnabled && (
                <Fade in>
                    <>
                        <Tooltip open={tipOpen} title="Update your contact details" placement="left">
                            <FaceIcon ref={ref as any} className={cls.settingsIcon} onClick={context.openUserDetails} />
                        </Tooltip>
                    </>
                </Fade>
            )}
            {titleAvatar && <img src={titleAvatar} className="pcw-avatar" alt="profile" />}
            <div className={classNames(cls.headerBehavior, "palavyr-header")} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </div>
    );
};
