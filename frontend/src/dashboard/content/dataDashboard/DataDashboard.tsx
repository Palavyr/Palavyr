import { makeStyles, Grid, Card, Typography } from "@material-ui/core";
import React from "react";
import { EnquiryRadarPlot } from "./EnquiryRadarPlot";

const useStyles = makeStyles((theme) => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
    },
}));

export const DataDashboard = () => {
    const cls = useStyles();

    return (
        <div>
            <Grid>
                <Grid>
                    <Card className={cls.plotCard} style={{ width: "500px", height: "500px" }}>
                        <Typography align="center" gutterBottom>
                            Radar Chart - Enquiries by Area
                        </Typography>
                        <EnquiryRadarPlot />
                    </Card>
                </Grid>
            </Grid>
        </div>
    );
};
