import React, { useState, useCallback, useEffect, useContext } from "react";
import { useHistory, useParams } from "react-router-dom";
import { Button, Dialog, Grid, makeStyles, Typography, useTheme } from "@material-ui/core";
import { SettingsGridRowText } from "@common/components/SettingsGridRowText";
import { AlertDetails, Settings } from "@Palavyr-Types";
import { Alert, AlertTitle } from "@material-ui/lab";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { OsTypeToggle } from "./enableAreas/OsTypeToggle";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles(theme => ({
    alert: {
        borderTop: `2px solid ${theme.palette.common.black}`,
        borderBottom: `2px solid ${theme.palette.common.black}`,
        "& .MuiGrid-grid-xs-3": {
            padding: "0px",
        },
    },
    alertTitle: {
        display: "flex",
        color: "white",
        justifyContent: "center",
        textAlign: "left",
    },
    paperColor: {
        backgroundColor: theme.palette.grey[300],
    },

    buttonHover: {
        "&:hover": {
            backgroundColor: theme.palette.error.main,
            color: theme.palette.warning.light,
        },
    },
}));

export const IntentSettings = () => {
    const { repository } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

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

    const cls = useStyles();
    const history = useHistory();

    const loadSettings = useCallback(async () => {
        const areas = await repository.Area.GetAreas();
        const areaData = areas.filter(x => x.areaIdentifier === areaIdentifier)[0];

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

    const handleIntentNameChange = async (newAreaName: string) => {
        if (newAreaName === settings.areaName) return;
        const updatedAreaName = await repository.Area.updateAreaName(areaIdentifier, newAreaName);
        const updatedSettings = { ...settings, areaName: updatedAreaName };
        setSettings(updatedSettings);
        window.location.reload(); // reloads the sidebar...
    };

    const handleIntentDisplayTitleChange = async (newAreaDisplayTitle: any) => {
        if (newAreaDisplayTitle === settings.areaTitle) return;
        const updatedDisplayTitle = await repository.Area.updateDisplayTitle(areaIdentifier, newAreaDisplayTitle);
        window.location.reload();
        const updatedSettings = { ...settings, areaTitle: updatedDisplayTitle };
        setSettings(updatedSettings);
    };

    const handleIntentDelete = async () => {
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

    const onIntentEnabledToggleChange = async () => {
        const updatedisEnabled = await repository.Area.UpdateIsEnabled(!isEnabledState, areaIdentifier);
        setIsEnabledState(updatedisEnabled);
    };

    const theme = useTheme();

    const [dialogOpen, setDialogOpen] = useState<boolean>(false);

    // I'll refactor this later. . .
    return loaded ? (
        <>
            <HeaderStrip title="Intent Settings" subtitle={`Modify settings that are specific to this intent (${settings.areaName}).`} />
            {isEnabledState !== null && <OsTypeToggle controlledState={isEnabledState} onChange={onIntentEnabledToggleChange} enabledLabel="Intent Enabled" disabledLabel="Intent Disabled" />}

            <Grid container spacing={3} justify="center">
                <SettingsBanner title="Widget Settings" subtitle="These options affect the appearance and behavior of the widget." />
                <Grid item xs={5}>
                    <SettingsGridRowText
                        classNames={cls.paperColor}
                        fullWidth
                        alertNode={
                            <Alert className={cls.alert} severity={settings.areaTitle === "Change this in the area Settings." || settings.areaTitle === "" ? "error" : "success"}>
                                <AlertTitle>
                                    <Typography variant="h5">Update Widget Display Name</Typography>
                                </AlertTitle>
                                Set the name of this area as used in the widget.
                            </Alert>
                        }
                        placeholder="New Area Name (Widget)"
                        currentValue={settings.areaTitle}
                        onClick={handleIntentDisplayTitleChange}
                        clearVal={false}
                    />
                </Grid>

                <Grid item xs={5}>
                    <SettingsGridRowText
                        classNames={cls.paperColor}
                        fullWidth
                        inputType="email"
                        alertNode={
                            <Alert className={cls.alert} severity={emailSeverity()}>
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
                    <SettingsBanner title="Dashboard Options" subtitle="These options only affect what you see in the dashboard" />
                </Grid>

                <Grid item xs={5}>
                    <SettingsGridRowText
                        classNames={cls.paperColor}
                        fullWidth
                        alertNode={
                            <Alert className={cls.alert} severity={settings.areaName ? "success" : "warning"}>
                                <AlertTitle>
                                    <Typography variant="h5">Update Dashboard Display Name</Typography>
                                </AlertTitle>
                                Set the name of area used for your reference on this dashboard.
                            </Alert>
                        }
                        placeholder="New Area Name (Dashboard)"
                        currentValue={settings.areaName}
                        onClick={handleIntentNameChange}
                        clearVal={false}
                    />
                </Grid>
            </Grid>
            <br></br>
            <Grid container spacing={3} justify="center">
                <Grid item xs={12}>
                    <SettingsBanner bgColor={theme.palette.error.main} title="DANGER ZONE" subtitle="CAUTION! These options cause permanent, irreversable changes." />
                </Grid>
                <Grid item xs={5}>
                    <SettingsGridRowText
                        classNames={cls.paperColor}
                        successText="Area Deleted"
                        alertNode={
                            <Alert className={cls.alert} severity="error">
                                <AlertTitle>
                                    <Typography variant="h5">Permanently DELETE</Typography>
                                </AlertTitle>
                                CAREFUL! Use this option to delete this area (and all associated data) forever.
                            </Alert>
                        }
                        onClick={async () => await Promise.resolve(setDialogOpen(true))}
                        clearVal={false}
                        buttonText="Permanently Delete"
                    />
                </Grid>
            </Grid>
            {alertState && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertDetails} />}
            <Dialog PaperProps={{ style: { margin: "2rem", padding: "2rem" } }} style={{ margin: "2rem", padding: "2rem" }} open={dialogOpen} onClose={() => setDialogOpen(false)}>
                <Typography variant="h4">Are you sure you want to delete this intent??</Typography>
                <Button className={cls.buttonHover} onClick={handleIntentDelete}>
                    PERMANENTLY DELETE
                </Button>
            </Dialog>
        </>
    ) : null;
};

export const SettingsBanner = ({ title, subtitle, bgColor }: { bgColor?: string; title: string; subtitle: string }) => {
    const cls = useStyles();
    const theme = useTheme();
    return (
        <Grid item xs={12} className={classNames(cls.alert, cls.alertTitle)} style={{ paddingTop: "3rem", paddingBottom: "3rem", marginTop: "2rem", background: bgColor ?? theme.palette.primary.main }}>
            <div style={{ width: "100%", display: "flex", flexDirection: "column", textAlign: "center" }}>
                <Typography display="inline" variant="h5">
                    {title}
                </Typography>
                <Typography display="inline">{subtitle}</Typography>
            </div>
        </Grid>
    );
};
