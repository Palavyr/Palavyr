import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { HighlightedInformation, IHighlightedInformation } from './HighlightedInformation';


export default {
    title: "Common/Borrowed/HighlightedInformation",
    component: HighlightedInformation
} as Meta;


const Template = (args: IHighlightedInformation) => <div style={{width: "30%"}}><HighlightedInformation {...args}><span>Test Text</span></HighlightedInformation></div>;

export const Primary = Template.bind({});
Primary.args = {}
