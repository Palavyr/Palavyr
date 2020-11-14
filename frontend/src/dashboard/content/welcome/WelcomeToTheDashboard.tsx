import * as React from 'react';
import { makeStyles, Card, Typography, Divider } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import classNames from 'classnames';
import { useHistory } from 'react-router-dom';
import { DashboardContext } from 'dashboard/layouts/DashboardContext';

const useStyles = makeStyles(theme => ({
    background: {
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    wrapper: {
        position: "relative",
        height: "100%",
        textAlign: "center"
    },
    sectionDiv: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    alert: {
        border: "1px solid black",
        marginTop: "1rem",
        marginBottom: "1rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center"
    },
    card: {
        width: "50%",
        margin: "4rem",
        padding: "3rem",
        color: "white",
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",

    },
    alertTitle: {
        fontSize: "16pt",
        fontWeight: "bold"
    },
    stepTitle: {
        fontSize: "18pt",
        fontWeight: "bold"
    },
    highlight: {
        "&:hover": {
            background: "lightgray",
            color: "black"
        }
    },
    headCard: {
        display: "flex",
        justifyContent: "center"
    }

}))

export const WelcomeToTheDashboard = () => {

    const classes = useStyles();
    const history = useHistory();
    const { checkAreaCount } = React.useContext(DashboardContext);

    return (
        <div className={classes.wrapper}>
            <Card className={classes.background}>
                <div className={classes.headCard}>
                    <div style={{ borderBottom: "2px solid black", width: "50%" }}>
                        <Typography variant="h3" style={{ paddingTop: "2rem", paddingBottom: "2rem" }}>
                            Welcome to palavyr!
                    </Typography>
                    </div>
                </div>
                <Typography style={{ padding: "1rem", fontSize: "16pt" }}>
                    Palavyr is simple to configure, and even more simple to use.
                </Typography>
                <Typography style={{ padding: "2rem", fontSize: "16pt" }}>
                    Follow these three simple steps to get started:
                </Typography>
                <Divider />
                <div className={classes.sectionDiv}>
                    <Alert className={classes.alert} icon={<></>} severity="info">
                        <AlertTitle className={classes.alertTitle}>Step 1.</AlertTitle>
                    </Alert>
                    <Card className={classNames(classes.card, classes.highlight)} onClick={() => checkAreaCount()}>
                        <Typography className={classes.stepTitle}>
                            Create a new area
                        </Typography>
                    </Card>
                </div>
                <Divider />
                <div className={classes.sectionDiv}>
                    <Alert className={classes.alert} icon={<></>} severity="info">
                        <AlertTitle className={classes.alertTitle}>Step 2.</AlertTitle>
                    </Alert>
                    <Card className={classes.card}>
                        <Typography className={classes.stepTitle}>
                            Configure your new area
                        </Typography>
                    </Card>
                </div>
                <Divider />
                <div className={classes.sectionDiv}>
                    <Alert className={classes.alert} icon={<></>} severity="info">
                        <AlertTitle className={classes.alertTitle}>Step 3.</AlertTitle>
                    </Alert>
                    <Card className={classNames(classes.card, classes.highlight)} onClick={
                        () => history.push('/dashboard/getwidget/')
                    }
                    >
                        <Typography className={classes.stepTitle}>
                            Insert your newly configured widget into your website
                        </Typography>
                    </Card>
                </div>
            </Card>
        </div>
    )
}