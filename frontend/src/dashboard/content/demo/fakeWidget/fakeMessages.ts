import { ElementType } from "react";
import { StandardComponents } from "../standardComponentDuplicate";

export type FakeMessage = {
    component: ElementType;
    sender: string;
    timestamp: Date;
    unread: boolean;
};

const firstMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeProvideInfo("Hello"),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

const firstClient = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeUserMessage("Hi there", new Date()),
    sender: "client",
    timestamp: new Date(),
    unread: false,
});

const secondMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeProvideInfo("Thanks for your interest."),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

const secondClient = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeUserMessage("Well you are welcome!", new Date()),
    sender: "client",
    timestamp: new Date(),
    unread: false,
});

const thirdMessage = (reg: StandardComponents): FakeMessage => ({
    component: reg.makeMultipleChoiceContinueButtons("Choose an option. There are a few options to choose from", ["First option", "Second option", "Another third Option"]),
    sender: "response",
    timestamp: new Date(),
    unread: false,
});

export const fakeMessages = (reg: StandardComponents) => [firstMessage(reg), firstClient(reg), secondMessage(reg), secondClient(reg), thirdMessage(reg)];
