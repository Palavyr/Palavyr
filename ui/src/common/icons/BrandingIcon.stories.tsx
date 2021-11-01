import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
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
