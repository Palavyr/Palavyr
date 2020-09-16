import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
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
