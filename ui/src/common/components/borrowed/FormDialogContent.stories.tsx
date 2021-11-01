import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { FormDialogContent, IFormDialogContent } from './FormDialogContent';


export default {
    title: "Common/Borrowed/FormDialogContent",
    component: FormDialogContent
} as Meta;


const Template = (args: IFormDialogContent) => <div style={{border: "1px solid lightgray", width: "20%"}}><FormDialogContent {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {
    loginEmail: "Test Email",
    loginPassword: "Test Password",
    setLoginEmail: () => {},
    setLoginPassword: () => {},
    setStatus: () => {},
    isPasswordVisible: true,
    setIsPasswordVisible: () => {},
    status: null,
    setRememberMe: () => {}
}

export const Secondary = Template.bind({});
Secondary.args = {
    loginEmail: "",
    loginPassword: "",
    setLoginEmail: () => {},
    setLoginPassword: () => {},
    setStatus: () => {},
    isPasswordVisible: true,
    setIsPasswordVisible: () => {},
    status: "invalidEmail",
    setRememberMe: () => {}
}