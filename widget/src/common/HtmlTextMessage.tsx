import React from "react";
import markdownIt from "markdown-it";
import markdownItSup from "markdown-it-sup";
import markdownItSanitizer from "markdown-it-sanitizer";
import markdownItClass from "@toycode/markdown-it-class";
import markdownItLinkAttributes from "markdown-it-link-attributes";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";

const useStyles = makeStyles(theme => ({
    outer: {
        fontSize: "16px",
    },
    inner: {
        fontSize: "18px",
    },
}));
export interface IHtmlTextMessage {
    message: string;
    showTimeStamp?: boolean;
    className: string;
}
export const HtmlTextMessage = ({ message, className, showTimeStamp = true }: IHtmlTextMessage) => {
    // const sanitizedHTML = markdownIt()
    //     .use(markdownItClass, {
    //         img: ["rcw-message-img"],
    //     })
    //     .use(markdownItSup)
    //     .use(markdownItSanitizer)
    //     .use(markdownItLinkAttributes, { attrs: { target: "_blank", rel: "noopener" } })
    //     .render(message);
    const cls = useStyles();

    return (
        <div className={classNames(cls.outer, className)}>
            <div className={classNames(cls.inner, "rcw-message-text")} dangerouslySetInnerHTML={{ __html: message }} />
        </div>
    );
};
