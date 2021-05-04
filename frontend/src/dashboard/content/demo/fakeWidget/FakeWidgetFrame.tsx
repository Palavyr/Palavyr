import React from "react";
import { Card, Typography } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";

export interface FakeWidgetFrameProps {
    title: string;
    header: React.ReactNode;
    prefs: WidgetPreferences;
    children: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    wrap: {
        paddingLeft: "2rem",
        paddingRight: "2rem",
        marginBottom: "4rem",
    },
    widgetContainer: (props: WidgetPreferences) => ({
        height: "500px",
        width: "380px",
        display: "table",
        borderRadius: "9px",
        backgroundColor: props.selectListColor,
        boxShadow: theme.shadows[10],
    }),
    widgetHeader: (prefs: WidgetPreferences) => ({
        display: "tableCell",
        backgroundColor: prefs.headerColor,
        color: prefs.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        minHeight: "20%",
        maxHeight: "30%",
        wordWrap: "break-word",
        borderRadius: "5px",
        borderBottomLeftRadius: "0px",
        borderBottomRightRadius: "0px",
    }),
    widgetBody: {
        display: "tableCell",
        height: "inherit",
        borderBottomRightRadius: "5px",
        borderBottomLeftRadius: "5px",
        backgroundColor: theme.palette.common.white,
        overflowY: "scroll",
        "-webkit-overflow-scrolling": "touch",
    },
}));

export const FakeWidgetFrame = ({ title, header, prefs, children }: FakeWidgetFrameProps) => {
    const cls = useStyles(prefs);
    return (
        <div className={cls.wrap}>
            <div>
                <Typography align="center" variant="h6">
                    {title}
                </Typography>
            </div>
            <div className={cls.widgetContainer} id="chatWidgetPanel">
                <Card className={cls.widgetHeader}>{header}</Card>
                <div className={cls.widgetBody}>{children}</div>
            </div>
        </div>
    );
};
