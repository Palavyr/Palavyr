import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ColoredButton, IColoredButton } from './ColoredButton';


export default {
    title: "Common/Borrowed/ColoredButton",
    component: ColoredButton
} as Meta;


const Template = (args: IColoredButton) => <ColoredButton {...args} >Test Text</ColoredButton>;

export const Primary = Template.bind({});
Primary.args = {
    color: "primary",
    onClick: () => {},
    variant: "contained",
    type: "button"
}
