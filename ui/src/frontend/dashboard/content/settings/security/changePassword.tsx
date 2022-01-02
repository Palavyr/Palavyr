import { makeStyles, Theme, createStyles, Grid, FormControl, InputLabel, Input, InputAdornment, IconButton, Paper, Divider } from "@material-ui/core";
import React, { useContext, useState } from "react";
import VisibilityIcon from "@material-ui/icons/Visibility";
import VisibilityOffIcon from "@material-ui/icons/VisibilityOff";
import classNames from "classnames";
import { Alert, AlertTitle } from "@material-ui/lab";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { SettingsWrapper } from "../SettingsWrapper";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "center",
        },
        margin: {
            margin: theme.spacing(1),
        },
        withoutLabel: {
            marginTop: theme.spacing(3),
        },
        paper: {
            backgroundColor: "rgb(0, 0, 0 ,0)", //theme.palette.secondary.light,
            border: "0px",
            boxShadow: "none",
            padding: "2rem",
            margin: "2rem",
        },
        rowStyle: {
            padding: "1rem",
            margin: "1rem",
        },
        titleText: {
            fontWeight: "bold",
        },
        input: {
            width: "100%",
        },
    })
);

export const ChangePassword = () => {
    const { repository } = useContext(DashboardContext);

    const [showOldPassword, setShowOldPassword] = useState<boolean>(false);
    const [oldPassword, setOldPassword] = useState<string>("");

    const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
    const [newPassword, setNewPassword] = useState<string>("");

    const [showNewPasswordCopy, setShowNewPasswordCopy] = useState<boolean>(false);
    const [newPasswordCopy, setNewPasswordCopy] = useState<string>("");

    const cls = useStyles();

    const handlePasswordChange = async (oldPassword: string, newPassword: string): Promise<boolean> => {
        const success = await repository.Settings.Account.UpdatePassword(oldPassword, newPassword);
        return success;
    };

    return (
        <SettingsWrapper>
            <HeaderStrip title="Change your password" subtitle="Update the password you use to log in." />
            <Divider />
            <Paper className={cls.paper}>
                <Alert>
                    <AlertTitle className={cls.titleText}>Update your password</AlertTitle>
                    Choose a strong password that contains at least:
                    <ul>
                        <li>6 or more characters</li>
                        <li>1 non-letter</li>
                        <li>1 uppercase letter</li>
                    </ul>
                </Alert>
                <Grid container spacing={3}>
                    <Grid className={cls.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(cls.margin)}>
                            <InputLabel htmlFor="standard-adornment-password-old">Old Password</InputLabel>
                            <Input
                                className={cls.input}
                                id="standard-adornment-password-old"
                                type={showOldPassword ? "text" : "password"}
                                value={oldPassword}
                                onChange={e => setOldPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowOldPassword(!showOldPassword)} onMouseDown={e => e.preventDefault()}>
                                            {showOldPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                    </Grid>
                    <Grid className={cls.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(cls.margin)}>
                            <InputLabel htmlFor="standard-adornment-password-new">New Password</InputLabel>
                            <Input
                                id="standard-adornment-password-new"
                                type={showNewPassword ? "text" : "password"}
                                value={newPassword}
                                onChange={e => setNewPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowNewPassword(!showNewPassword)} onMouseDown={e => e.preventDefault()}>
                                            {showNewPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                    </Grid>
                    <Grid className={cls.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(cls.margin)}>
                            <InputLabel htmlFor="standard-adornment-password-confirm">Confirm New Password</InputLabel>
                            <Input
                                id="standard-adornment-password-confirm"
                                type={showNewPasswordCopy ? "text" : "password"}
                                value={newPasswordCopy}
                                onChange={e => setNewPasswordCopy(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowNewPasswordCopy(!showNewPassword)} onMouseDown={e => e.preventDefault()}>
                                            {showNewPasswordCopy ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                    </Grid>
                    <div style={{ display: "flex", width: "100%", justifyContent: "flex-end" }}>
                        <SaveOrCancel
                            onSave={async () => {
                                if (!(newPassword === newPasswordCopy)) {
                                    alert("Passwords don't match");
                                } else {
                                    const result = await handlePasswordChange(oldPassword, newPassword);
                                    if (result === true) {
                                        setOldPassword("");
                                        setNewPassword("");
                                        setNewPasswordCopy("");
                                    } else {
                                        alert("Password does not match that on record.");
                                    }
                                }
                                return true;
                            }}
                            size="large"
                            customSaveMessage="Password successfully updated."
                        />
                    </div>
                </Grid>
            </Paper>
        </SettingsWrapper>
    );
};
