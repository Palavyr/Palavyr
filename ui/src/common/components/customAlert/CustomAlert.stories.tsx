import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { AlertType } from '@Palavyr-Types';
import { ICustomAlert, CustomAlert } from './CutomAlert';


export default {
    title: "Common/CustomAlert",
    component: CustomAlert
} as Meta;

const Template = (args: ICustomAlert) => <CustomAlert {...args} />;

const fakeAlert: AlertType = {
    title: "Fake Alert Title",
    message: "Fake alert Message"
}

export const Primary = Template.bind({});
Primary.args = {
    alertState: false,
    setAlert: (() => {}),
    alert: fakeAlert
}

