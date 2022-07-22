export const dummy =  () => null;

// import React, { useEffect } from "react";

// import { Meta, Story } from "@storybook/react";
// import { getSelectedOption, options } from "@test-data/options";
// import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
// import { ConfigureMockClient } from "widget/test/testUtils/ConfigureMockClient";
// import { newConversation } from "@test-data/newConversation";
// import { addResponseMessage, addUserMessage, closeUserDetails, dropMessages, toggleWidget } from "@store-dispatcher";
// import { shortStaticConvoSequence } from "@frontend/dashboard/content/designer/dummy_conversations";
// import { Widget, WidgetProps } from "./Widget";

// const fakeKey = "secret-key";
// const IntentId = "abc123";
// const routes = new PalavyrWidgetRepository(fakeKey).Routes;

// const conf = new ConfigureMockClient();
// conf.ConfigureGet(routes.newConvo(fakeKey, IntentId), newConversation(IntentId));

// export default {
//     title: "Widget/Widget",
//     component: Widget,
//     argTypes: {},
// } as Meta;

// export const ChatStart: Story<WidgetProps> = (args: WidgetProps) => {
//     useEffect(() => {
//         closeUserDetails();
//         dropMessages();
//         return () => {
//             dropMessages();
//         };
//     });
//     return <Widget {...args} />;
// };
// ChatStart.args = {
//     option: getSelectedOption(IntentId)
// };

// export const PopulatedChat: Story<WidgetProps> = (args: WidgetProps) => {
//     useEffect(() => {
//         dropMessages();
//         closeUserDetails();
//         shortStaticConvoSequence(IntentId).forEach(convoNode => {
//             if (convoNode.userResponse !== undefined){
//                 addUserMessage(convoNode.userResponse)
//             } else {
//                 addResponseMessage(convoNode.text)
//             }
//         });
//         return () => {
//             dropMessages();
//         };
//     });
//     return <Widget {...args} />;
// };
// PopulatedChat.args = {
//     option: getSelectedOption(IntentId),
// };
