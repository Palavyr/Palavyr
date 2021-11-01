import React, { useEffect } from "react";

import { Meta, Story } from "@storybook/react";
import { Widget, WidgetProps } from "./Widget";
import { getSelectedOption, options } from "@test-data/options";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { ConfigureMockClient } from "test/testUtils/ConfigureMockClient";
import { newConversation } from "@test-data/newConversation";
import { addResponseMessage, addUserMessage, closeUserDetails, dropMessages, toggleWidget } from "@store-dispatcher";
import { shortStaticConvoSequence } from "@test-data/conversationNodes";

const fakeKey = "secret-key";
const areaId = "abc123";
const routes = new PalavyrWidgetRepository(fakeKey).Routes;

const conf = new ConfigureMockClient();
conf.ConfigureGet(routes.newConvo(fakeKey, areaId), newConversation(areaId));

export default {
    title: "Widget/Widget",
    component: Widget,
    argTypes: {},
} as Meta;

export const ChatStart: Story<WidgetProps> = (args: WidgetProps) => {
    useEffect(() => {
        closeUserDetails();
        dropMessages();
        return () => {
            dropMessages();
        };
    });
    return <Widget {...args} />;
};
ChatStart.args = {
    option: getSelectedOption(areaId)
};

export const PopulatedChat: Story<WidgetProps> = (args: WidgetProps) => {
    useEffect(() => {
        dropMessages();
        closeUserDetails();
        shortStaticConvoSequence(areaId).forEach(convoNode => {
            if (convoNode.userResponse !== undefined){
                addUserMessage(convoNode.userResponse)
            } else {
                addResponseMessage(convoNode.text)
            }
        });
        return () => {
            dropMessages();
        };
    });
    return <Widget {...args} />;
};
PopulatedChat.args = {
    option: getSelectedOption(areaId),
};
