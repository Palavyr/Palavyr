import { Snackbar, SnackbarOrigin, Typography, useTheme } from "@material-ui/core";
import { green, red, yellow } from "@material-ui/core/colors";
import { Alert, Color } from "@material-ui/lab";
import { SetState } from "@Palavyr-Types";
import React from "react";

export type SnackbarPositions = "tr" | "t" | "tl" | "bl" | "b" | "br";
export interface PalavyrSnackbarProps {
    successText?: string;
    warningText?: string;
    errorText?: string;
    successOpen?: boolean;
    setSuccessOpen?: SetState<boolean>;
    warningOpen?: boolean;
    setWarningOpen?: SetState<boolean>;
    errorOpen?: boolean;
    setErrorOpen?: SetState<boolean>;
    severity?: Color;
    position?: SnackbarPositions;
    autoHideDuration?: number;
}

export const PalavyrSnackbar = ({ successText, warningText, errorText, successOpen, setSuccessOpen, warningOpen, setWarningOpen, errorOpen, setErrorOpen, severity = "success", position = "br", autoHideDuration = 6000 }: PalavyrSnackbarProps) => {
    const theme = useTheme();
    let backgroundColor;
    if (severity === "success") {
        backgroundColor = green[500];
    } else if (severity === "warning") {
        backgroundColor = yellow[500];
    } else if (severity === "error") {
        backgroundColor = red[500];
    } else {
        backgroundColor = theme.palette.primary.light;
    }

    const handleClose = () => {
        if (setSuccessOpen) setSuccessOpen(false);
        if (setWarningOpen) setWarningOpen(false);
        if (setErrorOpen) setErrorOpen(false);
    };

    let anchorOrigin: SnackbarOrigin;
    switch (position) {
        case "tr":
            anchorOrigin = { vertical: "top", horizontal: "right" };
            break;

        case "t":
            anchorOrigin = { vertical: "top", horizontal: "center" };
            break;

        case "tl":
            anchorOrigin = { vertical: "top", horizontal: "left" };
            break;

        case "bl":
            anchorOrigin = { vertical: "bottom", horizontal: "left" };
            break;

        case "b":
            anchorOrigin = { vertical: "bottom", horizontal: "center" };
            break;

        case "br":
            anchorOrigin = { vertical: "bottom", horizontal: "right" };
            break;

        default:
            anchorOrigin = { vertical: "bottom", horizontal: "right" };
            break;
    }
    return (
        <>
            {successOpen && (
                <Snackbar anchorOrigin={anchorOrigin} open={successOpen} autoHideDuration={autoHideDuration} onClose={handleClose}>
                    <Alert onClose={handleClose} style={{ backgroundColor: green[500], color: theme.palette.getContrastText(green[500]) }} severity={severity} icon={false}>
                        {successText}
                    </Alert>
                </Snackbar>
            )}
            {warningOpen && (
                <Snackbar anchorOrigin={anchorOrigin} open={warningOpen} autoHideDuration={autoHideDuration} onClose={handleClose}>
                    <Alert onClose={handleClose} style={{ backgroundColor: yellow[500], color: theme.palette.getContrastText(yellow[500]), borderColor: "black" }} severity={severity} icon={false}>
                        {warningText}
                    </Alert>
                </Snackbar>
            )}
            {errorOpen && (
                <Snackbar anchorOrigin={anchorOrigin} open={errorOpen} autoHideDuration={autoHideDuration} onClose={handleClose}>
                    <Alert onClose={handleClose} style={{ backgroundColor: red[500], color: theme.palette.getContrastText(red[500]) }} severity={severity} icon={false}>
                        {errorText}
                    </Alert>
                </Snackbar>
            )}
        </>
    );
};
