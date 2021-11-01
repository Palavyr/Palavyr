import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { DialogTitleWithCloseIcon, IDialogTitleWithCloseIcon } from './DialogTitleWithCloseIcon';


export default {
    title: "Common/Borrowed/DialogTitleWithCloseIcon",
    component: DialogTitleWithCloseIcon
} as Meta;


const Template = (args: IDialogTitleWithCloseIcon) => <div style={{border: "1px solid lightgray", width: "20%"}}><DialogTitleWithCloseIcon {...args} /></div>;

export const Primary = Template.bind({});
Primary.args = {
    onClose: () => {},
    title: "A test Title",
    disablePadding: false,
    disabled: false,
    paddingBottom: 12,
}
