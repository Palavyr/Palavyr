// ts-ignore

import React, { useContext, useState, useEffect, useCallback } from "react";
import { IntentNameDetails, EnquiryResources } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { Radar } from "react-chartjs-2";
import { DataPlot } from "./components/DataPlot";
import { getRandomColor } from "./DailyEnquiriesWeekly";
import colorLib from "@kurkle/color";
import { sum } from "lodash";
import { ChartData, ChartOptions } from "chart.js";

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

const calculateRadarData = (intentDetails: IntentNameDetails, enquiries: EnquiryResources) => {
    const intents = intentDetails.map(x => x.intentName);
    const enquiryIntents = enquiries.map(x => x.intentName);

    const counts: number[] = [];
    intents.forEach(intent => {
        const singleIntent = enquiryIntents.filter((x: string) => x === intent);
        counts.push(singleIntent.length);
    });

    const enquiryData: ChartData = {
        labels: intents,
        datasets: [{ label: "Enquiries", data: counts, backgroundColor: transparentize(getRandomColor("tobyface"), 0.5) }],
    };
    const enquiryOptions: any = {
        elements: {
            line: {
                borderWidth: 3,
            },
        },
        scales: {},
        maintainAspectRatio: false,
        responsive: true,

        plugins: {
            legend: {
                position: "top",
            },
        },
    };
    return { enquiryData, enquiryOptions };
};

export const EnquiryActivity = () => {
    const { repository, intentNameDetails } = useContext(DashboardContext);

    const [data, setData] = useState<any>();
    const [options, setOptions] = useState<any>();
    const [loadingspinner, setLoadingSpinner] = useState<boolean>(true);

    const loadEnquiries = useCallback(async () => {
        const enquiries = await repository.Enquiries.GetEnquiries();
        const { enquiryData, enquiryOptions } = calculateRadarData(intentNameDetails, enquiries);
        setData(enquiryData);
        setOptions(enquiryOptions);

        setLoadingSpinner(false);
    }, [intentNameDetails]);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    const hasData = (): boolean => {
        if (data === undefined) return false;

        const sumAllData = data => {
            const r: number[] = [];
            data.datasets.forEach(x => {
                r.push(sum(x.data));
            });
            return sum(r);
        };

        return (data.labels && data.labels.length > 0 && data.datasets && data.datasets.length > 0 && sumAllData(data) > 0) || false;
    };

    return (
        <DataPlot title="Activity Per Intent" subtitle="Learn which intents are seeing the most amount of traffic" hasData={hasData} loadingSpinner={loadingspinner}>
            <Radar data={data} options={options} />
        </DataPlot>
    );
};
