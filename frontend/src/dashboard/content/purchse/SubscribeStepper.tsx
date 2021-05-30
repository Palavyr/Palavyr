import React from "react";
import { makeStyles, Theme, createStyles } from "@material-ui/core/styles";
import Stepper from "@material-ui/core/Stepper";
import Step from "@material-ui/core/Step";
import StepLabel from "@material-ui/core/StepLabel";
import { Typography } from "@material-ui/core";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        text: {
            color: theme.palette.common.white
        },
        root: {
            width: "100%",
        },
        stepper: {
            backgroundColor: theme.palette.secondary.main,
            color: theme.palette.common.white,
            borderBottom: "3px solid black",
            borderTop: "3px solid black",
        },
        sub: {
            "&:hover": {
                cursor: "pointer",
            },
        },
    })
);

const steps = ["Select a subcription plan", "Select billing frequency", "Proceed to Stripe Checkout"];

interface ISubscribeStepper {
    activeStep: 0 | 1 | 2;
}

export const SubscribeStepper = ({ activeStep }: ISubscribeStepper) => {
    const cls = useStyles();
    const history = useHistory();

    return (
        <div className={cls.root}>
            <Stepper className={cls.stepper} activeStep={activeStep} alternativeLabel>
                {steps.map((label: string, index: number) => (
                    <Step className={index === 0 ? cls.sub : ""} onClick={index === 0 ? () => history.push("/dashboard/subscribe") : () => null} key={label}>
                        <StepLabel>
                            <Typography className={cls.text} variant="h5">{label}</Typography>
                        </StepLabel>
                    </Step>
                ))}
            </Stepper>
        </div>
    );
};
