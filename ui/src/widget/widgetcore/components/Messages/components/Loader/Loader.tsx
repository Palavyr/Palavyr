import React, { useContext } from "react";
import cn from "classnames";

import "./styles.scss";
import { Box, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

type Props = {
    typing: boolean;
};

const useStyles = makeStyles(theme => ({
    loaderContainer: (props: WidgetPreferences) => ({
        backgroundColor: props.chatBubbleColor,
        borderRadius: "10px",
        padding: "15px",
        maxWidth: "215px",
        textAlign: "left",
    }),
    dotColor: (props: WidgetPreferences) => ({
        backgroundColor: props.chatBubbleColor ? theme.palette.getContrastText(props.chatBubbleColor) : props.chatBubbleColor,
    }),
}));

export const Loader = ({ typing }: Props) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    return (
        <div className={cn("loader", { active: typing })}>
            <Box boxShadow={0} className={cls.loaderContainer}>
                <i style={{fontSize: "10px"}}>Typing...</i>{"   "}
                <span className={cn("loader-dots", cls.dotColor)}></span>
                <span className={cn("loader-dots", cls.dotColor)}></span>
                <span className={cn("loader-dots", cls.dotColor)}></span>
            </Box>
        </div>
    );
};
