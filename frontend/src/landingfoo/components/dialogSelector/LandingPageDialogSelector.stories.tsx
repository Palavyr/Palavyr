import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { LandingPageDialogSelector, ILandingPageDialogSelector } from './LandingPageDialogSelector';


export default {
    title: "Common/Borrowed/LandingPageDialogSelector",
    component: LandingPageDialogSelector
} as Meta;


const Template = (args: ILandingPageDialogSelector) => <LandingPageDialogSelector {...args} />;

const stdArgs = {
    openTermsDialog: () => {},
    openRegisterDialog: () => {},
    openLoginDialog: () => {},
    openChangePasswordDialog: () => {},
    onClose: () => {}
}

export const Primary_Login = Template.bind({});
Primary_Login.args = {
    dialogOpen: "login",
    ...stdArgs
}
