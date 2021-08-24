import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, CircularProgress, Card } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";
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
    plotCard: {
        margin: "2rem",
        padding: "2rem",
        borderRadius: "12px",
        border: "1px solid black",
        backgroundColor: theme.palette.common.white,
        boxShadow: theme.shadows[10],
    },
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
            <AreaConfigurationHeader title={title} subtitle={subtitle} light divider />
            {hasData() ? children : <NoActivityComponent />}
        </>
    );
};
