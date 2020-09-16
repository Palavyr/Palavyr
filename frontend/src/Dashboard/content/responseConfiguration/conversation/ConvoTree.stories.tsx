import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ConvoTree, IConvoTree } from './ConvoTree';
import { ConvoNode } from '@Palavyr-Types';


export default {
    title: "Dashboard/Conversation/ConvoTree",
    component: ConvoTree
} as Meta;

const Template = (args: IConvoTree) => <ConvoTree {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
    treeName: "Tree Name"
}
