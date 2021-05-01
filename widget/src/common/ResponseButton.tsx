import * as React from "react";
import { Button, makeStyles, PropTypes } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";

export interface IResponseButton {
    onClick: any;
    prefs: WidgetPreferences;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
}

const useStyles = makeStyles(theme => ({
    button: (prefs: WidgetPreferences) => ({
        color: prefs.chatBubbleColor ? theme.palette.getContrastText(theme.palette.getContrastText(prefs.chatBubbleColor)) : "none",
        backgroundColor: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : "none",
        marginBottom: "0.4rem",
        transion: "all ease-in-out 0.2s",
        "&:hover": {
            backgroundColor: prefs.chatBubbleColor ? theme.palette.getContrastText(theme.palette.getContrastText(prefs.chatBubbleColor)) : prefs.chatFontColor,
            color: prefs.chatBubbleColor ? theme.palette.getContrastText(prefs.chatBubbleColor) : prefs.chatFontColor,
            transition: "all ease-in-out 0.2s",
            boxShadow: theme.shadows[14],
            border: "none",
        },
    }),
    buttonFocus: (prefs: WidgetPreferences) => ({
        color: prefs.chatFontColor,
        borderColor: prefs.chatFontColor,
    }),
}));

export const ResponseButton = ({ onClick, prefs, disabled = false, variant = "outlined", text = "Submit" }: IResponseButton) => {
    const cls = useStyles(prefs);
    return (
        <Button disableElevation focusVisibleClassName={cls.buttonFocus} className={cls.button} disabled={disabled} variant={variant} size="small" onClick={onClick}>
            {text}
        </Button>
    );
};
