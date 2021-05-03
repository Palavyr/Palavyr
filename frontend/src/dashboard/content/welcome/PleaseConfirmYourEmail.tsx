import * as React from "react";
import { ApiClient } from "@api-client/Client";
import classNames from "classnames";
import { Card, Typography, FormControl, OutlinedInput, makeStyles, useTheme, Divider } from "@material-ui/core";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useState } from "react";
import auth from "auth/Auth";
import { SessionStorage } from "localStorage/sessionStorage";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { Align } from "dashboard/layouts/positioning/Align";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { PalavyrSnackbar } from "@common/components/PalavyrSnackbar";

const useStyles = makeStyles((theme) => ({
    contentRoot: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: "transparent",
        margin: "0px",
    },
    card: {
        backgroundColor: theme.palette.secondary.light,
        width: "50%",
        padding: "3rem",
        margin: "3rem",
    },
    centerText: {
        textAlign: "center",
    },
    margin: {
        margin: theme.spacing(1),
    },
    inputLabel: { background: "white", borderRadius: "5px", boxShadow: "0px 0px 12px 5px white" },
    outlinedInput: { background: "white" },
}));

export const PleaseConfirmYourEmail = () => {
    const client = new ApiClient();
    const [authToken, setAuthToken] = useState<string>("");
    const [, setAuthStatus] = useState<string | null>(null);
    const emailAddress = SessionStorage.getEmailAddress();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [warningOpen, setWarningOpen] = useState<boolean>(false);
    const [errorOpen, setErrorOpen] = useState<boolean>(false);
    const [successOpen, setSuccessOpen] = useState<boolean>(false);
    const [resentOpen, setResentOpen] = useState<boolean>(false);
    const [resendFailed, setResendFailed] = useState<boolean>(false);
    const [resendIsLoading, setResendIsLoading] = useState<boolean>(false);
    const cls = useStyles();

    const confirmAccount = async () => {
        setIsLoading(true);
        if (isNullOrUndefinedOrWhitespace(authToken)) {
            setIsLoading(false);
            setErrorOpen(true);
            return false;
        }
        const { data: emailConfirmed } = await client.Settings.Account.confirmEmailAddress(authToken);
        setTimeout(() => {
            if (emailConfirmed === true) {
                setSuccessOpen(true);
                auth.SetIsActive();
                window.location.reload();
            } else {
                setWarningOpen(true);
                setAuthStatus("CodeNotVerified");
            }
            setIsLoading(false);
        }, 1300);
    };

    const resendAuthToken = async () => {
        setResendIsLoading(true);
        if (emailAddress) {
            const { data: resendResult } = await client.Settings.Account.resendConfirmationToken(emailAddress);
            if (resendResult) {
                setResentOpen(true);
            } else {
                setResendFailed(true);
            }
        }
        setResendIsLoading(false);
    };

    const outlinedInputOnChange = (event: { target: { value: React.SetStateAction<string> } }) => {
        setAuthToken(event.target.value);
    };

    return (
        <>
            <div className={classNames(cls.contentRoot, cls.centerText)}>
                <div style={{ width: "100%" }}>
                    <AreaConfigurationHeader title="Confirm your email" subtitle={`Please provide the access token emailed to: ${emailAddress}`} />
                    <Align>
                        <Card className={cls.card}>
                            <FormControl fullWidth className={cls.margin} variant="outlined">
                                <OutlinedInput className={cls.outlinedInput} placeholder="Paste your confirmation code here" value={authToken} id="outlined-adornment-confirmation-code" onChange={outlinedInputOnChange} />
                                <br />
                                <Align direction="flex-end">
                                    <ColoredButton type="submit" variant="contained" color="primary" disabled={isLoading} onClick={confirmAccount}>
                                        Submit
                                        {isLoading && <ButtonCircularProgress />}
                                    </ColoredButton>
                                </Align>
                            </FormControl>
                        </Card>
                    </Align>
                    <Align>
                        <Align direction="center">
                            <div>
                                <Typography variant="body2" gutterBottom>
                                    If you need us to resend the confirmation token to your email address, click the button below
                                </Typography>
                                <br></br>
                                <ColoredButton type="submit" variant="contained" color="primary" disabled={resendIsLoading} onClick={resendAuthToken}>
                                    Resend Confirmation Token
                                    {resendIsLoading && <ButtonCircularProgress />}
                                </ColoredButton>
                            </div>
                        </Align>
                    </Align>
                </div>
            </div>
            <PalavyrSnackbar successText="Thank you!" position="br" severity="success" successOpen={successOpen} setSuccessOpen={setSuccessOpen} />
            <PalavyrSnackbar successText="A new confirmation token has been sent!" severity="success" successOpen={resentOpen} setSuccessOpen={setResentOpen} />
            <PalavyrSnackbar warningText="The confirmation code provided was not valid." severity="warning" warningOpen={warningOpen} setWarningOpen={setWarningOpen} position="br" />
            <PalavyrSnackbar errorText="Please provide a confirmation token." severity="error" errorOpen={errorOpen} setErrorOpen={setErrorOpen} position="br" />
            <PalavyrSnackbar errorText="Appologies, we are unable to resend the confirmation token at this time." severity="error" errorOpen={resendFailed} setErrorOpen={setResendFailed} position="br" />
        </>
    );
};
