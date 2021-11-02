import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ResponseConfiguration } from './ResponseConfiguration';


export default {
    title: "Dashboard/Response/ResponseConfiguration",
    component: ResponseConfiguration
} as Meta;


const Template = (args: any) => <ResponseConfiguration {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
}