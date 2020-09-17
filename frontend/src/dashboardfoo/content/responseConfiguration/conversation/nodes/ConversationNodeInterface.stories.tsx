import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ConversationNodeInterface, IConversationNodeInterface } from './ConversationNodeInterface';
import { DummyConvo } from 'test/dummyNodeData/dummyNodes';


export default {
    title: "Common/Borrowed/ConversationNodeInterface",
    component: ConversationNodeInterface
} as Meta;


const Template = (args: IConversationNodeInterface) => <ConversationNodeInterface {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    node: DummyConvo[0],
    nodeList: DummyConvo,
    addNodes: () => {},
    setNodes: () => {},
    parentState: true,
    changeParentState: () => {},
    optionPath: "",
    dynamicNodeTypes: {}
}
