import * as React from 'react';
import { Story, Meta } from '@storybook/react';

import { SaveOrCancel, ISaveOrCancel } from "./SaveOrCancel";

export default {
    title: "Common/SaveOrCancel",
    component: SaveOrCancel,
    argTypes: {}

} as Meta;

const Template = (args: ISaveOrCancel) => <SaveOrCancel {...args} />;

export const Primary = Template.bind({});
Primary.args = {}




