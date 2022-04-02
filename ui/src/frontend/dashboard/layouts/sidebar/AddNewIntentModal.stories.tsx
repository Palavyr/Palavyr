import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { AddNewIntentModal, AddNewIntentModalProps } from './AddNewIntentModal';


export default {
    title: "Dashboard/AreaContent/AddNewAreaModal",
    component: AddNewIntentModal
} as Meta;

const Template = (args: AddNewIntentModalProps) => <AddNewIntentModal {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    open: true,
    handleClose: () => null,
    setNewArea: ({}) => null
}
