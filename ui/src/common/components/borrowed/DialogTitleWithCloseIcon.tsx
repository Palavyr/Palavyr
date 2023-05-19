import React, { useEffect, useState } from "react";
import { DialogTitle, Box, useTheme, Button, makeStyles } from "@material-ui/core";
import { useHistory, useLocation } from "react-router-dom";
import { PalavyrText } from "../typography/PalavyrTypography";
import classNames from "classnames";

export interface LoginAndRegisterButtonsProps {
    disablePadding?: boolean;
    paddingBottom?: number;
}

type StyleProps = {
    loginDisabled: boolean;
    registerDisabled: boolean;
};

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
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
    registerButton: (props: StyleProps) => ({
        borderBottom: props.registerDisabled ? "5px solid black" : "none",
    }),
    loginButton: (props: StyleProps) => ({
        borderBottom: props.loginDisabled ? "5px solid black" : "none",
    }),
}));

export const LoginAndRegisterButtons = ({ paddingBottom, disablePadding }: LoginAndRegisterButtonsProps) => {
    const theme = useTheme();
    const history = useHistory();
    const loginDisabled = useLocationBasedButtonState(["/login", "/"]);
    const registerDisabled = useLocationBasedButtonState(["/signup"]);
    const cls = useStyles({ loginDisabled, registerDisabled });

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
                <Button disabled={loginDisabled} disableElevation variant="contained" className={classNames(cls.button, cls.loginButton)} size="medium" onClick={() => history.push("/login")}>
                    <PalavyrText variant="h5" className={cls.menuButtonText}>
                        Log In
                    </PalavyrText>
                </Button>
                <Button disabled={registerDisabled} disableElevation variant="contained" className={classNames(cls.button, cls.registerButton)} size="medium" onClick={() => history.push("/signup")}>
                    <PalavyrText variant="h5" className={cls.menuButtonText}>
                        Sign Up
                    </PalavyrText>
                </Button>
            </Box>
        </DialogTitle>
    );
};

const useLocationBasedButtonState = (paths: string[]) => {
    const [disabled, setDisabled] = useState(false);
    const location = useLocation();

    useEffect(() => {
        setDisabled(paths.includes(location.pathname));
    }, [location]);

    return disabled;
};
