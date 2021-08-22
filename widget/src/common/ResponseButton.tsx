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
        color: prefs.buttonFontColor,
        borderRadius: "0px",
        backgroundColor: prefs.buttonColor,
        marginBottom: "0.4rem",
        transion: "all ease-in-out 0.2s",
        border: "none",
        marginRight: "0.3rem",

        "&:hover": {
            color: prefs.buttonFontColor,
            backgroundColor: prefs.buttonColor,
            transition: "all ease-in-out 0.2s",
            boxShadow: theme.shadows[10],
            border: "none",
        },
    }),
    buttonFocus: (prefs: WidgetPreferences) => ({
        // color: prefs.chatFontColor,
        // borderColor: prefs.chatFontColor,
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
