import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { AddNewIntentModal, AddNewIntentModalProps } from './AddNewIntentModal';


export default {
    title: "Dashboard/IntentContent/AddNewIntentModal",
    component: AddNewIntentModal
} as Meta;

const Template = (args: AddNewIntentModalProps) => <AddNewIntentModal {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    open: true,
    handleClose: () => null,
    setNewIntent: ({}) => null
}
