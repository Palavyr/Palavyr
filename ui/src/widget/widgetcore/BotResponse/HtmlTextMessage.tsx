import React, { useContext } from "react";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { useAppContext } from "widget/hook";

const useStyles = makeStyles(theme => ({
    outer: (widgetPreferences: WidgetPreferences) => ({
        fontFamily: widgetPreferences.fontFamily,
    }),
    inner: (widgetPreferences: WidgetPreferences) => ({
        fontSize: "15px",
        fontFamily: widgetPreferences.fontFamily,
    }),
}));
export interface IHtmlTextMessage {
    message: string;
    showTimeStamp?: boolean;
    className: string;
}
export const HtmlTextMessage = ({ message, className, showTimeStamp = true }: IHtmlTextMessage) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    const { name } = useAppContext();

    return (
        <div className={classNames(cls.outer, className)}>
            <div className={classNames(cls.inner, "rcw-message-text")} dangerouslySetInnerHTML={{ __html: message.replace("{%name%}", name).replace("{%Name%}", name) }} />
        </div>
    );
};
