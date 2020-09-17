import * as React from 'react';
import { makeStyles, Card, Typography } from '@material-ui/core';
// import { PleaseConfirmYourEmail } from './PleaseConfirmYourEmail';

const useStyles = makeStyles(theme => ({
    card: {
        width: "100%",
        margin: "3rem",
        padding: "2rem",

    },
    wrapper: {
        alignContent: "center",
        alignItems: "center"
    }
}))

export const WelcomeToTheDashboard = () => {

    const classes = useStyles();
    return (
        <div className={classes.wrapper}>
            <Card className={classes.card}>
                <Typography variant="h3">
                    Welcome to palavyr!
                </Typography>
                Here we will include a nice concise tutorial on how to use this webapp.
        </Card>
        </div>
    )
}