import { Grid, makeStyles } from "@material-ui/core";
import { EnquiryActivtyResource } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { createNavLink } from "dashboard/layouts/sidebar/sections/sectionComponents/AreaLinkItem";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { IntentActivityCard } from "./IntentActivityCard";

const useStyles = makeStyles(theme => ({
    container: {
        display: "flex",
        flexDirection: "row",
    },
}));

type Meta = {
    areaName: string;
    count: number;
    areaId: string;
    completed: number;
    emails: number;
};

export const IntentActivityCards = () => {
    const { repository } = useContext(DashboardContext);
    const [activity, setActivity] = useState<EnquiryActivtyResource[]>([]);
    const cls = useStyles();
    const history = useHistory();

    const loadEnquiries = useCallback(async () => {
        const enquiryActivy = await repository.Enquiries.getEnquiryInsights();
        setActivity(enquiryActivy);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <Grid container>
            {activity.map((a: EnquiryActivtyResource, index: number) => {
                return <Grid item>{activity && activity[0] && <IntentActivityCard key={index} activityResource={activity[0]} onClick={() => history.push(createNavLink(a.intentIdentifier))} />}</Grid>;
            })}
        </Grid>
    );
};
