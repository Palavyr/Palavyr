import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ResponseConfiguration } from './ResponseConfiguration';


export default {
    title: "Dashboard/Response/ResponseConfiguration",
    component: ResponseConfiguration
} as Meta;


const Template = (args: any) => <ResponseConfiguration {...args} />;

// TODO: Mock api call to get data
export const Primary = Template.bind({});
Primary.args = {
    intentId: "abc-123",
}
