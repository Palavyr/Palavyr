import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { IntentContent, IntentContentProps } from './IntentContent';


export default {
    title: "Dashboard/IntentContent",
    component: IntentContent
} as Meta;


const Template = (args: IntentContentProps) => <IntentContent {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    intentId: "abc-123",
    intentName: "Test Name",
    classes: "",
    setLoaded: () => {},
    setViewName: () => {}
}
