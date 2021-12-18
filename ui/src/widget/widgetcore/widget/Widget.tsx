import * as React from "react";
import { makeStyles } from "@material-ui/core";
import { WidgetLayout } from "./WidgetLayout";
import "@widgetcore/widget/widget.module.scss";

export const useWidgetStyles = makeStyles(theme => ({
    // root: {
    //     display: "flex",
    //     flexDirection: "column",
    //     minHeight: "100%",
    //     height: "100%",
    //     // flexFlow: "column",
    //     // height: "100vh",
    //     overflowY: 'hidden'
    // },

    pwbox: {
        display: "flex",
        flexFlow: "column",
        height: "100vh",
        overflowY: 'hidden'
    },

    pwrow: {
        border: "1px dotted grey",
    },

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
        flex: "0 1 40px",
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
