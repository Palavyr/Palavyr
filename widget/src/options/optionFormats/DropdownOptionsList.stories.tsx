import * as React from "react";

import { DropdownListOptions, DropdownListProps } from "./DropdownOptionsList";
import { widgetPreferences } from "@test-data/widgetPreferences";
import { Meta, Story } from "@storybook/react";

export default {
    title: "WidgetOptions/DropDownListOptions",
    component: DropdownListOptions,
    argTypes: {},
} as Meta;

const Template = (args: DropdownListProps) => <DropdownListOptions {...args} />;

export const Primary: Story<DropdownListProps> = Template.bind({});
Primary.args = {
    setSelectedOption: () => null,
    options: [
        {
            areaDisplay: "Area 1",
            areaId: "abc-1",
        },
        {
            areaDisplay: "Area 2",
            areaId: "abc-2",
        },
        {
            areaDisplay: "Area 3",
            areaId: "abc-123",
        },
        {
            areaDisplay: "Area 4",
            areaId: "abc-123",
        },
    ],
};
