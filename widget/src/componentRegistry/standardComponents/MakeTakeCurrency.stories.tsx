import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ConvoNode } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makeTakeCurrency } from './MakeTakeCurrency';

const client = CreateClient("fake")

const args = {
    node: ConvoNode,
    nodeList: [ConvoNode],
    client: client,
    convoId: "abc",
    convoContext: {}
}

const TakeCurrency = makeTakeCurrency(args);

export default {
    title: "Standard/TakeCurrency",
    component: TakeCurrency,
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
            <TakeCurrency {...args} />
        </div>
    </MemoryRouter>
);

export const Primary = Template.bind({});
Primary.args = {
    setSelectedOption: () => { },
}