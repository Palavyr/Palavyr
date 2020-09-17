import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { NavBar, INavBar } from './NavBar';


export default {
    title: "Landing/NavBar",
    component: NavBar
} as Meta;


const Template = (args: INavBar) => <NavBar {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    openRegisterDialog: () => {},
    openLoginDialog: () => {},
    handleMobileDrawerOpen: () => {},
    handleMobileDrawerClose: () => {},
    mobileDrawerOpen: true,
    selectedTab: "",
    selectTab: () => {}
}

