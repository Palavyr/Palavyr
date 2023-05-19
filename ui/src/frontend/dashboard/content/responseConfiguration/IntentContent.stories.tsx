import * as React from 'react';
import { Story, Meta } from '@storybook/react';
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
