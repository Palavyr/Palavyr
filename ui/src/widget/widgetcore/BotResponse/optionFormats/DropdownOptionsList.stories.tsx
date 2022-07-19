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
            IntentDisplay: "Area 1",
            IntentId: "abc-1",
        },
        {
            IntentDisplay: "Area 2",
            IntentId: "abc-2",
        },
        {
            IntentDisplay: "Area 3",
            IntentId: "abc-123",
        },
        {
            IntentDisplay: "Area 4",
            IntentId: "abc-123",
        },
    ],
};
