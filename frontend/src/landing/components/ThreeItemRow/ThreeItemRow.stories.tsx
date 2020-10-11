import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { ThreeItemRow, IThreeItemRow, ItemRowObject } from './ThreeItemRow';


export default {
    title: "Landing/ThreeItemRow",
    component: ThreeItemRow
} as Meta;


const Template = (args: IThreeItemRow) => <ThreeItemRow {...args} />;

const data: Array<ItemRowObject> = [
    {
        title: "Engage potential clients",
        text: "Collect all of the information that you need to engage, sort, and secure a potential client.",
        type: "pencil",
    },
    {
        title: "Direct Fee Estimate",
        text: "Deliver a competitive fee estimate and convince your prospective clients to sign.",
        type: "calculator",
    },
    {
        title: "Persuade clients to sign",
        text: "Anticipate your client's needs ahead of time and offer a competetive fee estimate.",
        type: "check",
    }
]


export const Primary = Template.bind({});
Primary.args = {
    listOfThree: data,
}

