import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
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
