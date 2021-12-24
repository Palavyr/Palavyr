import { makeStyles, Typography, useTheme } from "@material-ui/core";
import React, { useContext, useEffect, useState } from "react";
import Tour, { ReactourStep } from "reactour";
import Fade from "react-reveal/Fade";
import HighlightOffIcon from "@material-ui/icons/HighlightOff";

import { disableBodyScroll, enableBodyScroll } from "body-scroll-lock";
import { AuthContext } from "frontend/dashboard/layouts/DashboardContext";

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
    const { isActive } = useContext(AuthContext);

    const [stepsEnabled, setStepsEnabled] = useState<boolean>(false);
    const [currentStep, setCurrentStep] = useState<number>(1);
    const cls = useStyles();
    const theme = useTheme();

    const disableBody = (target) => disableBodyScroll(target);
    const enableBody = (target) => enableBodyScroll(target);

    useEffect(() => {
        if (isActive) {
            setStepsEnabled(initialize);
        }
    }, []);

    //https://github.com/elrumordelaluz/reactour
    return (
        <Fade>
            <Tour
                onAfterOpen={disableBody}
                onBeforeClose={(target) => {
                    enableBody(target);
                    onBlur();
                }}
                getCurrentStep={(curr) => setCurrentStep(curr)}
                className={cls.tour}
                accentColor={theme.palette.primary.light}
                steps={steps}
                isOpen={stepsEnabled}
                onRequestClose={() => setStepsEnabled(!stepsEnabled)}
                badgeContent={(curr, tot) => `${curr} of ${tot}`}
                lastStepNextButton={<HighlightOffIcon />}
                maskSpace={0}
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
