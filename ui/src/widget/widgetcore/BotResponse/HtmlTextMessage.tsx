import React, { useContext } from "react";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";
import { WidgetContext } from "@widgetcore/context/WidgetContext";


const useStyles = makeStyles<{}>((theme: any) => ({
    outer: (widgetPreferences: WidgetPreferencesResource) => ({
        fontFamily: widgetPreferences.fontFamily,
    }),
    inner: (widgetPreferences: WidgetPreferencesResource) => ({
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
    const {
        preferences,
        context: { name },
    } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    return (
        <div className={classNames(cls.outer, className)}>
            <div className={classNames(cls.inner, "rcw-message-text")} dangerouslySetInnerHTML={{ __html: message.replace("{%name%}", name).replace("{%Name%}", name) }} />
        </div>
    );
};
