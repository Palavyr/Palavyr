import { makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import format from "date-fns/format";
import React from "react";
import { FakeMessage } from "./fakeMessages";
import { getComponentToRender } from "./MessageParts";
import { v4 as uuid } from "uuid";

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
    const Component = getComponentToRender(message, prefs);

    const id = uuid();

    return (
        <>
            <div className={cls.message} key={`${index}-${format(message.timestamp, "hh:mm")}`}>
                {Component}
            </div>
        </>
    );
};
