import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ThreeNodes } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makePhoneNumber } from './MakePhoneNumber';

const client = CreateClient("fake")

const args = {
    node: ThreeNodes[0],
    nodeList: ThreeNodes,
    client: client,
    convoId: "abc",
    convoContext: {}
}

const PhoneNumber = makePhoneNumber(args);

export default {
    title: "EndingSequence/PhoneNumber",
    component: PhoneNumber,
    argTypes: {}
} as Meta;

const frame = {
    height: "500px",
    width: "320px",
    borderRadius: "9px",
    border: "0px",
    zIndex: 999
}

const TemplateContinue = (args) => (
    <MemoryRouter>
        <div style={frame}>
            <PhoneNumber {...args} />
        </div>
    </MemoryRouter>
);


export const Primary = TemplateContinue.bind({});
Primary.args = {
    setSelectedOption: () => { },
}
