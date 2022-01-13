import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { FormCard, IFormDialog } from './FormCard';
import { Button } from '@material-ui/core';


export default {
    title: "Common/Borrowed/FormDialog",
    component: FormCard
} as Meta;


const Template = (args: IFormDialog) => <FormCard {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    open: true,
    onClose: () => {},
    headline: "A test headline",
    loading: true,
    onFormSubmit: () => {},
    content: <div>A div content</div>,
    actions: <Button>Test Button Action</Button>,
    hideBackdrop: true
}
