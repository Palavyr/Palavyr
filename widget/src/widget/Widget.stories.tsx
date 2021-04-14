import React from "react";

import { widgetPreferences } from "@test-data/widgetPreferences";
import { Meta, Story } from "@storybook/react";
import { Widget, WidgetProps } from "./Widget";
import { getSelectedOption, options } from "@test-data/options";
import { WidgetClient } from "client/Client";
import { ConfigureMockClient } from "test/testUtils/ConfigureMockClient";
import { newConversation } from "@test-data/newConversation";

const fakeKey = "secret-key";
const areaId = "abc123";
const routes = new WidgetClient(fakeKey).Routes;

const conf = new ConfigureMockClient();
conf.ConfigureGet(routes.newConvo(fakeKey, areaId), newConversation(areaId));

export default {
    title: "Widget/Widget",
    component: Widget,
    argTypes: {},
} as Meta;

const Template = (args: WidgetProps) => <Widget {...args} />;

export const Primary: Story<WidgetProps> = Template.bind({});
Primary.args = {
    option: getSelectedOption(areaId),
    preferences: widgetPreferences,
};
