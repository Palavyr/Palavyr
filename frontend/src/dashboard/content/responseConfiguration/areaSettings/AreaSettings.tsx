import React, { useState, useCallback, useEffect } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { useHistory, useParams } from "react-router-dom";
import { Grid, makeStyles, Typography, useTheme } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { AlertDetails, Settings } from "@Palavyr-Types";
import { Alert, AlertTitle } from "@material-ui/lab";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { OsTypeToggle } from "./enableAreas/OsTypeToggle";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SessionStorage } from "localStorage/sessionStorage";
import { CacheIds } from "@api-client/AxiosClient";

const useStyles = makeStyles((theme) => ({
    alert: {
        borderTop: `2px solid ${theme.palette.common.black}`,
        borderBottom: `2px solid ${theme.palette.common.black}`,
    },
    alertTitle: {
        display: "flex",
        color: "white",
        justifyContent: "center",
        textAlign: "left",
    },
}));

export const AreaSettings = () => {
    const repository = new PalavyrRepository();
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
        const areas = await repository.Area.GetAreas();
        const areaData = areas.filter((x) => x.areaIdentifier === areaIdentifier)[0];

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
    }, [areaIdentifier]);

    useEffect(() => {
        if (!loaded) {
            loadSettings();
        }

        setLoaded(true);
        return () => {
            setLoaded(false);
        };
    }, []);

    const handleAreaNameChange = async (newAreaName: string) => {
        if (newAreaName === settings.areaName) return;
        const updatedAreaName = await repository.Area.updateAreaName(areaIdentifier, newAreaName);
        const updatedSettings = { ...settings, areaName: updatedAreaName };
        setSettings(updatedSettings);
        window.location.reload(); // reloads the sidebar...
    };

    const handleAreaDisplayTitleChange = async (newAreaDisplayTitle: any) => {
        if (newAreaDisplayTitle === settings.areaTitle) return;
        const updatedDisplayTitle = await repository.Area.updateDisplayTitle(areaIdentifier, newAreaDisplayTitle);
        window.location.reload();
        const updatedSettings = { ...settings, areaTitle: updatedDisplayTitle };
        setSettings(updatedSettings);
    };

    const handleAreaDelete = async () => {
        await repository.Area.deleteArea(areaIdentifier);
        history.push("/dashboard");
        window.location.reload();
    };

    const verifyEmailAddress = async (newEmailAddress: string) => {
        const emailVerification = await repository.Settings.EmailVerification.RequestEmailVerification(newEmailAddress, areaIdentifier);
        setAlertDetails({ title: emailVerification.title, message: emailVerification.message });
        setAlertState(true);
        if (!(emailVerification.status === "Failed")) {
            const updatedSettings = { ...settings, emailAddress: newEmailAddress };
            setSettings(updatedSettings);
        }
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
        const updatedisEnabled = await repository.Area.UpdateIsEnabled(!isEnabledState, areaIdentifier);
        setIsEnabledState(updatedisEnabled);
    };

    const theme = useTheme();

    return loaded ? (
        <>
            <AreaConfigurationHeader title="Area Settings" subtitle={`Modify settings that are specific to this area (${settings.areaName}).`} />
            {isEnabledState !== null && <OsTypeToggle controlledState={isEnabledState} onChange={onAreaEnabledToggleChange} enabledLabel="Area Enabled" disabledLabel="Area Disabled" />}

            <Grid container spacing={3} justify="center">
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} variant="filled" severity="info">
                        <AlertTitle>
                            <Typography variant="h5">Important Settings</Typography>
                        </AlertTitle>
                        These options affect the appearance and behavior of the widget.
                    </Alert>
                </Grid>
                <Grid item xs={5}>
                    <SettingsGridRowText
                        fullWidth
                        alertNode={
                            <Alert className={classes.alert} severity={settings.areaTitle === "Change this in the area Settings." || settings.areaTitle === "" ? "error" : "success"}>
                                <AlertTitle>
                                    <Typography variant="h5">Update Widget Display Name</Typography>
                                </AlertTitle>
                                Set the name of this area as used in the widget.
                            </Alert>
                        }
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
                                <AlertTitle>
                                    <Typography variant="h5">{settings.isVerified ? "Email Verified" : "Verify the email used to send responses for this area"}</Typography>
                                </AlertTitle>
                                Submit a new email to be used for responses.
                            </Alert>
                        }
                        buttonText="Update and Verify"
                        placeholder="New Email Address"
                        currentValue={settings.emailAddress}
                        onClick={verifyEmailAddress}
                        clearVal={false}
                    />
                </Grid>
            </Grid>
            <br></br>

            <Grid container spacing={3} justify="center">
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} style={{ backgroundColor: theme.palette.warning.dark }} variant="filled" severity="warning">
                        <AlertTitle>
                            <Typography variant="h5">Dashboard Specific Options</Typography>
                        </AlertTitle>
                        These options only affect what you see in the dashboard.
                    </Alert>
                </Grid>

                <Grid item xs={5}>
                    <SettingsGridRowText
                        fullWidth
                        alertNode={
                            <Alert className={classes.alert} severity={settings.areaName ? "success" : "warning"}>
                                <AlertTitle>
                                    <Typography variant="h5">Update Dashboard Display Name</Typography>
                                </AlertTitle>
                                Set the name of area used for your reference on this dashboard.
                            </Alert>
                        }
                        placeholder="New Area Name (Dashboard)"
                        currentValue={settings.areaName}
                        onClick={handleAreaNameChange}
                        clearVal={false}
                    />
                </Grid>
            </Grid>
            <br></br>
            <Grid container spacing={3} justify="center">
                <Grid item xs={12}>
                    <Alert className={classNames(classes.alert, classes.alertTitle)} severity="error" variant="filled">
                        <AlertTitle>
                            <Typography variant="h5">DANGER ZONE</Typography>
                        </AlertTitle>
                        WAIT! These options are permanent.
                    </Alert>
                </Grid>
                <Grid item xs={5}>
                    <SettingsGridRowText
                        successText="Area Deleted"
                        alertNode={
                            <Alert className={classes.alert} severity="error">
                                <AlertTitle>
                                    <Typography variant="h5">Permanently DELETE</Typography>
                                </AlertTitle>
                                CAREFUL! Use this option to delete this area (and all associated data) forever.
                            </Alert>
                        }
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
