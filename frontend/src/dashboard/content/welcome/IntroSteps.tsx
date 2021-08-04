import { Button, makeStyles, Typography, useTheme } from "@material-ui/core";
import React, { useState } from "react";
import Tour, { ReactourStep } from "reactour";
import Fade from "react-reveal/Fade";
import HighlightOffIcon from "@material-ui/icons/HighlightOff";

export interface IIntroSteps {}

const useStyles = makeStyles((theme) => ({
    tour: {
        backgroundColor: theme.palette.common.white,
    },
}));

export const IntroSteps = () => {
    const [stepsEnabled, setStepsEnabled] = useState<boolean>(true);
    const [currentStep, setCurrentStep] = useState<number>(1);
    const cls = useStyles();
    const theme = useTheme();

    const steps: ReactourStep[] = [
        {
            content: "Welcome to Palavyr.com! I hope you are as excited as we are to build a brand-spanking-new chatbot!",
        },
        {
            content: <Typography>This guided tour will show you around to get your oriented.</Typography>,
        },
        {
            selector: ".widget-state-switch",
            content: (
                <Typography>
                    "Firstly, this toggle indicates the status of your live chatbot (not the demo!). A disabled widget won't show any area options. Click to enable when you're ready to show your widget to the
                    world!"
                </Typography>
            ),
        },
        {
            selector: ".quick-start-guide",
            content: "This is the first stop yo",
        },
    ];
    //https://github.com/elrumordelaluz/reactour
    return (
        <Fade>
            <Tour
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
                <Typography variant="h6" gutterBottom>
                    The Palavyr Guided Tour
                </Typography>
            </Tour>
        </Fade>
    );
};
