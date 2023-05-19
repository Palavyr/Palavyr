import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { BrandingIcon, IBrandingIcon } from './BrandingIcon';


export default {
    title: "Common/BrandingIcon",
    component: BrandingIcon
} as Meta;


const Template = (args: IBrandingIcon) => <BrandingIcon {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    iconType: "calculator",
    iconColor: "blue",
    iconSize: 54
}
