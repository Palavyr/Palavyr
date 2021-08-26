import React from 'react'

import { Meta, Story } from "@storybook/react";
import { ConvoHeader, ConvoHeaderProps } from "./ConvoHeader";
import { widgetPreferences } from '@test-data/widgetPreferences';


export default {
    title: "Widget/Header",
    component: ConvoHeader,
    argTypes: {},
} as Meta;

const Template: Story<ConvoHeaderProps> = (args) => <ConvoHeader {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    title: "A good title",
    subtitle: "A great sub-title",
    titleAvatar: undefined,
    headerColor: widgetPreferences.headerColor,
    headerFontColor: widgetPreferences.headerFontColor
};