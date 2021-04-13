import React from "react";
import format from "date-fns/format";
import "./styles.scss";
import { Link } from "@Palavyr-Types";

type Props = {
    message: Link;
    showTimeStamp: boolean;
};

export const Snippet = ({ message, showTimeStamp }: Props) => {
    return (
        <div>
            <div className="rcw-snippet">
                <h5 className="rcw-snippet-title">{message.title}</h5>
                <div className="rcw-snippet-details">
                    <a href={message.link} target={message.target} className="rcw-link">
                        {message.link}
                    </a>
                </div>
            </div>
            {showTimeStamp && <span className="rcw-timestamp">{format(message.timestamp, "hh:mm")}</span>}
        </div>
    );
};
