import { AreaNameDetail, AreaNameDetails, Enquiries, EnquiryRow } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useStyles } from "../demo/ColorOptions";
import { Line } from "react-chartjs-2";
import { DataPlot } from "./components/DataPlot";
import seedrandom from "seedrandom";

type EnqDataSet = {
    label: string;
    data: number[];
    borderColor: string;
    fill: boolean;
    cubicInterpolationMode: string;
    tension: number;
};

export const getRandomColor = (seed: number | string) => {
    var letters = "0123456789ABCDEF".split("");
    var color = "#";
    const rng = seedrandom(seed);
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(rng() * 16)];
    }
    return color;
};

const calcualateDailEnquiryByDay = (areaDetails: AreaNameDetails, enquiries: Enquiries) => {
    const dates = enquiries.map((x) => {
        const date = new Date(Date.parse(x.timeStamp));
        date.toLocaleDateString();
        return {
            date,
            dateString: date.toDateString(),
        };
    });

    // const uniqueDates = uniqBy(dates, (x) => x.dateString).sort((a, b) => a.date.getTime() - b.date.getTime());

    const lastSevenDays: Date[] = [];
    const now = new Date().getDate();
    for (let index = 0; index < 7; index++) {
        const newDay = new Date();
        newDay.setDate(now - index);

        lastSevenDays.push(newDay);
    }
    lastSevenDays.reverse();

    const enquiryData: EnqDataSet[] = [];
    areaDetails.forEach((detail: AreaNameDetail) => {
        const areaDataResult: number[] = [];
        const areaName = detail.areaName;
        const areaEnquiries = enquiries.filter((enq: EnquiryRow) => enq.areaName === areaName);
        lastSevenDays.forEach((previousDate) => {
            const enquiriesOnDateInArea = areaEnquiries.filter((enq) => {
                const timeStampDate = new Date(Date.parse(enq.timeStamp)).toDateString();
                const isEqual = previousDate.toDateString() === timeStampDate;
                return isEqual;
            });
            areaDataResult.push(enquiriesOnDateInArea.length);
        });

        enquiryData.push({
            label: areaName,
            data: areaDataResult,
            borderColor: getRandomColor(detail.areaName),
            fill: false,
            cubicInterpolationMode: "monotone",
            tension: 0.8,
        });
    });

    const enquiryOptions = {
        responsive: true,
        plugins: {
            legend: {
                position: "top",
            },
        },
        scales: {
            y: {
                ticks: {
                    beginAtZero: true,
                    callback: function (value) {
                        if (value % 1 === 0) {
                            return value;
                        }
                    },
                },
            },
        },
    };

    return { enquiryData, enquiryOptions, lastSevenDays };
};

export type Dataset = {
    data: number[];
    label: string;
};
export type DataSets = Dataset[];

export type EnquiryData = {
    labels: string[];
    datasets: DataSets;
};

export type EnqDatasetOptions = {
    responsive: boolean;
    plugins: {
        legend: {
            position: string;
        };
    };
};

type PlotData = {
    labels: string[];
    datasets: EnqDataSet[];
};

export const DailyEnquiriesWeekly = () => {
    const { repository, areaNameDetails } = useContext(DashboardContext);
    const cls = useStyles();

    const [data, setData] = useState<PlotData>();
    const [options, setOptions] = useState<EnqDatasetOptions>();
    const [loadingspinner, setLoadingSpinner] = useState<boolean>(true);

    const loadEnquiries = useCallback(async () => {
        const enquiries = await repository.Enquiries.getEnquiries();
        const { enquiryData, enquiryOptions, lastSevenDays } = calcualateDailEnquiryByDay(areaNameDetails, enquiries);

        const plotdata: PlotData = {
            labels: lastSevenDays.map((x) => x.toDateString()),
            datasets: enquiryData,
        };

        setData(plotdata);
        setOptions(enquiryOptions);

        setLoadingSpinner(false);
    }, [areaNameDetails]);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    return (
        <DataPlot
            title="7 Days of Daily Activity"
            subtitle="Learn about the daily activity of your widget, broken down by area"
            hasData={data !== undefined && data && data && data.labels.length > 0}
            loadingSpinner={loadingspinner}
        >
            <Line data={data} options={options} />
        </DataPlot>
    );
};
