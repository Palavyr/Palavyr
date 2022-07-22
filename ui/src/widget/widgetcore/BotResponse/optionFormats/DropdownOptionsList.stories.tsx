import * as React from "react";

import { DropdownListOptions, DropdownListProps } from "./DropdownOptionsList";
import { Meta, Story } from "@storybook/react";

export default {
    title: "WidgetOptions/DropDownListOptions",
    component: DropdownListOptions,
    argTypes: {},
} as Meta;

const Template = (args: DropdownListProps) => <DropdownListOptions {...args} />;

export const Primary: Story<DropdownListProps> = Template.bind({});
Primary.args = {
    options: [
        {
            IntentDisplay: "intent1",
            IntentId: "abc-1",
        },
        {
            IntentDisplay: "intent2",
            IntentId: "abc-2",
        },
        {
            IntentDisplay: "intent3",
            IntentId: "abc-123",
        },
        {
            IntentDisplay: "intent4",
            IntentId: "abc-123",
        },
    ],
};
