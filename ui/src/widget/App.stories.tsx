import * as React from "react";
import { WidgetApp } from "./WidgetApp";
import { Meta } from "@storybook/react";

import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { ConfigureMockClient } from "widget/test/testUtils/ConfigureMockClient";
import { precheckResult } from "@test-data/preCheckResults";
import { testWidgetPreferences } from "@test-data/widgetPreferences";
import { intents } from "@test-data/intents";
import { convoA } from "@frontend/dashboard/content/designer/dummy_conversations";

const fakeKey = "secret-key";
const isDemo = false;
const intentId = "abc123";
const routes = new PalavyrWidgetRepository(fakeKey).Routes;

const client = new ConfigureMockClient();
client.ConfigureGet(routes.precheck(fakeKey, isDemo), precheckResult);
client.ConfigureGet(routes.widgetPreferences(fakeKey), testWidgetPreferences);
client.ConfigureGet(routes.intents(fakeKey), intents);
client.ConfigureGet(routes.newConversationHistory(fakeKey, true), convoA(intentId));

export default {
    title: "Main/WidgetApp",
    component: WidgetApp,
    argTypes: {},
} as Meta;

const Template = () => <WidgetApp />;

export const Primary = Template.bind({});
Primary.args = {};
