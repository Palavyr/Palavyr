import React from "react";
import { DialogTitle, Box, useTheme, Button, makeStyles } from "@material-ui/core";
import { PalavyrText } from "../typography/PalavyrTypography";
import { useHistory } from "react-router-dom";
import { Align } from "@common/positioning/Align";
import classNames from "classnames";

export interface LoginAndRegisterButtonsProps {
    disablePadding?: boolean;
    paddingBottom?: number;
}

const useStyles = makeStyles(theme => ({
    navButtons: {
        display: "flex",
        justifyContent: "space-evenly",
        verticalAlign: "middle",
    },
    menuButtonText: {
        color: theme.palette.common.black,
        "&:hover": {
            color: theme.palette.primary.dark,
        },
    },
    button: {
        backgroundColor: "lightgray",
        border: "0px",
        "&:hover": {
            boxShadow: theme.shadows[5],
        },
    },
}));

export const LoginAndRegisterButtons = ({ paddingBottom, disablePadding }: LoginAndRegisterButtonsProps) => {
    const theme = useTheme();
    const history = useHistory();
    const cls = useStyles();
    var dialogTitleStyles = {
        paddingBottom: paddingBottom ? (paddingBottom && disablePadding ? 0 : paddingBottom) : theme.spacing(3),
        paddingTop: disablePadding ? 0 : theme.spacing(2),
        width: "100%",
    };

    if (disablePadding) {
        dialogTitleStyles = {
            ...dialogTitleStyles,
            ...{
                paddingLeft: 0,
                paddingRight: 0,
            },
        };
    }

    return (
        <DialogTitle style={dialogTitleStyles}>
            <Box display="flex" justifyContent="space-around">
                <Button disableElevation variant="contained" className={cls.button} size="medium" onClick={() => history.push("/login")}>
                    <PalavyrText variant="h5" className={cls.menuButtonText}>
                        Log In
                    </PalavyrText>
                </Button>
                <Button disableElevation variant="contained" className={cls.button} size="medium" onClick={() => history.push("/signup")}>
                    <PalavyrText variant="h5" className={cls.menuButtonText}>
                        Sign Up
                    </PalavyrText>
                </Button>
            </Box>
        </DialogTitle>
    );
};
