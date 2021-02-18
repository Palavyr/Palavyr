import { makeStyles, Theme, createStyles, Grid, FormControl, InputLabel, Input, InputAdornment, Button, IconButton, Paper, Divider } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";
import React, { useState } from "react";
import { Statement } from "@common/components/Statement";
import VisibilityIcon from "@material-ui/icons/Visibility";
import VisibilityOffIcon from "@material-ui/icons/VisibilityOff";
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";
import { Alert, AlertTitle } from "@material-ui/lab";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";

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
        textField: {
            // width: '50ch',
        },
        paper: {
            backgroundColor: "#C7ECEE",
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
    var client = new ApiClient();

    const [showOldPassword, setShowOldPassword] = useState<boolean>(false);
    const [oldPassword, setOldPassword] = useState<string>("");

    const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
    const [newPassword, setNewPassword] = useState<string>("");

    const [showNewPasswordCopy, setShowNewPasswordCopy] = useState<boolean>(false);
    const [newPasswordCopy, setNewPasswordCopy] = useState<string>("");

    const classes = useStyles();

    const handlePasswordChange = async (oldPassword: string, newPassword: string): Promise<boolean> => {
        var { data: success } = await client.Settings.Account.UpdatePassword(oldPassword, newPassword);
        return success;
    };

    return (
        <div style={{ width: "50%" }}>
            <AreaConfigurationHeader title="Change your password" subtitle="Update the password you use to log in." />
            <Divider />
            <Paper className={classes.paper}>
                <Alert>
                    <AlertTitle className={classes.titleText}>Update your password</AlertTitle>
                    Choose a strong password that contains at least:
                    <ul>
                        <li>6 or more characters</li>
                        <li>1 non-letter</li>
                        <li>1 uppercase letter</li>
                    </ul>
                </Alert>
                <Grid container spacing={3}>
                    <Grid className={classes.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password-old">Old Password</InputLabel>
                            <Input
                                className={classes.input}
                                id="standard-adornment-password-old"
                                type={showOldPassword ? "text" : "password"}
                                value={oldPassword}
                                onChange={(e) => setOldPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowOldPassword(!showOldPassword)} onMouseDown={(e) => e.preventDefault()}>
                                            {showOldPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                    </Grid>
                    <Grid className={classes.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password-new">New Password</InputLabel>
                            <Input
                                id="standard-adornment-password-new"
                                type={showNewPassword ? "text" : "password"}
                                value={newPassword}
                                onChange={(e) => setNewPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowNewPassword(!showNewPassword)} onMouseDown={(e) => e.preventDefault()}>
                                            {showNewPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                    </Grid>
                    <Grid className={classes.rowStyle} item xs={12}>
                        <FormControl fullWidth className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password-confirm">Confirm New Password</InputLabel>
                            <Input
                                id="standard-adornment-password-confirm"
                                type={showNewPasswordCopy ? "text" : "password"}
                                value={newPasswordCopy}
                                onChange={(e) => setNewPasswordCopy(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton aria-label="toggle password visibility" onClick={() => setShowNewPasswordCopy(!showNewPassword)} onMouseDown={(e) => e.preventDefault()}>
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
                            useModal
                            size="large"
                            customSaveMessage={{
                                title: "",
                                message: "Password successfully updated.",
                            }}
                        />
                    </div>
                </Grid>
            </Paper>
        </div>
    );
};
