import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { VisibilityPasswordTextField, IVisibilityPasswordTextField } from './VisibilityPasswordTextField';


export default {
    title: "Common/Borrowed/VisibilityPasswordTextField",
    component: VisibilityPasswordTextField
} as Meta;


const Template = (args: IVisibilityPasswordTextField) => <VisibilityPasswordTextField {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    loginEmail: "test",
    loginPassword: "test",
    setLoginEmail: () => {},
    setLoginPassword: () => {},
    setStatus: () => {},
    isPasswordVisible: false,
    setIsPasswordVisible: () => {},
    status: null,
    variant: "outlined"

}
