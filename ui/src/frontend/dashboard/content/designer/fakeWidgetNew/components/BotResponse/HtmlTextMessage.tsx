import React from "react";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";

const useStyles = makeStyles(theme => ({
    outer: {},
    inner: {
        fontSize: "15px",
    },
}));
export interface IHtmlTextMessage {
    message: string;
    showTimeStamp?: boolean;
    className: string;
}

const getNameContext = () => "";
export const HtmlTextMessage = ({ message, className, showTimeStamp = true }: IHtmlTextMessage) => {
    const cls = useStyles();

    return (
        <div className={classNames(cls.outer, className)}>
            <div className={classNames(cls.inner, "rcw-message-text")} dangerouslySetInnerHTML={{ __html: message.replace("{%name%}", getNameContext()).replace("{%Name%}", getNameContext()) }} />
        </div>
    );
};
