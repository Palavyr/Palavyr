import { GlobalState } from "@Palavyr-Types";
import React, { useRef, useEffect } from "react";
import { useSelector } from "react-redux";
import "./style.scss";

const send = require("assets/send_button.svg") as string;

type Props = {
    placeholder: string;
    disabledInput: boolean;
    autofocus: boolean;
    sendMessage: (event: any) => void;
    buttonAlt: string;
    onTextInputChange?: (event: any) => void;
};

export const Sender = ({ sendMessage, placeholder, disabledInput, autofocus, onTextInputChange, buttonAlt }: Props) => {
    const showChat = useSelector((state: GlobalState) => state.behaviorReducer.showChat);
    const inputRef = useRef(null);

    useEffect(() => {
        // @ts-ignore
        if (showChat) inputRef.current?.focus();
    }, [showChat]);

    return (
        <form className="rcw-sender" onSubmit={sendMessage}>
            <input
                type="text"
                className="rcw-new-message"
                name="message"
                ref={inputRef}
                placeholder={placeholder}
                disabled={disabledInput}
                autoFocus={autofocus}
                autoComplete="off"
                onChange={onTextInputChange}
            />
            <button type="submit" className="rcw-send">
                <img src={send} className="rcw-send-icon" alt={buttonAlt} />
            </button>
        </form>
    );
};
