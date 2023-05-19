import React, { useContext } from "react";
import cn from "classnames";

import { makeStyles } from "@material-ui/core";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

import clss from "./Loader.module.scss";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    loaderContainer: (props: WidgetPreferencesResource) => ({
        backgroundColor: props.chatBubbleColor,
        borderRadius: "10px",
        padding: "15px",
        maxWidth: "215px",
        textAlign: "left",
        display: "flex",
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center",
    }),
    dotColor: (props: WidgetPreferencesResource) => ({
        backgroundColor: theme.palette.getContrastText(props.chatBubbleColor),
    }),
    invisible: {
        opacity: 0,
    },
    text: (props: WidgetPreferencesResource) => ({
        color: theme.palette.getContrastText(props.chatBubbleColor),
        marginRight: ".5rem",
        fontSize: ".8rem",
    }),
}));

export const Loader = () => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    return (
        <div className={cls.loaderContainer}>
            <PalavyrText display="inline" className={cls.text}>
                <i>
                    Typing
                    {"   "}
                </i>
            </PalavyrText>
            <div className={cn(clss.dots, cls.dotColor)}></div>
            <div className={cn(clss.dots, cls.dotColor)}></div>
            <div className={cn(clss.dots, cls.dotColor)}></div>
        </div>
    );
};
