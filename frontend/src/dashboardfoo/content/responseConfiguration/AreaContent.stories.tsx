import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { AreaContent, IAreaContent } from './AreaContent';


export default {
    title: "Dashboard/AreaContent",
    component: AreaContent
} as Meta;


const Template = (args: IAreaContent) => <AreaContent {...args} />;
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
    areaName: "Test Name",
    classes: "",
    setLoaded: () => {},
    setViewName: () => {}
}
