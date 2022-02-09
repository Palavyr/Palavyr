import * as React from "react";
import classNames from "classnames";
import { Card, FormControl, OutlinedInput, makeStyles } from "@material-ui/core";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { useContext, useState } from "react";
import auth from "@auth/Auth";
import { SessionStorage } from "@localStorage/sessionStorage";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { Align } from "@common/positioning/Align";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { PalavyrSnackbar } from "@common/components/PalavyrSnackbar";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { ALL_COOKIE_NAMES } from "@constants";
import Cookies from "js-cookie";
import Auth from "@auth/Auth";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles(theme => ({
    contentRoot: {
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
        display: "flex",
        flexDirection: "row",
        justifyContent: "center",
    },
    inputLabel: { background: "white", borderRadius: "5px", boxShadow: "0px 0px 12px 5px white" },
    outlinedInput: { background: "white", width: "35ch", marginRight: "1rem" },
    alignment: {
        justifyContent: "space-around",
        display: "flex",
        flexDirection: "row",
        marginTop: "4rem",
    },
}));

export const PleaseConfirmYourEmail = () => {
    const { repository } = useContext(DashboardContext);
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
    const history = useHistory();
    const cls = useStyles();

    const confirmAccount = async () => {
        setIsLoading(true);
        if (isNullOrUndefinedOrWhitespace(authToken)) {
            setIsLoading(false);
            setErrorOpen(true);
            return false;
        }
        try {
            const emailConfirmed = await repository.Settings.Account.confirmEmailAddress(authToken);
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
        } catch {
            setIsLoading(false);
        }
    };

    const resendAuthToken = async () => {
        setResendIsLoading(true);
        if (emailAddress) {
            const resendResult = await repository.Settings.Account.resendConfirmationToken(emailAddress);
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

    const handleAccountDelete = async () => {
        if (emailAddress) {
            await repository.Settings.Account.CancelRegistration(emailAddress);
            Auth.ClearAuthentication();
            ALL_COOKIE_NAMES.forEach((x: string) => Cookies.remove(x));
            history.push("/");
        }
    };

    return (
        <>
            <div className={classNames(cls.contentRoot, cls.centerText)}>
                <div style={{ width: "100%" }}>
                    <HeaderStrip title="Confirm your email" subtitle={`Please provide the access token emailed to: ${emailAddress}`} />
                    <Align direction="center">
                        <Card className={cls.card}>
                            <FormControl fullWidth className={cls.margin} variant="outlined">
                                <OutlinedInput className={cls.outlinedInput} placeholder="Confirmation Token" value={authToken} id="outlined-adornment-confirmation-code" onChange={outlinedInputOnChange} />
                                <ColoredButton type="submit" variant="contained" color="primary" disabled={isLoading} onClick={confirmAccount}>
                                    Submit
                                    {isLoading && <ButtonCircularProgress />}
                                </ColoredButton>
                            </FormControl>
                            <div className={cls.alignment}>
                                <div>
                                    <ColoredButton type="submit" variant="contained" color="primary" disabled={resendIsLoading} onClick={resendAuthToken}>
                                        Resend Confirmation Token
                                        {resendIsLoading && <ButtonCircularProgress />}
                                    </ColoredButton>
                                </div>
                                <div>
                                    <ColoredButton type="submit" variant="contained" color="primary" disabled={resendIsLoading} onClick={handleAccountDelete}>
                                        Cancel Registration
                                        {resendIsLoading && <ButtonCircularProgress />}
                                    </ColoredButton>
                                </div>
                            </div>
                        </Card>
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
