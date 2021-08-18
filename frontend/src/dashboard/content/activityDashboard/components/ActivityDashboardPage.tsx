import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, Grid } from "@material-ui/core";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React from "react";
import { DailyEnquiriesWeekly } from "../DailyEnquiriesWeekly";
import { EnquiryActivity } from "../EnquiryActivity";

const useStyles = makeStyles(theme => ({
    squareGridItem: {
        // height: "400px",
        // width: "400px",
    },
    rectangleGridItem: {
        // height: "300px",
        // width: "600px",
    },
}));

export const ActivityDashboardPage = () => {
    const cls = useStyles();
    const { setViewName } = React.useContext(DashboardContext);
    setViewName("Data Dashboard");

    return (
        <>
            <AreaConfigurationHeader
                divider
                title="Widget Activity Dashboard"
                subtitle="Review the activity of your chatbot! This page is early release, but we've made a couple plots availble as a sneak peak for you!"
            />
            <Grid container>
                <Grid item xs={2}></Grid>
                <Grid item justify="center" xs={8} className={cls.squareGridItem}>
                    <EnquiryActivity />
                </Grid>
                <Grid item xs={2}></Grid>

                <Grid item xs={2}></Grid>
                <Grid item justify="center" xs={8} className={cls.rectangleGridItem}>
                    <DailyEnquiriesWeekly />
                </Grid>
                <Grid item xs={2}></Grid>
            </Grid>
        </>
    );
};
