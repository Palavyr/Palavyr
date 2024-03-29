import * as React from "react";
import { Button, makeStyles, PropTypes } from "@material-ui/core";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { useContext } from "react";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

export interface IResponseButton {
    onClick?(): void;
    onSubmit?(e: { preventDefault: () => void }): void;
    disabled?: boolean;
    text?: string;
    color?: PropTypes.Color;
    variant?: "outlined" | "contained";
    type?: "button" | "submit";
}

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    button: (prefs: WidgetPreferencesResource) => ({
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        color: prefs.buttonFontColor,
        borderRadius: "0px",
        backgroundColor: prefs.buttonColor,
        marginBottom: "0.7rem",
        transion: "all ease-in-out 0.2s",
        border: "none",
        marginRight: "0.3rem",

        fontFamily: prefs.fontFamily,
        "&:hover": {
            color: prefs.buttonFontColor,
            backgroundColor: prefs.buttonColor,
            transition: "all ease-in-out 0.2s",
            boxShadow: theme.shadows[10],
            border: "none",
        },
    }),
}));

export const ResponseButton = ({ onClick, onSubmit, disabled = false, variant = "outlined", text = "Submit", type = "button" }: IResponseButton) => {
    const { preferences } = useContext(WidgetContext);

    const cls = useStyles(preferences);
    return (
        <Button type={type} disableElevation className={cls.button} disabled={disabled} variant={variant} size="small" onClick={onClick} onSubmit={onSubmit}>
            {text}
        </Button>
    );
};
