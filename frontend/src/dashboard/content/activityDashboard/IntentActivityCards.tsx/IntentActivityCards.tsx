import { makeStyles } from "@material-ui/core";
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
    const [meta, setMeta] = useState<Meta[]>([]);
    const cls = useStyles();
    const history = useHistory();

    const loadEnquiries = useCallback(async () => {
        const areas = await repository.Area.GetAreas();
        const enquiries = await repository.Enquiries.getEnquiries();

        const me: Meta[] = [];
        areas.forEach(area => {
            const filteredForSingleArea = enquiries.filter(area2 => area2.areaName === area.areaDisplayTitle);

            me.push({
                areaName: area.areaName,
                count: filteredForSingleArea.length,
                areaId: area.areaIdentifier,
                completed: 24,
                emails: 0,
            });
        });
        setMeta(me);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <div className={cls.container}>
            {meta.map(m => {
                return <IntentActivityCard areaName={m.areaName} count={m.count} completed={m.completed} emails={m.emails} onClick={() => history.push(createNavLink(m.areaId))} />;
            })}
        </div>
    );
};
