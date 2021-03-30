import * as React from 'react';
import { makeStyles } from '@material-ui/core';
import { SessionStorage } from 'localStorage/sessionStorage';

const useStyles = makeStyles(theme => ({
    loggedin: {
        color: "navy"
    },
    logwrapper: {
        textAlign: "center",
        paddingTop: "1rem",
        paddingBottom: "0.7rem",
        // height: "72px",

    }
}))

export const UserDetails = () => {

    const email = SessionStorage.getEmailAddress();
    const googleImage = SessionStorage.getGoogleImage();
    const classes = useStyles();

    return (
        <div className={classes.logwrapper}>
            <span className={classes.loggedin}>Logged in as:</span>
            <br></br>
            <span className={classes.loggedin}>{email}</span>
            {
                (googleImage !== "" && googleImage !== undefined && googleImage !== null) &&
                <div style={{ display: "flex", width: '100%', justifyContent: "center" }}>
                    <img src={googleImage} alt="" style={{borderRadius: "50%", margin: "10px", height: "75px", width: "75px"}} />
                </div>
            }
        </div>
    )
}