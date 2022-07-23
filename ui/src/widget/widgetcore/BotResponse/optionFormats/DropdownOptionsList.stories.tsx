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
            intentDisplay: "intent1",
            intentId: "abc-1",
        },
        {
            intentDisplay: "intent2",
            intentId: "abc-2",
        },
        {
            intentDisplay: "intent3",
            intentId: "abc-123",
        },
        {
            intentDisplay: "intent4",
            intentId: "abc-123",
        },
    ],
};
