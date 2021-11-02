import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { IconBox, IIconBox } from './IconBox';


export default {
    title: "Landing/IconBox",
    component: IconBox
} as Meta;


const Template = (args: IIconBox) => <div style={{border: "1px solid black"}}><IconBox {...args} /></div>;
export const Primary = Template.bind({});
Primary.args = {
    iconType: "calculator",
    iconTitle: "A cool title",
    iconSize: 35,
    iconColor: "red",
    children: (<span>WOW</span>)
}

