import { makeStyles } from "@material-ui/core";
import { ClassNameMap } from "@material-ui/core/styles/withStyles";
import { Enquiries } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { ActivityCard } from "./ActivityCard";

// export const stylesHook = (styles: ClassNameMap<any>) => {
//     return makeStyles(theme => styles);
// };

// const styles: ClassNameMap<any> = {
//     container: {
//         display: "flex",
//         flexDirection: "row"
//     },
// };

const useStyles = makeStyles(theme => ({
    container: {
        display: "flex",
        flexDirection: "row",

    },
}));

export const ActivityCards = () => {
    const { repository } = useContext(DashboardContext);
    const [data, setData] = useState<Enquiries>();
    const [counts, setCounts] = useState<Object>({});
    const cls = useStyles();

    const loadEnquiries = useCallback(async () => {
        const areas = await repository.Area.GetAreas();
        const enquiries = await repository.Enquiries.getEnquiries();
        const enquiryAreas = enquiries.map(x => x.areaName);

        const counts = {};
        areas
            .map(x => x.areaName)
            .forEach(areaName => {
                const filteredForSingleArea = enquiryAreas.filter(area => area === areaName);
                counts[areaName] = filteredForSingleArea.length;
            });
        setCounts(counts);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <div className={cls.container}>
            {Object.keys(counts).map(key => {
                const c = counts[key];
                return <ActivityCard areaName={c.areaName} />;
            })}
        </div>
    );
};
