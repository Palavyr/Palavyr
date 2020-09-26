import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ResponseConfiguration, IResponseConfiguration } from './ResponseConfiguration';
import { useDashboardStyles } from 'dashboard/layouts/dashboard.styles';


export default {
    title: "Dashboard/Response/ResponseConfiguration",
    component: ResponseConfiguration
} as Meta;

const classes = useDashboardStyles();

const Template = (args: IResponseConfiguration) => <ResponseConfiguration {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
    classes: classes
}