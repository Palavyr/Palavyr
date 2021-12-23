import React from "react";
import format from "date-fns/format";

import "./styles.scss";
import { IMessage } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { MESSAGE_SENDER } from "@widgetcore/constants";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyles = makeStyles(theme => ({
    timeStamp: {
        fontSize: "9px",
        borderTop: "1px dashed gray",
        float: "right",
        background: "none",
    },
}));

export type MessageProps = {
    message: IMessage;
    showTimeStamp: boolean;
};

// User Uses this message component
export const Message = ({ message, showTimeStamp = true }: MessageProps) => {
    const cls = useStyles();
    return (
        <div className={`rcw-${MESSAGE_SENDER.CLIENT}`}>
            <PalavyrText>{message.text}</PalavyrText>
            {showTimeStamp && <span className={classNames("rcw-timestamp", cls.timeStamp)}>{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
