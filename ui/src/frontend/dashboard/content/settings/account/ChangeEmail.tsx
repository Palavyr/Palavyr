import React, { useState, useCallback, useEffect, useContext } from "react";
import { CircularProgress, Divider, List, ListItem, ListItemText, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { Alert, AlertTitle } from "@material-ui/lab";
import { AlertDetails } from "@Palavyr-Types";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

type GeneralSettings = {
    emailAddress: string;
    isVerified: boolean;
    awaitingVerification: boolean;
};

const useStyles = makeStyles(theme => ({
    titleText: {
        fontWeight: "bold",
    },
}));

export const ChangeEmail = () => {
    const { repository } = useContext(DashboardContext);
    const classes = useStyles();

    const [loaded, setLoaded] = useState<boolean>(false);
    const [settings, setSettings] = useState<GeneralSettings>({
        emailAddress: "",
        isVerified: false,
        awaitingVerification: false,
    });
    const [alertState, setAlertState] = useState<boolean>(false);
    const [alertDetails, setAlertDetails] = useState<AlertDetails>({ title: "", message: "" });

    const loadEmail = useCallback(async () => {
        const { emailAddress, isVerified, awaitingVerification } = await repository.Settings.Account.getEmail();
        setSettings({
            emailAddress: emailAddress,
            isVerified: isVerified,
            awaitingVerification: awaitingVerification,
        });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        loadEmail();
        setLoaded(true);

        return () => {
            setLoaded(false);
        };
    }, [setSettings, loadEmail]);

    const emailSeverity = (): "success" | "warning" | "error" | "info" | undefined => {
        let severityLevel: "success" | "warning" | "error" | "info" | undefined;

        if (settings.isVerified) {
            severityLevel = "success";
        } else {
            if (settings.awaitingVerification) {
                severityLevel = "warning";
            } else {
                severityLevel = "error";
            }
        }
        return severityLevel;
    };

    const verifyEmailAddress = async (newEmailAddress: string) => {
        const res = await repository.Settings.Account.updateEmail(newEmailAddress);
        setAlertDetails({ title: res.title, message: res.message });
        setAlertState(true);
        if (!(res.status === "Failed")) setSettings({ ...settings, emailAddress: newEmailAddress });
    };

    const SettingsVerifiedText = () => {
        return (
            <>
                <PalavyrText variant="body1" gutterBottom>
                    Your primary email address has already been verified. This address will be used as the 'From' address when sending email responses from your widget, unless you specify a different email in
                    the intent settings.
                </PalavyrText>
                <PalavyrText paragraph variant="body1">
                    You can submit a new email to be used for responses at any time. This requires responding to a verification email being sent to your email address by Amazon Web Services.
                </PalavyrText>
            </>
        );
    };

    const SettingsUnverifiedText = () => {
        return (
            <>
                <PalavyrText paragraph variant="body1">
                    Submit a new email to be used for responses. This requires responding to a verification email being sent to your email address by Amazon Web Services.
                </PalavyrText>
                <PalavyrText paragraph variant="body1">
                    To verify, click the verifiation link send to your inbox. The email will use the subject line:<br></br>
                </PalavyrText>
                <PalavyrText paragraph gutterBottom>
                    <strong>Amazon Web Services – Email Address Verification Request</strong>
                </PalavyrText>
                <List>
                    <ListItem>
                        <ListItemText>
                            <PalavyrText>
                                <Alert severity="warning">
                                    This link will expire in <strong>24 hours</strong>.
                                </Alert>
                            </PalavyrText>
                        </ListItemText>
                    </ListItem>
                </List>
                <List>
                    <ListItem>
                        <ListItemText>
                            <PalavyrText>
                                <Alert severity="warning">
                                    When updating your response email address, it is recommended to disable the widget from your website until you have verified that your address is valid. Otherwise your end
                                    users might fail to receive their response PDF.
                                </Alert>
                            </PalavyrText>
                        </ListItemText>
                    </ListItem>
                </List>
            </>
        );
    };

    return (
        <>
            <SettingsWrapper>
                <HeaderStrip title="Primary Email" subtitle="Change the primary email address used to send emails to your customers. This is also the email address used for billing." />
                <Divider />
                {
                    <SettingsGridRowText
                        loading={settings.emailAddress == ""}
                        fullWidth
                        inputType="email"
                        alertNode={
                            <Alert severity={emailSeverity()}>
                                <AlertTitle className={classes.titleText}>{settings.isVerified ? "Primary Email Verified" : "Verify the primary email address used to send responses."}</AlertTitle>
                                {settings.isVerified ? <SettingsVerifiedText /> : <SettingsUnverifiedText />}
                            </Alert>
                        }
                        placeholder="New Email"
                        onClick={verifyEmailAddress}
                        clearVal={true}
                        currentValue={settings.emailAddress}
                        buttonText="Update and Send Verification Email"
                    />
                }
            </SettingsWrapper>
            {alertState && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
        </>
    );
};
