import * as React from "react";
import { App } from "./App";
import { Meta } from "@storybook/react";

import { WidgetClient } from "client/Client";
import { ConfigureMockClient } from "test/testUtils/ConfigureMockClient";
import { precheckResult } from "@test-data/preCheckResults";
import { widgetPreferences } from "@test-data/widgetPreferences";
import { areas } from "@test-data/areas";
import { convoA } from "@test-data/conversationNodes";

const fakeKey = "secret-key";
const isDemo = false;
const areaId = "abc123";
const routes = new WidgetClient(fakeKey).Routes;

const conf = new ConfigureMockClient();
conf.ConfigureGet(routes.precheck(fakeKey, isDemo), precheckResult);
conf.ConfigureGet(routes.widgetPreferences(fakeKey), widgetPreferences);
conf.ConfigureGet(routes.areas(fakeKey), areas);
conf.ConfigureGet(routes.newConvo(fakeKey, areaId), convoA(areaId));

export default {
    title: "Main/App",
    component: App,
    argTypes: {},
} as Meta;

const Template = () => <App />;

export const Primary = Template.bind({});
Primary.args = {};
