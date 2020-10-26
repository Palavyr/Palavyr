import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ConversationNodeEditor, IConversationNodeEditor } from './ConversationNodeEditor';


export default {
    title: "Dashboard/Conversation/ConversationNodeEditor",
    component: ConversationNodeEditor
} as Meta;

const Template = (args: IConversationNodeEditor) => <ConversationNodeEditor {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    modalState: true,
    setModalState: () => null,
    node: {},
    changeParentState: () => null,
    parentState: true,
    setNodes: () => null,
    nodeList: [{}]
}
