import * as React from 'react';
import { LocalStorage } from 'localStorage/localStorage';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles(theme => ({
    loggedin: {
        color: "white"
    },
    logwrapper: {
        textAlign: "center",
        paddingTop: "1rem",
        paddingBottom: "0.7rem",
        // height: "200px"
        height: "72px"
    }
}))

export const UserDetails = () => {

    const email = LocalStorage.getEmailAddress();
    const classes = useStyles();

    return (
        <div className={classes.logwrapper}>
            <span className={classes.loggedin}>Logged in as:</span>
            <br></br>
            <span className={classes.loggedin}>{email}</span>
        </div>
    )
}