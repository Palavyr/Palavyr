import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { CookieRules, ICookieRules } from './CookieRules';


export default {
    title: "Landing/CookieRules",
    component: CookieRules
} as Meta;


const Template = (args: ICookieRules) => <CookieRules {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {}
}
