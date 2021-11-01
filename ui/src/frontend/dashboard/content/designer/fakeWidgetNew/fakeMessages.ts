import { IMessage, WidgetNodeResource } from "@Palavyr-Types";
import { StandardComponents } from "./components/componentRegistry/standardComponentRegistry";
import { Message } from "./components/components/Messages/components/Message/Message";

export const createUserMessage = (text: string): IMessage => {
    return {
        text,
        type: "user",
        component: Message,
        sender: "client",
        showAvatar: false,
        timestamp: new Date(),
        unread: false,
        customId: undefined,
        props: undefined,
    };
};

export const createBotMessage = (text: string, componentFunc): IMessage => {
    const node: Partial<WidgetNodeResource> = {
        text: text,
    };

    return {
        text,
        type: "component",
        component: componentFunc(node, null, null, null),
        sender: "response",
        showAvatar: false,
        timestamp: new Date(),
        unread: false,
        customId: undefined,
        props: undefined,
    };
};

