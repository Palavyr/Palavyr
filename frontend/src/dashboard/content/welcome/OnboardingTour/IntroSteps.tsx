import { makeStyles, Typography, useTheme } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import Tour, { ReactourStep } from "reactour";
import Fade from "react-reveal/Fade";
import HighlightOffIcon from "@material-ui/icons/HighlightOff";

export interface IIntroSteps {
    steps: ReactourStep[];
    initialize: boolean;
    onBlur(): void;
}

const useStyles = makeStyles((theme) => ({
    tour: {
        backgroundColor: theme.palette.common.white,
        padding: "2.2rem",
        borderRadius: "12px",
    },
}));

export const IntroSteps = ({ steps, onBlur, initialize = true }: IIntroSteps) => {
    const [stepsEnabled, setStepsEnabled] = useState<boolean>(false);
    const [currentStep, setCurrentStep] = useState<number>(1);
    const cls = useStyles();
    const theme = useTheme();

    useEffect(() => {
        setStepsEnabled(initialize);
    }, []);

    //https://github.com/elrumordelaluz/reactour
    return (
        <Fade>
            <Tour
                onBeforeClose={onBlur}
                getCurrentStep={(curr) => setCurrentStep(curr)}
                className={cls.tour}
                accentColor={theme.palette.primary.light}
                steps={steps}
                isOpen={stepsEnabled}
                onRequestClose={() => setStepsEnabled(!stepsEnabled)}
                badgeContent={(curr, tot) => `${curr} of ${tot}`}
                lastStepNextButton={<HighlightOffIcon />}
                maskSpace={5}
                startAt={0} // set to 1 after cookie is set and they reopen
            >
                {currentStep === 0 ? (
                    <Typography align="center" variant="h6" gutterBottom>
                        The Palavyr Guided Tour
                    </Typography>
                ) : (
                    <></>
                )}
            </Tour>
        </Fade>
    );
};
