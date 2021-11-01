import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, CircularProgress, Card } from "@material-ui/core";
import { Align } from "frontend/dashboard/layouts/positioning/Align";
import React from "react";
import { NoActivityComponent } from "./NoActivityComponent";

export type Dataset = {
    data: number[];
    label: string;
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
    hasData(): boolean;
    loadingSpinner: boolean;
    children: React.ReactNode;
    title: string;
    subtitle?: string;
}

const useStyles = makeStyles(theme => ({
    title: {
        padding: "1rem",
    },
    plotCard: {},

    fallback: {
        margin: "2rem",
    },
}));

export const DataPlot = ({ title, subtitle = "", hasData, loadingSpinner, children }: DataPlotProps) => {
    const cls = useStyles();

    return loadingSpinner ? (
        <span className={cls.fallback}>
            <Align>
                <CircularProgress />
            </Align>
        </span>
    ) : (
        <>
            <div style={{ height: "2rem" }}></div>
            <AreaConfigurationHeader title={title} subtitle={subtitle} divider />
            {hasData() ? children : <NoActivityComponent />}
            <div style={{ height: "4rem" }}></div>
        </>
    );
};
