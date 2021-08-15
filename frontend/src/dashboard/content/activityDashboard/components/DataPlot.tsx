import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles, CircularProgress, Card, Fade } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";
import React from "react";
import { NoActivityComponent } from "./NoActivityComponent";

const useStyles = makeStyles(theme => ({
    title: {
        padding: "1rem",
    },
    plotCard: {
        margin: "2rem",
        padding: "2rem",
        borderRadius: "12px",
    },
    fallback: {
        margin: "2rem",
    },
}));

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
    hasData: boolean;
    loadingSpinner: boolean;
    children: React.ReactNode;
    title: string;
    subtitle?: string;
}

export const DataPlot = ({ title, subtitle = "", hasData, loadingSpinner, children }: DataPlotProps) => {
    const cls = useStyles();

    return loadingSpinner ? (
        <span className={cls.fallback}>
            <Align>
                <CircularProgress />
            </Align>
        </span>
    ) : (
        <Fade>
            <Card className={cls.plotCard}>
                <AreaConfigurationHeader title={title} subtitle={subtitle} light divider />
                {hasData ? children : <NoActivityComponent />}
            </Card>
        </Fade>
    );
};
