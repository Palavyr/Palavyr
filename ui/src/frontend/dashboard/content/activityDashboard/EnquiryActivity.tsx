import React, { useContext, useState, useEffect, useCallback } from "react";
import { AreaNameDetails, Enquiries } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { Radar } from "react-chartjs-2";
import { DataPlot } from "./components/DataPlot";
import { getRandomColor } from "./DailyEnquiriesWeekly";
import colorLib from "@kurkle/color";
import { sum } from "lodash";

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
    const { repository, areaNameDetails } = useContext(DashboardContext);

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

    const hasData = () => {
        if (data === undefined) return false;

        const sumAllData = data => {
            const r: number[] = [];
            data.datasets.forEach(x => {
                r.push(sum(x.data));
            });
            return sum(r);
        };

        return data.labels && data.labels.length > 0 && data.datasets && data.datasets.length > 0 && sumAllData(data) > 0;
    };

    return (
        <DataPlot
            title="Activity Per Area"
            subtitle="Learn which areas are seeing the most amount of traffic"
            hasData={hasData} //{ /*&& sum(data.datasets[0].data) > 0 */}
            loadingSpinner={loadingspinner}
        >
            <Radar data={data} options={options} />
        </DataPlot>
    );
};
