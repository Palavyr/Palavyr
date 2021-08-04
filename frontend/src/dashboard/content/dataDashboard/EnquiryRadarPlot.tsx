import React, { useContext, useState, useEffect, useCallback } from "react";
import { makeStyles, Grid, Card, Typography } from "@material-ui/core";
import { AreaNameDetails, Enquiries } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { Radar } from "react-chartjs-2";

const useStyles = makeStyles((theme) => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
    },
}));

export type Dataset = {
    data: number[];
};
export type DataSets = Dataset[];

export type EnquiryData = {
    labels: string[];
    datasets: DataSets;
};

export type EnquiryOptions = {
    elements: {
        lins: {
            borderwidth: number;
        };
    };
};

export const EnquiryRadarPlot = () => {
    const { repository, areaNameDetails } = useContext(DashboardContext);
    const cls = useStyles();

    const [data, setData] = useState<EnquiryData>();
    const [options, setOptions] = useState();

    const loadEnquiries = useCallback(async () => {
        const enquiries = await repository.Enquiries.getEnquiries();
        const { enquiryData, enquiryOptions } = calculateRadarData(areaNameDetails, enquiries);
        setData(enquiryData);
        setOptions(enquiryOptions);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return <Radar data={data} options={options} />;
};

const calculateRadarData = (areaDetails: AreaNameDetails, enquiries: Enquiries) => {
    const areas = areaDetails.map((x) => x.areaName);
    const enquiryAreas = enquiries.map((x) => x.areaName);

    const counts: number[] = [];
    areas.forEach((area) => {
        const singleArea = enquiryAreas.filter((x: string) => x === area);
        counts.push(singleArea.length);
    });

    const enquiryData = {
        labels: areas,
        datasets: [{ data: counts }],
    };
    const enquiryOptions = {
        elements: {
            line: {
                borderWidth: 3,
            },
        },
    };
    return { enquiryData, enquiryOptions };
};
