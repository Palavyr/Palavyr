import * as React from 'react';
import { Paper, Grid, Button } from '@material-ui/core';
import { useState } from 'react';
import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';
import IconButton from '@material-ui/core/IconButton';
import InputLabel from '@material-ui/core/InputLabel';
import InputAdornment from '@material-ui/core/InputAdornment';
import FormControl from '@material-ui/core/FormControl';
import Visibility from '@material-ui/icons/Visibility';
import VisibilityOff from '@material-ui/icons/VisibilityOff';
import Input from '@material-ui/core/Input';
import { Statement } from './Statement';


export interface ISensitiveGridRowText {
    title: string;
    details?: string;
    children?: React.ReactNode;
    onClick: any;
}


const useStyles = makeStyles((theme => ({
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
        width: '35ch',
    },
    row: {
        padding: "1rem",
        margin: "1rem"
    },
    paper: {
        padding: "2rem",
        margin: "2rem",
    }
})))

export const SensitiveGridRowText = ({ title, details, children, onClick }: ISensitiveGridRowText) => {

    const [showOldPassword, setShowOldPassword] = useState<boolean>(false);
    const [oldPassword, setOldPassword] = useState<string>();

    const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
    const [newPassword, setNewPassword] = useState<string>();

    const [showNewPasswordCopy, setShowNewPasswordCopy] = useState<boolean>(false);
    const [newPasswordCopy, setNewPasswordCopy] = useState<string>();

    const classes = useStyles();

    return (
        <Paper className={classes.paper}>
            <Statement title={title} details={details} >
                {children}
            </Statement>
            <Grid className={classes.row} container spacing={2}>
                <FormControl className={clsx(classes.margin, classes.textField)}>
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
                                    {showOldPassword ? <Visibility /> : <VisibilityOff />}
                                </IconButton>
                            </InputAdornment>
                        }
                    />
                </FormControl>
                <br></br>
                <FormControl className={clsx(classes.margin, classes.textField)}>
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
                                    {showNewPassword ? <Visibility /> : <VisibilityOff />}
                                </IconButton>
                            </InputAdornment>
                        }
                    />
                </FormControl>
                <br></br>
                <FormControl className={clsx(classes.margin, classes.textField)}>
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
                                    {showNewPasswordCopy ? <Visibility /> : <VisibilityOff />}
                                </IconButton>
                            </InputAdornment>
                        }
                    />
                </FormControl>
                <br></br>
                <Grid item xs={4}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={async () => {

                            if (!(newPassword === newPasswordCopy)) {
                                alert("Passwords don't match")
                            } else {
                                var res = await onClick(oldPassword, newPassword)
                                if (res) {
                                    setNewPassword(undefined)
                                    setOldPassword(undefined)
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
    )
}