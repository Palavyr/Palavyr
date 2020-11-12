import React from "react";
import { makeStyles, Theme, createStyles } from "@material-ui/core/styles";
import Stepper from "@material-ui/core/Stepper";
import Step from "@material-ui/core/Step";
import StepLabel from "@material-ui/core/StepLabel";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            width: "100%",
        },
        stepper: {
            backgroundColor: "#C7ECEE",
            borderBottom: "3px solid black"
        },
        backButton: {
            marginRight: theme.spacing(1),
        },
        instructions: {
            marginTop: theme.spacing(1),
            marginBottom: theme.spacing(1),
        },
    })
);

function getSteps() {
    return ["Select a subcription plan", "Select billing frequency", "Proceed to Stripe Checkout"];
}

interface ISubscribeStepper {
    activeStep: 0 | 1 | 2;
}

export const SubscribeStepper = ({ activeStep }: ISubscribeStepper) => {
    const classes = useStyles();
    const steps = getSteps();
    return (
        <div className={classes.root}>
            <Stepper className={classes.stepper} activeStep={activeStep} alternativeLabel>
                {steps.map((label) => (
                    <Step key={label}>
                        <StepLabel>{label}</StepLabel>
                    </Step>
                ))}
            </Stepper>
        </div>
    );
};
