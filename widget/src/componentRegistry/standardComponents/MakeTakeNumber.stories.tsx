import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ConvoNode } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makeTakeNumber } from './MakeTakeNumber';

const client = CreateClient("fake")

const args = {
    node: ConvoNode,
    nodeList: [ConvoNode],
    client: client,
    convoId: "abc",

}

const TakeNumber = makeTakeNumber(args);

export default {
    title: "Standard/TakeNumber",
    component: TakeNumber,
    argTypes: {}
} as Meta;

const frame = {
    height: "500px",
    width: "320px",
    borderRadius: "9px",
    border: "0px",
    zIndex: 999
}

const Template = (args) => (
    <MemoryRouter>
        <div style={frame}>
            <TakeNumber {...args} />
        </div>
    </MemoryRouter>
);

export const Primary = Template.bind({});
Primary.args = {
    setSelectedOption: () => { },
}