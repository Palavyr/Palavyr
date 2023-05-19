import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ChangePasswordDialog, IChangePasswordDialog } from './ChangePasswordDialog';


export default {
    title: "Common/Borrowed/ChangePasswordDialog",
    component: ChangePasswordDialog
} as Meta;


const Template = (args: IChangePasswordDialog) => <ChangePasswordDialog {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    setLoginStatus: () => {},
    onClose: () => {}
}
