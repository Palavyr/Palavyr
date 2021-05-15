import * as React from "react";
import { App } from "./App";
import { Meta } from "@storybook/react";

import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { ConfigureMockClient } from "test/testUtils/ConfigureMockClient";
import { precheckResult } from "@test-data/preCheckResults";
import { widgetPreferences } from "@test-data/widgetPreferences";
import { areas } from "@test-data/areas";
import { convoA } from "@test-data/conversationNodes";

const fakeKey = "secret-key";
const isDemo = false;
const areaId = "abc123";
const routes = new PalavyrWidgetRepository(fakeKey).Routes;

const client = new ConfigureMockClient();
client.ConfigureGet(routes.precheck(fakeKey, isDemo), precheckResult);
client.ConfigureGet(routes.widgetPreferences(fakeKey), widgetPreferences);
client.ConfigureGet(routes.areas(fakeKey), areas);
client.ConfigureGet(routes.newConvo(fakeKey, areaId), convoA(areaId));

export default {
    title: "Main/App",
    component: App,
    argTypes: {},
} as Meta;

const Template = () => <App />;

export const Primary = Template.bind({});
Primary.args = {};
