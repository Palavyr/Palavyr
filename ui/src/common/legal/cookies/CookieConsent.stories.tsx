import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { CookieConsent, ICookieConsent } from './CookieConsent';


export default {
    title: "Landing/CookieConsent",
    component: CookieConsent
} as Meta;


const Template = (args: ICookieConsent) => <CookieConsent {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    handleCookieRulesDialogOpen: () => {},
    setOpen: false,
}
