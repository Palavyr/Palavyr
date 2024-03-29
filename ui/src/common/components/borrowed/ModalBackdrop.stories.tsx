import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ModalBackdrop, IModalBackdrop } from './ModalBackdrop';


export default {
    title: "Common/Borrowed/ModalBackdrop",
    component: ModalBackdrop
} as Meta;


const Template = (args: IModalBackdrop) => <ModalBackdrop {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    open: true
}
