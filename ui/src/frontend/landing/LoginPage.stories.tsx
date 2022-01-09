import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { LoginPage } from './LoginPage';
import {MemoryRouter } from 'react-router';


export default {
    title: "Landing/LandingPage",
    component: LoginPage
} as Meta;


const Template = () => <MemoryRouter><LoginPage /></MemoryRouter>;
export const Primary = Template.bind({});
Primary.args = {}
