import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { NodeTypeSelector, INodeTypeSelector } from './NodeTypeSelector';
import { NodeTypeOptionsDefinition } from './NodeTypeOptions';
import { DummyConvo } from 'test/dummyNodeData/dummyNodes';


export default {
    title: "Dashboard/Conversation/NodeTypeSelector",
    component: NodeTypeSelector
} as Meta;



const Template = (args: INodeTypeSelector) => <div style={{width: "20%"}}><NodeTypeSelector {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {
    node: DummyConvo[0],
    nodeList: DummyConvo,
    addNodes: () => {},
    setNodes: () => {},
    parentState: true,
    changeParentState: () => {},
    dynamicNodeTypes: NodeTypeOptionsDefinition
}
