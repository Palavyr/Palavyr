import React from "react";
import { makeStyles, Theme, createStyles } from "@material-ui/core/styles";
import Stepper from "@material-ui/core/Stepper";
import Step from "@material-ui/core/Step";
import StepLabel from "@material-ui/core/StepLabel";
import { Typography } from "@material-ui/core";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            width: "100%",
        },
        stepper: {
            backgroundColor: theme.palette.secondary.light,
            color: theme.palette.common.white,
            borderBottom: "3px solid black",
            borderTop: "3px solid black",
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
    const cls = useStyles();
    return (
        <div className={cls.root}>
            <Stepper className={cls.stepper} activeStep={activeStep} alternativeLabel>
                {getSteps().map((label) => (
                    <Step key={label}>
                        <StepLabel>
                            <Typography variant="h5">{label}</Typography>
                        </StepLabel>
                    </Step>
                ))}
            </Stepper>
        </div>
    );
};
