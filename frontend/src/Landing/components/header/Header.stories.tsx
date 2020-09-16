import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { Header } from './Header';
import { IHaveWidth } from '@Palavyr-Types';


export default {
    title: "Landing/Header",
    component: Header
} as Meta;


const Template = (args: IHaveWidth) => <Header {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    width: "100px"
}

