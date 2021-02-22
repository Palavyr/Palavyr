import React, { useState, useCallback, useEffect } from "react";
import { ApiClient } from "@api-client/Client";
import { useHistory, useParams } from "react-router-dom";
import { Divider, Grid, makeStyles } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { AlertDetails, Settings } from "@Palavyr-Types";
import { Alert, AlertTitle } from "@material-ui/lab";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { OsTypeToggle } from "./enableAreas/OsTypeToggle";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
    titleText: {
        fontWeight: "bold",
    },
    alert: {
        border: "2px solid black",
    },
    alertTitle: {
        display: "flex",
        color: "white",
        justifyContent: "center",
        textAlign: "left",
    },
}));

export const AreaSettings = () => {
    var client = new ApiClient();
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const { setIsLoading } = React.useContext(DashboardContext);

    const [loaded, setLoaded] = useState<boolean>(false);
    const [alertState, setAlertState] = useState<boolean>(false);
    const [settings, setSettings] = useState<Partial<Settings>>({
        emailAddress: "",
        isVerified: false,
        awaitingVerification: false,
        areaName: "",
        areaTitle: "",
        subject: "",
        isEnabled: false,
    });
    const [alertDetails, setAlertDetails] = useState<AlertDetails>({ title: "", message: "" });
    const [isEnabledState, setIsEnabledState] = useState<boolean | null>(null);

    const classes = useStyles();
    const history = useHistory();

    const loadSettings = useCallback(async () => {
        setIsLoading(true);
        var { data: areaData } = await client.Area.GetArea(areaIdentifier);
        setSettings({
            emailAddress: areaData.areaSpecificEmail,
            isVerified: areaData.emailIsVerified,
            awaitingVerification: areaData.awaitingVerification,
            areaName: areaData.areaName,
            areaTitle: areaData.areaDisplayTitle,
            subject: areaData.subject,
            isEnabled: areaData.isEnabled,
        });
        setIsEnabledState(areaData.isEnabled);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        if (!loaded) loadSettings();

        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, []);

    const handleAreaNameChange = async (newAreaName: string) => {
        if (newAreaName === settings.areaName) return;
        var { data: updatedAreaName } = await client.Area.updateAreaName(areaIdentifier, newAreaName);
        setSettings({ ...settings, areaName: updatedAreaName });
        window.location.reload(); // reloads the sidebar...
    };

    // const handleSubjectChange = async (newSubject: string) => {
    //     if (newSubject === settings.subject) return;
    //     const { data: updatedSubject } = await client.Configuration.Email.UpdateSubject(areaIdentifier, newSubject);
    //     setSettings({ ...settings, subject: updatedSubject });
    //     window.location.reload();
    // };

    const handleAreaDisplayTitleChange = async (newAreaDisplayTitle: any) => {
        if (newAreaDisplayTitle === settings.areaTitle) return;
        const { data: updatedDisplayTitle } = await client.Area.updateDisplayTitle(areaIdentifier, newAreaDisplayTitle);
        window.location.reload();
        setSettings({ ...settings, areaTitle: updatedDisplayTitle });
    };

    const handleAreaDelete = async () => {
        await client.Area.deleteArea(areaIdentifier);
        history.push("/dashboard");
        window.location.reload();
    };

    const verifyEmailAddress = async (newEmailAddress: string) => {
        const { data: emailVerification } = await client.Settings.EmailVerification.RequestEmailVerification(newEmailAddress, areaIdentifier);
        setAlertDetails({ title: emailVerification.title, message: emailVerification.message });
        setAlertState(true);
        if (!(emailVerification.status === "Failed")) setSettings({ ...settings, emailAddress: newEmailAddress });
    };

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

    const onAreaEnabledToggleChange = async () => {
        const { data: updatedisEnabled } = await client.Area.UpdateIsEnabled(!isEnabledState, areaIdentifier);
        setIsEnabledState(updatedisEnabled);
    };

    return loaded ? (
        <>
            <AreaConfigurationHeader title="Area Settings" subtitle={`Modify settings that are specific to this area (${settings.areaName}).`} />
            {isEnabledState !== null && <OsTypeToggle controlledState={isEnabledState} onChange={onAreaEnabledToggleChange} enabledLabel="Area Enabled" disabledLabel="Area Disabled" />}

            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} variant="filled" severity="info">
                        <AlertTitle className={classes.titleText}>Important Settings</AlertTitle>
                        These options affect the appearance and behavior of the widget.
                    </Alert>
                </Grid>
                <Grid item xs={5}>
                    <SettingsGridRowText
                        fullWidth
                        alertNode={
                            <Alert className={classes.alert} severity={settings.areaTitle === "Change this in the area Settings." || settings.areaTitle === "" ? "error" : "success"}>
                                <AlertTitle className={classes.titleText}>Update Widget Display Name</AlertTitle>
                                Set the name of this area as used in the widget.
                            </Alert>
                        }
                        title=""
                        name=""
                        details="Update the area title used in the widget."
                        placeholder="New Area Name (Widget)"
                        currentValue={settings.areaTitle}
                        onClick={handleAreaDisplayTitleChange}
                        clearVal={false}
                    />
                </Grid>

                <Grid item xs={5}>
                    <SettingsGridRowText
                        fullWidth
                        inputType="email"
                        alertNode={
                            <Alert className={classes.alert} severity={emailSeverity()}>
                                <AlertTitle className={classes.titleText}>{settings.isVerified ? "Email Verified" : "Verify the email used to send responses for this area"}</AlertTitle>
                                Submit a new email to be used for responses.
                            </Alert>
                        }
                        title=""
                        name=""
                        details=""
                        buttonText="Update and Verify"
                        placeholder="New Email Address"
                        currentValue={settings.emailAddress}
                        onClick={verifyEmailAddress}
                        clearVal={false}
                    />
                </Grid>
                {/* <Grid item xs={8}>
                    <SettingsGridRowText
                        fullWidth
                        inputType="text"
                        alertNode={
                            <Alert className={classes.alert} severity="warning">
                                <AlertTitle className={classes.titleText}>Response Email Subject Line</AlertTitle>
                                Configure a subject line used with respones emails sent from this area.
                            </Alert>
                        }
                        title=""
                        name=""
                        details=""
                        buttonText="Update"
                        placeholder="New Response Email Subject"
                        currentValue={settings.subject}
                        onClick={handleSubjectChange}
                        clearVal={false}
                    />
                </Grid> */}
            </Grid>
            <Divider />
            <br></br>

            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} variant="filled" severity="warning">
                        <AlertTitle className={classes.titleText}>Dashboard Specific Options</AlertTitle>
                        These options only affect what you see in the dashboard.
                    </Alert>
                </Grid>

                <Grid item xs={5}>
                    <SettingsGridRowText
                        fullWidth
                        alertNode={
                            <Alert className={classes.alert} severity={settings.areaName ? "success" : "warning"}>
                                <AlertTitle className={classes.titleText}>Update Dashboard Display Name</AlertTitle>
                                Set the name of area used for your reference on this dashboard.
                            </Alert>
                        }
                        title=""
                        name=""
                        details=" Update the name of this area for dashboard."
                        placeholder="New Area Name (Dashboard)"
                        currentValue={settings.areaName}
                        onClick={handleAreaNameChange}
                        clearVal={false}
                    />
                </Grid>
            </Grid>
            <Divider />
            <br></br>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} severity="error" variant="filled">
                        <AlertTitle className={classes.titleText}>DANGER ZONE</AlertTitle>
                        WAIT! These options are permanent.
                    </Alert>
                </Grid>
                <Grid item xs={5}>
                    <SettingsGridRowText
                        alertNode={
                            <Alert className={classes.alert} severity="error">
                                <AlertTitle className={classes.titleText}>Permanently DELETE</AlertTitle>
                                CAREFUL! Use this option to delete this area (and all associated data) forever.
                            </Alert>
                        }
                        title="Permanently Delete Area"
                        name="Delete Area"
                        details="Permanently delete this area."
                        onClick={handleAreaDelete}
                        clearVal={false}
                        buttonText="Permanently Delete"
                    />
                </Grid>
            </Grid>
            {alertState && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
        </>
    ) : null;
};
