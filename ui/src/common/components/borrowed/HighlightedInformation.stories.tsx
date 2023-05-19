import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { HighlightedInformation, IHighlightedInformation } from './HighlightedInformation';


export default {
    title: "Common/Borrowed/HighlightedInformation",
    component: HighlightedInformation
} as Meta;


const Template = (args: IHighlightedInformation) => <div style={{width: "30%"}}><HighlightedInformation {...args}><span>Test Text</span></HighlightedInformation></div>;

export const Primary = Template.bind({});
Primary.args = {}
