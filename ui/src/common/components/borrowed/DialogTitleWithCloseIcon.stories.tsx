import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { LoginAndRegisterButtons, LoginAndRegisterButtonsProps } from './DialogTitleWithCloseIcon';


export default {
    title: "Common/Borrowed/DialogTitleWithCloseIcon",
    component: LoginAndRegisterButtons
} as Meta;


const Template = (args: LoginAndRegisterButtonsProps) => <div style={{border: "1px solid lightgray", width: "20%"}}><LoginAndRegisterButtons {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {},
    title: "A test Title",
    disablePadding: false,
    disabled: false,
    paddingBottom: 12,
}
