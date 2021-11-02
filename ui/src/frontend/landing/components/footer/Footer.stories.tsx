import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { Footer, IFooter } from './Footer';


export default {
    title: "Landing/Footer",
    component: Footer
} as Meta;


const Template = (args: IFooter) => <Footer {...args} />;

export const Primary = Template.bind({});
Primary.args = { width: "100px" }

