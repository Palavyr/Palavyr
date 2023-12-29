import React, { useContext } from "react";
import format from "date-fns/format";
import { UserMessageData } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import "./styles.scss";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";


const useStyles = makeStyles<{}>((theme: any) => ({
    timeStamp: (prefs: WidgetPreferencesResource) => ({
        fontSize: "9px",
        borderTop: "1px dashed gray",
        float: "right",
        background: "none",
        fontFamily: prefs.fontFamily,
    }),
}));

export type MessageProps = {
    message: UserMessageData;
    showTimeStamp: boolean;
};

// User Uses this message component
export const UserMessage = ({ message, showTimeStamp = true }: MessageProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    return (
        <div className="pca-user-response">
            <PalavyrText>{message.text}</PalavyrText>
            {showTimeStamp && <span className={classNames("rcw-timestamp", cls.timeStamp)}>{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
