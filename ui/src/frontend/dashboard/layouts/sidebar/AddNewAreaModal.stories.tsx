import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { AddNewAreaModal, IAddNewAreaModal } from './AddNewAreaModal';


export default {
    title: "Dashboard/AreaContent/AddNewAreaModal",
    component: AddNewAreaModal
} as Meta;

const Template = (args: IAddNewAreaModal) => <AddNewAreaModal {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    open: true,
    handleClose: () => null,
    setNewArea: ({}) => null
}
