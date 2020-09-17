import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { CustomNodeSelect, ISelectNodeType } from './CustomNodeSelect';
import { NodeTypeOptionsDefinition } from './NodeTypeOptions';


export default {
    title: "Dashboard/Conversation/CustomNodeSelect",
    component: CustomNodeSelect
} as Meta;


const Template = (args: ISelectNodeType) => <div style={{width: "20%"}}><CustomNodeSelect {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {

    onChange: () => {},
    option: "An Option",
    completeNodeTypes: NodeTypeOptionsDefinition
}
