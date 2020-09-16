import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { Footer } from './Footer';
import { IHaveWidth } from '@Palavyr-Types';


export default {
    title: "Landing/Footer",
    component: Footer
} as Meta;


const Template = (args: IHaveWidth) => <Footer {...args} />;

export const Primary = Template.bind({});
Primary.args = { width: "100px" }

