import { AreaNameDetail, AreaNameDetails, Enquiries, EnquiryRow } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useStyles } from "../demo/ColorOptions";
import { Chart, Point, ChartConfiguration } from "chart.js";
import { Line } from "react-chartjs-2";
import { DataPlot, EnquiryOptions } from "./components/DataPlot";
import { uniqBy } from "lodash";

type EnqDataSet = {
    label: string;
    data: number[];
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
    const uniqueDates = uniqBy(dates, (x) => x.dateString);

    const enquiryData: EnqDataSet[] = [];
    areaDetails.forEach((detail: AreaNameDetail) => {
        const areaDataResult: number[] = [];
        const areaName = detail.areaName;
        const areaEnquiries = enquiries.filter((enq: EnquiryRow) => enq.areaName === areaName);
        uniqueDates.forEach((x) => {
            const enquiriesOnDateInArea = areaEnquiries.filter((enq) => x.dateString === new Date(Date.parse(enq.timeStamp)).toDateString());
            areaDataResult.push(enquiriesOnDateInArea.length);
        });

        enquiryData.push({
            label: areaName,
            data: areaDataResult,
        });
    });

    const enquiryOptions = {
        responsive: true,
        plugins: {
            legend: {
                position: "top",
            },
            // title: {
            //     display: true,
            //     text: "Chart.js Line Chart",
            // },
        },
    };

    return { enquiryData, enquiryOptions, uniqueDates };
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
        const { enquiryData, enquiryOptions, uniqueDates } = calcualateDailEnquiryByDay(areaNameDetails, enquiries);

        const plotdata: PlotData = {
            labels: uniqueDates.map((x) => x.dateString),
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
        <DataPlot title="Daily Activity" subtitle="Learn how much daily activity each area is seeings" hasData={data !== undefined && data && data && data.labels.length > 0} loadingSpinner={loadingspinner}>
            <Line data={data} options={options} />
        </DataPlot>
    );
};
