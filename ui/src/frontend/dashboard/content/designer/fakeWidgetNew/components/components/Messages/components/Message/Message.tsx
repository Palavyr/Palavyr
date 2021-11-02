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
import { createUserMessage } from "frontend/dashboard/content/designer/fakeWidgetNew/fakeMessages";

const useStyles = makeStyles(theme => ({
    timeStamp: {
        fontSize: "9px",
        borderTop: "1px dashed gray",
        float: "right",
        background: "none",
    },
}));

export type MessageProps = {
    text: string;
    style: any;
};

// User Uses this message component
export const Message = ({ text, style }: MessageProps) => {
    const message: IMessage = createUserMessage(text);

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
        <div className={`rcw-client`}>
            <div dangerouslySetInnerHTML={{ __html: sanitizedHTML }} />
            {true && <span style={style} className={classNames("rcw-timestamp", cls.timeStamp)}>{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
