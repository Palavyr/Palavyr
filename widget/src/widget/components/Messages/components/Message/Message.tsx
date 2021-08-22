import React from "react";
import format from "date-fns/format";
import markdownIt from "markdown-it";
import markdownItSup from "markdown-it-sup";
import markdownItSanitizer from "markdown-it-sanitizer";
import markdownItClass from "@toycode/markdown-it-class";
import markdownItLinkAttributes from "markdown-it-link-attributes";

import "./styles.scss";
import { IMessage } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";

const useStyles = makeStyles(theme => ({
    timeStamp: {
        fontSize: "9px",
        // marginTop: "0.6rem",
        borderTop: "1px solid black",
        float: "right",
        background: "none",
    },
}));

export type MessageProps = {
    message: IMessage;
    showTimeStamp: boolean;
};

export const Message = ({ message, showTimeStamp = true }: MessageProps) => {
    const sanitizedHTML = markdownIt()
        .use(markdownItClass, {
            img: ["rcw-message-img"],
        })
        .use(markdownItSup)
        .use(markdownItSanitizer)
        .use(markdownItLinkAttributes, { attrs: { target: "_blank", rel: "noopener" } })
        .render(message.text);

    const cls = useStyles();
    return (
        <div className={`rcw-${message.sender}`}>
            <div dangerouslySetInnerHTML={{ __html: sanitizedHTML }} />
            {showTimeStamp && <span className={classNames("rcw-timestamp", cls.timeStamp)}>{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
