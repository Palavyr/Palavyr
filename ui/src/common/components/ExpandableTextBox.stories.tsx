import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ExpandableTextBox, IExpandableTextBox } from './ExpandableTextBox';
import { Statement } from './Statement';


export default {
    title: "Common/ExpandableTextBox",
    component: ExpandableTextBox
} as Meta;

const NoChildTemplate = (args: IExpandableTextBox) => <ExpandableTextBox {...args} />;

export const Primary = NoChildTemplate.bind({});
Primary.args = {
    title: 'Intro Statement',
}

const WithStatementChildTemplate = (args: IExpandableTextBox) => {
    return (
        <ExpandableTextBox {...args}>
            <Statement title='Test Title'>
                This is some internal.
            </Statement>
        </ExpandableTextBox>
    )
};


export const WithChild = WithStatementChildTemplate.bind({});
WithChild.args = {
    title: 'Into Statement'
}
