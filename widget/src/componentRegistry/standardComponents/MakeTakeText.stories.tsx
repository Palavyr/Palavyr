import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ConvoNode } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makeTakeText } from './MakeTakeText';
import { defaultContextProperties } from 'src/App';
import { Dispatch } from 'react';
import { SetStateAction } from 'react';
import { ContextProperties } from 'src/types';
const client = CreateClient("fake")

const args = {
    node: ConvoNode,
    nodeList: [ConvoNode],
    client: client,
    convoId: "abc",
    contextProperties: defaultContextProperties,
    setContextProperties: () => null as Dispatch<SetStateAction<ContextProperties>>
}

const TakeText = makeTakeText(args);

export default {
    title: "Standard/TakeText",
    component: TakeText,
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
            <TakeText {...args} />
        </div>
    </MemoryRouter>
);

export const Primary = Template.bind({});
Primary.args = {
    setSelectedOption: () => { },
}