import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { RegisterDialog, IRegisterDialog } from './RegisterDialog';


export default {
    title: "Landing/RegisterDialog",
    component: RegisterDialog
} as Meta;


const Template = (args: IRegisterDialog) => <RegisterDialog {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {},
    openTermsDialog: () => {},
    status: null,
    setStatus: () => {}
}
