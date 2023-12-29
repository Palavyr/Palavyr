import { Grid, makeStyles } from "@material-ui/core";
import { EnquiryInsightsResource } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { createNavLink } from "frontend/dashboard/layouts/sidebar/sections/sectionComponents/IntentLinkItem";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { IntentActivityCard } from "./IntentActivityCard";


const useStyles = makeStyles<{}>((theme: any) => ({
    container: {
        display: "flex",
        flexDirection: "row",
    },
}));

type Meta = {
    intentName: string;
    count: number;
    IntentId: string;
    completed: number;
    emails: number;
};

export const IntentActivityCards = () => {
    const { repository } = useContext(DashboardContext);
    const [activity, setActivity] = useState<EnquiryInsightsResource[]>([]);
    const cls = useStyles();
    const history = useHistory();

    const loadEnquiries = useCallback(async () => {
        const enquiryActivy = await repository.Enquiries.GetEnquiryInsights();
        setActivity(enquiryActivy);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <Grid container>
            {activity.map((a: EnquiryInsightsResource, index: number) => {
                return <Grid item>{a && <IntentActivityCard key={index} activityResource={a} onClick={() => history.push(createNavLink(a.intentIdentifier))} />}</Grid>;
            })}
        </Grid>
    );
};
