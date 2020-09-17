import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { LoginActions, ILoginActions } from './LoginActions';


export default {
    title: "Landing/LoginActions",
    component: LoginActions
} as Meta;


const Template = (args: ILoginActions) => <LoginActions {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    isLoading: false,
    openChangePasswordDialog: () => {}
}
