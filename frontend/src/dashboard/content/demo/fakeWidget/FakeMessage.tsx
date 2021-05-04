import { makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import format from "date-fns/format";
import React from "react";
import { FakeMessage } from "./fakeMessages";
import { getComponentToRender } from "./MessageParts";

const useStyles = makeStyles((theme) => ({
    message: {
        margin: "10px",
        display: "flex",
        wordWrap: "break-word",
    },
}));

export interface IFakeMessageProps {
    message: FakeMessage;
    index: number;
    prefs: WidgetPreferences;
}

export const FakeMessageComponent = ({ message, index, prefs }: IFakeMessageProps) => {
    const cls = useStyles();
    return (
        <>
            <div className={cls.message} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                {getComponentToRender(message, prefs)}
            </div>
        </>
    );
};
