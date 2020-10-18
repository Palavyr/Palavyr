import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Grid, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { Alert, AlertTitle } from "@material-ui/lab";
import { AlertDetails, EmailVerificationResponse } from "@Palavyr-Types";

type GeneralSettings = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
}

const useStyles = makeStyles(theme => ({
    titleText: {
        fontWeight: "bold"
    }
}))


type AccountEmailSettingsResponse = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification;
}

export const ChangeEmail = () => {
    var client = new ApiClient();
    const classes = useStyles();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [settings, setSettings] = useState<GeneralSettings>({
        emailAddress: "",
        isVerified: false,
        awaitingVerification: false,
    })
    const [alertState, setAlertState] = useState<boolean>(false);
    const [alertDetails, setAlertDetails] = useState<AlertDetails>({ title: "", message: "" })

    const loadEmail = useCallback(async () => {
        var settingsResponse = (await client.Settings.Account.getEmail()).data as AccountEmailSettingsResponse;
        setSettings({
            emailAddress: settingsResponse.emailAddress,
            isVerified: settingsResponse.isVerified,
            awaitingVerification: settingsResponse.awaitingVerification
        })
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {

        loadEmail();
        setLoaded(true);

        return () => {
            setLoaded(false)
        }
    }, [setSettings, loadEmail])


    const emailSeverity = (): "success" | "warning" | "error" | "info" | undefined => {
        let severityLevel: "success" | "warning" | "error" | "info" | undefined;

        if (settings.isVerified) {
            severityLevel = "success";
        } else {
            if (settings.awaitingVerification) {
                severityLevel = "warning";
            } else {
                severityLevel = "error"
            }
        }
        return severityLevel;
    }

    const verifyEmailAddress = async (newEmailAddress: string) => {
        if (newEmailAddress === settings.emailAddress) return
        const res = (await client.Settings.Account.updateEmail(newEmailAddress)).data as EmailVerificationResponse;
        setAlertDetails({ title: res.title, message: res.message })
        setAlertState(true);
        if (!(res.status === "Failed")) setSettings({ ...settings, emailAddress: newEmailAddress })
    }

    return (
        <>
            <Grid container spacing={3}>
                <SettingsGridRowText
                    fullWidth
                    inputType="email"
                    alertNode={
                        <Alert severity={emailSeverity()}>
                            <AlertTitle className={classes.titleText}>
                                {
                                    settings.isVerified
                                        ? "Default Email Verified"
                                        : "Verify the default email address used to send responses."
                                }
                            </AlertTitle>
                            Submit a new email to be used for responses. This requires responding to a verification email being sent to your email address by Amazon Web Services.
                            <br></br>
                            To verify, click the verifiation link send to your inbox. The email will use the subject line:
                            <strong>Amazon Web Services – Email Address Verification Request</strong>
                        </Alert>
                    }
                    placeholder="New Email"
                    onClick={verifyEmailAddress}
                    clearVal={true}
                    currentValue={settings.emailAddress}
                    buttonText="Update and Verify"
                />
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlertState} alert={alertDetails} />}
        </>
    )
}