import { IMessage, WidgetNodeResource } from "@Palavyr-Types";
import { StandardComponents } from "./components/componentRegistry/standardComponentRegistry";
import { Message } from "./components/components/Messages/components/Message/Message";

const createUserMessage = (text: string): IMessage => {
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

const createBotMessage = (text: string, componentFunc): IMessage => {
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
const registry = new StandardComponents();

export const fakeMessages: IMessage[] = [createBotMessage("Welcome to Palavyr.com", registry.makeProvideInfo)];
