import * as React from "react";
import { Button, makeStyles, PropTypes } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { useContext } from "react";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

export interface IResponseButton {
    onClick?(): void;
    onSubmit?(e: { preventDefault: () => void }): void;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
    type?: "button" | "submit"
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

export const ResponseButton = ({ onClick, onSubmit, disabled = false, variant = "outlined", text = "Submit", type = "button" }: IResponseButton) => {
    const { preferences } = useContext(WidgetContext);

    const cls = useStyles(preferences);
    return (
        <Button type={type} disableElevation focusVisibleClassName={cls.buttonFocus} className={cls.button} disabled={disabled} variant={variant} size="small" onClick={onClick} onSubmit={onSubmit}>
            {text}
        </Button>
    );
};
