import { LoginRepository } from "@common/client/LoginRepository";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { VisibilityPasswordTextField } from "@common/components/borrowed/VisibilityPasswordTextField";
import { PASSWORDS_DONT_MATCH, PASSWORD_TOO_SHORT, RESET_PASSWORD_SUCCESS } from "@constants";
import { Box, Button, makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { useCallback } from "react";
import { useRef } from "react";
import { useState } from "react";
import { Redirect, useHistory, useLocation } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
    resetButton: {
        color: "white",
        backgroundColor: "#3e5f82",
    },
    actions: {},
}));

export const RenderPasswordDialog = () => {
    const cls = useStyles();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [status, setStatus] = useState<string | null>(null);

    const location = useLocation();
    const history = useHistory();

    const searchParams = new URLSearchParams(location.search);
    const secretKey = searchParams.get("key") as string; // from url from email

    const registerPasswordRef = useRef<HTMLInputElement | null>(null);
    const registerPasswordRepeatRef = useRef<HTMLInputElement | null>(null);

    const [isPasswordVisible, setIsPasswordVisible] = useState(false);

    const onSubmit = useCallback(
        async (event) => {
            event.preventDefault();
            const client = new LoginRepository();

            const registerPassword = registerPasswordRef.current;

            if (registerPassword && registerPassword.value !== registerPassword.value) {
                setStatus(PASSWORDS_DONT_MATCH);
                return;
            }

            setStatus(null);
            setIsLoading(true);
            setTimeout(async () => {
                if (registerPassword !== null) {
                    const resetResponse = await client.Reset.ResetPassword(registerPassword.value, secretKey);
                    if (resetResponse.status) {
                        history.push(RESET_PASSWORD_SUCCESS);
                    }
                }
                setIsLoading(false);
            }, 1500);
        },
        [setIsLoading, setStatus, registerPasswordRef, registerPasswordRepeatRef]
    );

    const passwordHelperText = () => {
        if (status === PASSWORD_TOO_SHORT) {
            return "Create a password at least 6 characters long.";
        }
        if (status === PASSWORDS_DONT_MATCH) {
            return "Your passwords dont match.";
        }
        return null;
    };

    const passwordOnChange = () => {
        if (status === PASSWORD_TOO_SHORT || status === PASSWORDS_DONT_MATCH) {
            setStatus(null);
        }
    };

    return (
        <div style={{ display: "flex", justifyContent: "center", width: "100%", paddingTop: "5%" }}>
            <form style={{ width: "400px" }} onSubmit={onSubmit}>
                <Typography align="center" variant="h5">
                    Choose a new password
                </Typography>
                <div>
                    <VisibilityPasswordTextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        error={status === "passwordTooShort" || status === "passwordsDontMatch"}
                        label="Password"
                        inputRef={registerPasswordRef}
                        autoComplete="off"
                        onChange={passwordOnChange}
                        FormHelperTextProps={{ error: true }}
                        isVisible={isPasswordVisible}
                        onVisibilityChange={setIsPasswordVisible}
                    />
                    <VisibilityPasswordTextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        error={status === "passwordTooShort" || status === "passwordsDontMatch"}
                        label="Repeat Password"
                        inputRef={registerPasswordRepeatRef}
                        autoComplete="off"
                        onChange={passwordOnChange}
                        helperText={passwordHelperText()}
                        FormHelperTextProps={{ error: true }}
                        isVisible={isPasswordVisible}
                        onVisibilityChange={setIsPasswordVisible}
                    />
                </div>
                <Box width="100%" className={cls.actions}>
                    <Button className={cls.resetButton} type="submit" fullWidth variant="contained" size="large" disabled={isLoading}>
                        Submit
                        {isLoading && <ButtonCircularProgress />}
                    </Button>
                </Box>
            </form>
        </div>
    );
};
