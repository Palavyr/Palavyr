import * as React from 'react';
import { Story, Meta } from '@storybook/react';
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
