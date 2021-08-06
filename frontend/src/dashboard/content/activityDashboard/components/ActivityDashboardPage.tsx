import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, Grid } from "@material-ui/core";
import React from "react";
import { DailyEnquiriesWeekly } from "../DailyEnquiriesWeekly";
import { EnquiryActivity } from "../EnquiryActivity";

const useStyles = makeStyles((theme) => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
        padding: "2rem",
    },
}));

export const ActivityDashboardPage = () => {
    const cls = useStyles();

    return (
        <>
            <AreaConfigurationHeader title="Widget Activity Dashboard" subtitle="Review the activity of your chatbot" />
            <Grid container>
                <Grid item xs={3}></Grid>
                <Grid item justify="center" xs={6}>
                    <EnquiryActivity />
                </Grid>
                <Grid item xs={3}></Grid>

                <Grid item xs={2}></Grid>

                <Grid item justify="center" xs={8}>
                    <DailyEnquiriesWeekly />
                </Grid>
                <Grid item xs={2}></Grid>
            </Grid>
        </>
    );
};
