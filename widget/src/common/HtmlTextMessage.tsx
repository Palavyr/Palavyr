import React from "react";
import markdownIt from "markdown-it";
import markdownItSup from "markdown-it-sup";
import markdownItSanitizer from "markdown-it-sanitizer";
import markdownItClass from "@toycode/markdown-it-class";
import markdownItLinkAttributes from "markdown-it-link-attributes";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { getNameContext } from "@store-dispatcher";

const useStyles = makeStyles(theme => ({
    outer: {
    },
    inner: {
        fontSize: "15px",
    },
}));
export interface IHtmlTextMessage {
    message: string;
    showTimeStamp?: boolean;
    className: string;
}
export const HtmlTextMessage = ({ message, className, showTimeStamp = true }: IHtmlTextMessage) => {
    const cls = useStyles();

    return (
        <div className={classNames(cls.outer, className)}>
            <div className={classNames(cls.inner, "rcw-message-text")} dangerouslySetInnerHTML={{ __html: message.replace("{%name%}", getNameContext()).replace("{%Name%}", getNameContext()) }} />
        </div>
    );
};
