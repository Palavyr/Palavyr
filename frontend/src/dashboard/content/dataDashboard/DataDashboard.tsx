import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, Grid, Card } from "@material-ui/core";
import React from "react";
import { EnquiryRadarPlot } from "./EnquiryRadarPlot";

const useStyles = makeStyles((theme) => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
        padding: "2rem",
    },
}));

export const DataDashboard = () => {
    const cls = useStyles();

    return (
        <>
            <AreaConfigurationHeader title="Widget Activity Dashboard" subtitle="Review the activity of your chatbot" />
            <Grid container>
                <Grid item xs={3}>
                    <Card className={cls.plotCard} style={{ width: "450px", height: "450px" }}>
                        <EnquiryRadarPlot />
                    </Card>
                </Grid>
                <Grid item xs={9}>

                </Grid>
            </Grid>
        </>
    );
};
