import React, { useContext, useState, useEffect, useCallback } from "react";
import { makeStyles } from "@material-ui/core";
import { AreaNameDetails, Enquiries } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { Radar } from "react-chartjs-2";
import { DataPlot } from "./components/DataPlot";
import { getRandomColor } from "./DailyEnquiriesWeekly";
import colorLib from "@kurkle/color";
import { sum } from "lodash";

const useStyles = makeStyles(theme => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
    },
    fallback: {
        margin: "2rem",
    },
}));

export type Dataset = {
    data: number[];
    label: string;
    backgroundColor: string;
};
export type DataSets = Dataset[];

export type EnquiryData = {
    labels: string[];
    datasets: DataSets;
};

export type EnquiryOptions = {
    elements: {
        line: {
            borderWidth: number;
        };
    };
};

export interface DataPlotProps {
    hasData: boolean;
    loadingSpinner: boolean;
    children: React.ReactNode;
}

export const transparentize = (value, opacity) => {
    var alpha = opacity === undefined ? 0.5 : 1 - opacity;
    return colorLib(value)
        .alpha(alpha)
        .rgbString();
};

const calculateRadarData = (areaDetails: AreaNameDetails, enquiries: Enquiries) => {
    const areas = areaDetails.map(x => x.areaName);
    const enquiryAreas = enquiries.map(x => x.areaName);

    const counts: number[] = [];
    areas.forEach(area => {
        const singleArea = enquiryAreas.filter((x: string) => x === area);
        counts.push(singleArea.length);
    });

    const enquiryData: EnquiryData = {
        labels: areas,
        datasets: [{ label: "Enquiries", data: counts, backgroundColor: transparentize(getRandomColor("tobyface"), 0.5) }],
    };
    const enquiryOptions = {
        elements: {
            line: {
                borderWidth: 3,
            },
        },
        scales: {},
    };
    return { enquiryData, enquiryOptions };
};

export const EnquiryActivity = () => {
    const { repository, areaNameDetails } = useContext(DashboardContext);
    const cls = useStyles();

    const [data, setData] = useState<EnquiryData>();
    const [options, setOptions] = useState<EnquiryOptions>();
    const [loadingspinner, setLoadingSpinner] = useState<boolean>(true);

    const loadEnquiries = useCallback(async () => {
        const enquiries = await repository.Enquiries.getEnquiries();
        const { enquiryData, enquiryOptions } = calculateRadarData(areaNameDetails, enquiries);
        setData(enquiryData);
        setOptions(enquiryOptions);

        setLoadingSpinner(false);
    }, [areaNameDetails]);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <DataPlot
            title="Activity Per Area"
            subtitle="Learn which areas are seeing the most amount of traffic"
            hasData={data !== undefined && data && data.labels && data.labels.length > 0 && data.datasets && data.datasets.length > 0 /*&& sum(data.datasets[0].data) > 0 */}
            loadingSpinner={loadingspinner}
        >
            <Radar data={data} options={options} />
        </DataPlot>
    );
};
