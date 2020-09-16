import { makeStyles, Theme, createStyles, Grid, FormControl, InputLabel, Input, InputAdornment, Button, IconButton, Paper } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";
import React, { useState } from "react";
import { Statement } from "@common/components/Statement";
import VisibilityIcon from '@material-ui/icons/Visibility';
import VisibilityOffIcon from '@material-ui/icons/VisibilityOff';
import { CustomAlert } from "@common/components/customAlert/CutomAlert";
import classNames from "classnames";


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: 'flex',
            flexWrap: 'wrap',
        },
        margin: {
            margin: theme.spacing(1),
        },
        withoutLabel: {
            marginTop: theme.spacing(3),
        },
        textField: {
            width: '25ch',
        },
    }),
);


const rowStyle = {
    padding: "1rem",
    margin: "1rem"
}

const paperStyle = {
    padding: "2rem",
    margin: "2rem",
    width: "100%"
}

export const ChangePassword = () => {
    var client = new ApiClient();

    // const [, setLoaded] = useState<boolean>(false);
    // const [, setPassword] = useState<string>("");

    const [alertState, setAlert] = useState<boolean>(false);

    // useEffect(() => {
    //     setLoaded(true);
    //     return () => {
    //         setLoaded(false)
    //     }
    // }, [setPassword])

    const [showOldPassword, setShowOldPassword] = useState<boolean>(false);
    const [oldPassword, setOldPassword] = useState<string>("");

    const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
    const [newPassword, setNewPassword] = useState<string>("");

    const [showNewPasswordCopy, setShowNewPasswordCopy] = useState<boolean>(false);
    const [newPasswordCopy, setNewPasswordCopy] = useState<string>("");

    const classes = useStyles();

    const handlePasswordChange = async (oldPassword: string, newPassword: string) => {
        var success = await client.Settings.Account.UpdatePassword(oldPassword, newPassword);
        return success;
    }

    const customAlert = {
        title: "",
        message: "Password successfully updated."
    }
    const name = "Update Password";
    const details = "Update Your Password";

    return (
        <>
            <Grid container spacing={3}>
                <Paper style={paperStyle}>
                    <Statement title={name} details={details} />
                    <Grid style={rowStyle} container spacing={2}>
                        <FormControl className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password">Old Password</InputLabel>
                            <Input
                                id="standard-adornment-password"
                                type={showOldPassword ? 'text' : 'password'}
                                value={oldPassword}
                                onChange={(e) => setOldPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton
                                            aria-label="toggle password visibility"
                                            onClick={() => setShowOldPassword(!showOldPassword)}
                                            onMouseDown={(e) => e.preventDefault()}
                                        >
                                            {showOldPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                        <br></br>
                        <FormControl className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password">New Password</InputLabel>
                            <Input
                                id="standard-adornment-password"
                                type={showNewPassword ? 'text' : 'password'}
                                value={newPassword}
                                onChange={(e) => setNewPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton
                                            aria-label="toggle password visibility"
                                            onClick={() => setShowNewPassword(!showNewPassword)}
                                            onMouseDown={(e) => e.preventDefault()}
                                        >
                                            {showNewPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>
                        <br></br>
                        <FormControl className={classNames(classes.margin, classes.textField)}>
                            <InputLabel htmlFor="standard-adornment-password">Confirm New Password</InputLabel>
                            <Input
                                id="standard-adornment-password"
                                type={showNewPasswordCopy ? 'text' : 'password'}
                                value={newPasswordCopy}
                                onChange={(e) => setNewPasswordCopy(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButton
                                            aria-label="toggle password visibility"
                                            onClick={() => setShowNewPasswordCopy(!showNewPassword)}
                                            onMouseDown={(e) => e.preventDefault()}
                                        >
                                            {showNewPasswordCopy ? <VisibilityIcon /> : <VisibilityOffIcon />}
                                        </IconButton>
                                    </InputAdornment>
                                }
                            />
                        </FormControl>

                        <Grid item xs={4}>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={async () => {

                                    if (!(newPassword === newPasswordCopy)) {
                                        alert("Passwords don't match")
                                    } else {
                                        var res = await handlePasswordChange(oldPassword, newPassword)
                                        if (res.data === true) {
                                            setNewPassword("")
                                            setOldPassword("")
                                            setNewPasswordCopy("")
                                            setAlert(true);
                                        } else {
                                            alert("Password does not match that on record.")
                                        }
                                    }
                                }}
                            >
                                Change Password
                    </Button>
                        </Grid>

                    </Grid>
                </Paper>
            </Grid>
            {alertState && <CustomAlert alertState={alertState} setAlert={setAlert} alert={customAlert} />}
        </>
    )
}