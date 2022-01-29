import React, { useContext } from "react";
import cn from "classnames";

// import "./styles.scss";
import { makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

import clss from "./Loader.module.scss";

type Props = {
    typing?: boolean;
};

const useStyles = makeStyles(theme => ({
    loaderContainer: (props: WidgetPreferences) => ({
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
    dotColor: (props: WidgetPreferences) => ({
        backgroundColor: theme.palette.getContrastText(props.chatBubbleColor),
    }),
    invisible: {
        opacity: 0,
    },
    text: (props: WidgetPreferences) => ({
        color: theme.palette.getContrastText(props.chatBubbleColor),
        marginRight: ".5rem",
        fontSize: ".8rem",
    }),
}));

export const Loader = ({ typing }: Props) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    return (
        <div className={cls.loaderContainer}>
            <PalavyrText display="inline-block" className={cls.text}>
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
