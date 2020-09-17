import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ConversationNode, IConversationNode } from './ConversationNode';
import { ParentNode, ChildNode1, ChildNode2 } from 'test/dummyNodeData/dummyNodes';
import "./ConversationNode.css";

export default {
    title: "Dashboard/Conversation/ConversationNode",
    component: ConversationNode
} as Meta;


const Template = (args: IConversationNode) => (
    <fieldset className="fieldset" id="tree-test">
        <div className="main-tree tree-wrap">
            <div className="tree-item"><ConversationNode {...args} /></div>
        </div>
    </fieldset>
);
export const SingleNode = Template.bind({});
SingleNode.args = {
    nodeList: [ParentNode],
    node: ParentNode,
    parentId: null,
    addNodes: () => { },
    setNodes: () => { },
    parentState: true,
    changeParentState: () => { },
    dynamicNodeTypes: {},
}


export const ThreeNodes = Template.bind({});
ThreeNodes.args = {
    nodeList: [ParentNode, ChildNode1, ChildNode2],
    node: ParentNode,
    parentId: null,
    addNodes: () => { },
    setNodes: () => { },
    parentState: true,
    changeParentState: () => { },
    dynamicNodeTypes: {},
}