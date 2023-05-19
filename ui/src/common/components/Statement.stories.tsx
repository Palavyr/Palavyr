import * as React from 'react';
import { Story, Meta } from '@storybook/react';

import { Statement } from "./Statement";

export default {
    title: "Common/Statement",
    component: Statement
} as Meta;

const wrapperStyle = {border: "1px solid black"};
const Template = (args) => (<div style={wrapperStyle}><Statement {...args} /></div>);
const TemplateWithChildren = (args) => (<div style={wrapperStyle}><Statement{...args}><span>This is a JSX span</span></Statement></div>)

export const Primary = Template.bind({});
Primary.args = {
    title: 'Intro Statement',
    details: 'Use this section to create an introduction statement for your estimate.You can make it clear that fees are estimate only, or provide context for your client to understand their estimate.',
}


export const UsingJSX = TemplateWithChildren.bind({});
UsingJSX.args = {
    title: 'Intro Statement'
}

export const UsingJSXAndDetails = TemplateWithChildren.bind({});
UsingJSXAndDetails.args = {
    title: 'Intro Statement',
    details: "This a test details - comes before the children."
}
