import { LandingWrapper } from "@landing/components/LandingWrapper";
import { TitleContent } from "@landing/components/TitleContent";
import { makeStyles, Paper } from "@material-ui/core";
import { Typography } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";
import React from "react";
import { TermsOfUseContent } from "./TermsOfUseContent";

const useStyles = makeStyles((theme) => ({
    primaryText: {
        color: theme.palette.success.main,
    },
    secondaryText: {
        color: theme.palette.success.dark,
    },
    content: {
        margin: "2rem",
        padding: "2rem",
        height: "100%",
        width: "100%",
    },
}));

export const TermsOfUsePage = () => {
    const cls = useStyles();
    return (
        <LandingWrapper
            TitleContent={
                <TitleContent
                    title={
                        <Typography align="center" variant="h2" className={cls.primaryText}>
                            Terms of Use Agreement
                        </Typography>
                    }
                    subtitle={
                        <Typography align="center" variant="h6" className={cls.secondaryText}>
                            We know this can a little boring to read, but its important. Thanks for understanding :)
                        </Typography>
                    }
                />
            }
            MainContent={
                <Align>
                    <Paper className={cls.content}>
                        <TermsOfUseContent />
                    </Paper>
                </Align>
            }
        />
    );
};
