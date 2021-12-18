import * as React from "react";
import { makeStyles } from "@material-ui/core";
import { WidgetLayout } from "./WidgetLayout";
import "@widgetcore/widget/widget.module.scss";

export const useWidgetStyles = makeStyles(theme => ({
    pwbox: {
        display: "flex",
        flexFlow: "column",
        height: "100vh",
        overflowY: "hidden",
    },

    pwrow: {},

    pheader: {
        flex: "0 1 auto",
    },

    pcontent: {
        flexFlow: "column",
        flexGrow: 1,
        overflowY: "auto",
        width: "100%",
    },
    pfooter: {
        display: "flex",
        flex: "0 1 30px",
        flexFlow: "row",
        justifyContent: "space-between",
    },
}));

export interface WidgetProps {
    designMode?: boolean;
}

export const Widget = ({ designMode }: WidgetProps) => {
    const cls = useWidgetStyles();
    return (
        <div className={cls.pwbox}>
            <WidgetLayout designMode={designMode} />
        </div>
    );
};
