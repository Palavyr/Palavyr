import * as React from 'react';
import { Story, Meta } from '@storybook/react';

import { SensitiveGridRowText, ISensitiveGridRowText } from "./SensitiveGridRowText";

export default {
    title: "Common/SensitiveGridRowText",
    component: SensitiveGridRowText
} as Meta;

const Template = (args: ISensitiveGridRowText) => <SensitiveGridRowText {...args} ><span>Child Text</span></SensitiveGridRowText>;

export const Primary = Template.bind({});
Primary.args = {
    title: 'Intro Statement',
    details: 'Text text',
    onClick: (() => {}),
}

export const WithCurrentValue = Template.bind({});
WithCurrentValue.args = {
    ...Primary.args,
    currentValue: "The Current Value"
}




