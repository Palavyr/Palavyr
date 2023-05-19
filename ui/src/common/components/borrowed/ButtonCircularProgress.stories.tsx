import * as React from 'react';
import { Story, Meta } from '@storybook/react';
import { ButtonCircularProgress, IButtonCircularProgress } from './ButtonCircularProgress';


export default {
    title: "Common/Borrowed/ButtonCircularProgress",
    component: ButtonCircularProgress
} as Meta;


const Template = (args: IButtonCircularProgress) => <ButtonCircularProgress {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    size: 34,
}
