import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { LandingPage } from './Landing';
import {MemoryRouter } from 'react-router';


export default {
    title: "Landing/LandingPage",
    component: LandingPage
} as Meta;


const Template = () => <MemoryRouter><LandingPage /></MemoryRouter>;
export const Primary = Template.bind({});
Primary.args = {}
