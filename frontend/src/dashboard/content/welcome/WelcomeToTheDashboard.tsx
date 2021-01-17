import * as React from "react";
import { makeStyles, Card, Typography, Divider, Link } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import classNames from "classnames";
import { useHistory } from "react-router-dom";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { OnboardingTodo } from "./OnboardingTodo";

const useStyles = makeStyles((theme) => ({
    background: {
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    wrapper: {
        position: "relative",
        height: "100%",
        textAlign: "center",
    },
    sectionDiv: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    sectionHeadDiv: {
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    listItems:{
        widgth: "40%",
        margin: "0 auto",
        padding: "1rem",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
    },
    alert: {
        border: "1px solid black",
        marginTop: "1rem",
        marginBottom: "1rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
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
        fontWeight: "bold",
    },
    stepTitle: {
        fontSize: "18pt",
        fontWeight: "bold",
    },
    highlight: {
        "&:hover": {
            background: "lightgray",
            color: "black",
        },
    },
    headCard: {
        display: "flex",
        justifyContent: "center",
    },
    clickable: {
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export const WelcomeToTheDashboard = () => {
    const cls = useStyles();
    const history = useHistory();
    const { checkAreaCount } = React.useContext(DashboardContext);
    const preventDefault = (event: React.SyntheticEvent) => event.preventDefault();

    return (
        <div className={cls.wrapper}>
            <Card className={cls.background}>
                <div className={cls.headCard}>
                    <div style={{ borderBottom: "2px solid black", width: "50%" }}>
                        <Typography variant="h3" style={{ paddingTop: "2rem", paddingBottom: "2rem" }}>
                            Welcome to palavyr!
                        </Typography>
                    </div>
                </div>
                <Typography style={{ padding: "1rem", paddingTop: "2rem", fontSize: "16pt" }}>How does it work</Typography>
                <Typography style={{ padding: "1rem" }}>Palavyr is simple to configure, and even more simple to use.</Typography>
                <div className={cls.sectionHeadDiv} >
                    <div className={cls.listItems}>

                    <Typography gutterBottom align="left">
                        1. You configure the Palavyr widget by you in this dashboard.
                    </Typography>
                    <Typography gutterBottom align="left">
                        2. You provide the palavyr widget in your business website.
                    </Typography>
                    <Typography gutterBottom align="left">3. Your customers use the widget to receive infrmation and service fee estimates via email.</Typography>
                    </div>
                </div>
                <Typography style={{ padding: "2rem", fontSize: "16pt" }}>Follow these steps to get started:</Typography>
                <Divider />
                <div className={cls.sectionDiv}>
                    <Alert className={cls.alert} icon={<></>} severity="info">
                        <AlertTitle className={cls.alertTitle}>Step 1.</AlertTitle>
                    </Alert>
                    <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => checkAreaCount()}>
                        <Typography className={cls.stepTitle}>Getting started</Typography>
                        <Typography gutterBottom>There are a couple things to do to get started.</Typography>
                        <Typography align="left" gutterBottom>
                            1. Check your email again for another verification email from Amazon.com. We use amazon to send emails on your behalf and they need you to confirm your address with them.
                        </Typography>
                        <Typography align="left" gutterBottom>
                            2. Navigate to your general settigs and set your Company/Buiness name and default contact information.
                        </Typography>
                        {/* <OnboardingTodo /> */}
                    </Card>
                </div>
                <Divider />
                <div className={cls.sectionDiv}>
                    <Alert className={cls.alert} icon={<></>} severity="info">
                        <AlertTitle className={cls.alertTitle}>Step 2.</AlertTitle>
                    </Alert>
                    <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => checkAreaCount()}>
                        <Typography gutterBottom className={cls.stepTitle}>
                            Create your first area
                        </Typography>
                        <Typography align="center" gutterBottom>
                            We provide a preconfigured example area for you to review. This shoudl give you an idea of how to use Palaver. Once you've finished, you will want to delete this area and create your own.
                        </Typography>

                        <Typography>Click here to create a new area.</Typography>
                    </Card>
                </div>
                <Divider />
                <div className={cls.sectionDiv}>
                    <Alert className={cls.alert} icon={<></>} severity="info">
                        <AlertTitle className={cls.alertTitle}>Step 3.</AlertTitle>
                    </Alert>
                    <Card className={cls.card}>
                        <Typography className={cls.stepTitle}>Configure your new area</Typography>
                        <Typography align="left" gutterBottom>
                            Configuring an area consists of providing an email template, configuring your fees, information, and attachments, and finally configuring a conversation.
                        </Typography>
                        <Typography align="left">In your area, follow the tabs in the order they are provided (from left to right). At the end, you can preview your response PDF.</Typography>
                    </Card>
                </div>
                <Divider />
                <div className={cls.sectionDiv}>
                    <Alert className={cls.alert} icon={<></>} severity="info">
                        <AlertTitle className={cls.alertTitle}>Step 4.</AlertTitle>
                    </Alert>
                    <Card className={classNames(cls.card, cls.highlight, cls.clickable)} onClick={() => history.push("/dashboard/getwidget/")}>
                        <Typography className={cls.stepTitle}>Insert your newly configured widget into your website</Typography>
                        <Typography>When you are ready to use your configured widget, simple paste the provided code into your website's html.</Typography>
                    </Card>
                </div>
            </Card>
        </div>
    );
};
