import * as React from 'react';
import { Story, Meta } from '@storybook/react';

import { SettingsGridRowText, ISettingsGridRow } from "./SettingsGridRowText";

export default {
    title: "Common/SettingsGridRowText",
    component: SettingsGridRowText
} as Meta;

const Template = (args: ISettingsGridRow) => <SettingsGridRowText {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    title: 'Intro Statement',
    details: 'Use this section to create an introduction statement for your estimate.You can make it clear that fees are estimate only, or provide context for your client to understand their estimate.',
    placeholder: "A test Placeholder",
    onClick: (() => {}),
    clearVal: true
}

export const WithCurrentValue = Template.bind({});
WithCurrentValue.args = {
    ...Primary.args,
    currentValue: "The Current Value"
}
