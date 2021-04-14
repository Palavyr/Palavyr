import { Meta, Story } from "@storybook/react";
import React from "react";
import { widgetUrl } from "test/routes";
import { CollectDetailsForm, CollectDetailsFormProps } from "./CollectDetailsForm";

const fakeKey = "secret-key";
const isDemo = false;
const homeUrl = widgetUrl(fakeKey, isDemo);

export default {
    title: "Details/CollectDialog",
    component: CollectDetailsForm,
    argTypes: {},
    args: {
        chatStarted: false,
        setChatStarted: () => null,
    },
} as Meta;

const Template: Story<CollectDetailsFormProps> = args => <CollectDetailsForm {...args} />;

export const Primary = Template.bind({});
