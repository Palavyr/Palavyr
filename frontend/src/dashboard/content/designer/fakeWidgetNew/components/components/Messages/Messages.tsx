import React, { ElementType, useContext, useEffect, useRef, useState } from "react";
import format from "date-fns/format";
import { Loader } from "./components/Loader/Loader";
import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "../../context/WidgetContext";
import { scrollToBottom } from "../../utils/messages";
import { StandardComponents } from "../../componentRegistry/standardComponentRegistry";
import "./styles.scss";
import { MessageWrapper } from "../../BotResponse/utils/MessageWrapper";
import { LineSpacer } from "@common/components/typography/LineSpacer";
import { Message } from "./components/Message/Message";

type Props = {
    showTimeStamp: boolean;
    profileAvatar?: string;
};

const useStyles = makeStyles(theme => ({
    face: {
        height: "4rem",
    },
    messageTube: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
        overflowY: "auto",
        maxHeight: "100vh",
        zIndex: 9,
    }),
    messages: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.chatBubbleColor,
    }),
}));

export const Messages = ({ profileAvatar, showTimeStamp }: Props) => {
    const { preferences } = useContext(WidgetContext);
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        const registry = new StandardComponents();
        const fakeMessages = [
            [0, registry.makeProvideInfo("Welcome to the widget design page. You can customize the color palette of your widget across a varienty of component types.")],
            [0, registry.makeProvideInfo("For example, you are currently reading from the 'Provide Info' node type. It just presents some text or a picture.")],
            [0, registry.makeSelectOptions("This is the 'Select Options' node type used in the introdution.")],
            [1, () => <Message text="This is the selection response." style={{ float: "right" }} />],
            [0, registry.makeMultipleChoiceAsPathButtons("Buttons")],
            //     [0, registry.makeMultipleChoiceContinueButtons("Multiple Choice where all options lead to the same next node.")],
            //     [0, registry.makeTakeCurrency("Take Currency")],
            //     [0, registry.makeTakeNumber("Take Number")],
            //     [0, registry.makeTakeText("Take Text")],
            //     [0, registry.makeCollectDetails("Collect basic details from your users")],
            //     [0, registry.makeEndWithoutEmail("End without Email")],
        ];
        setMessages(fakeMessages as never);
    }, []);

    useEffect(() => {
        // scrollToBottom(messageRef.current);
    }, []);

    const messageRef = useRef<HTMLDivElement | null>(null);

    const cls = useStyles({ ...preferences });

    return (
        <div className={classNames("rcw-messages-container", cls.messageTube)} ref={messageRef} style={{ paddingBottom: "2rem" }}>
            <LineSpacer numLines={2} />
            {messages.map((data, index) => {
                const Component = data[1] as ElementType<any>;
                return (
                    <div className={classNames("rcw-message", cls.messages)} key={`${index}-${format(new Date(), "hh:mm")}`}>
                        {data[0] === 0 ? (
                            <MessageWrapper>
                                <Component />
                            </MessageWrapper>
                        ) : (
                            <Component />
                        )}
                    </div>
                );
            })}
            <Loader typing={true} />
        </div>
    );
};
