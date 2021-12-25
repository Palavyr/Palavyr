import React from "react";
import format from "date-fns/format";
import { UserMessageData } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import "./styles.scss";

const useStyles = makeStyles(theme => ({
    timeStamp: {
        fontSize: "9px",
        borderTop: "1px dashed gray",
        float: "right",
        background: "none",
    },
}));

export type MessageProps = {
    message: UserMessageData;
    showTimeStamp: boolean;
};

// User Uses this message component
export const UserMeess = ({ message, showTimeStamp = true }: MessageProps) => {
    const cls = useStyles();
    return (
        <div className="pca-user-response">
            <PalavyrText>{message.text}</PalavyrText>
            {showTimeStamp && <span className={classNames("rcw-timestamp", cls.timeStamp)}>{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
