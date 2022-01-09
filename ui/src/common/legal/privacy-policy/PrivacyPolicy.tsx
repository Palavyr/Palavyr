import { LandingWrapper } from "@landing/components/LandingWrapper";
import { TitleContent } from "@landing/components/TitleContent";
import { makeStyles, Paper, Typography } from "@material-ui/core";
import { Align } from "@common/positioning/Align";
import React from "react";
import { PrivacyPolicyContent } from "./PrivacyPolicyContent";

const useStyles = makeStyles(theme => ({
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
        width: "70%",
    },
}));

export const PrivacyPolicyPage = () => {
    const cls = useStyles();

    return (
        <LandingWrapper
        // TitleContent={
        //     <TitleContent
        //         title={
        //             <Typography align="center" variant="h2" className={cls.primaryText}>
        //                 Privacy Policy
        //             </Typography>
        //         }
        //         subtitle={
        //             <Typography align="center" variant="h6" className={cls.secondaryText}>
        //                 We know this can a little boring to read, but its important. Thanks for understanding :)
        //             </Typography>
        //         }
        //     />
        // }
        // MainContent={
        //     <Align>
        //         <Paper className={cls.content}>
        //             <PrivacyPolicyContent />
        //         </Paper>
        //     </Align>
        // }
        />
    );
};
