import * as React from "react";
import { Meta } from "@storybook/react/types-6-0";
import { ConvoTree } from "./ConvoTree";

export default {
    title: "Dashboard/Conversation/ConvoTree",
    component: ConvoTree,
} as Meta;

const Template = () => <ConvoTree />;
export const Primary = Template.bind({});
Primary.args = {
    areaIdentifier: "abc-123",
    treeName: "Tree Name",
};
