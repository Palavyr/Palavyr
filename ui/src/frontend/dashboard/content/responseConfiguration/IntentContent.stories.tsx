import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { IntentContent, IntentContentProps } from './IntentContent';


export default {
    title: "Dashboard/AreaContent",
    component: IntentContent
} as Meta;


const Template = (args: IntentContentProps) => <IntentContent {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
    areaName: "Test Name",
    classes: "",
    setLoaded: () => {},
    setViewName: () => {}
}
