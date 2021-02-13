import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ThreeNodes } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makeStartEndingSequence } from './MakeStartEndingSequence';
import { defaultContextProperties } from 'src/App';
import { Dispatch } from 'react';
import { SetStateAction } from 'react';
import { ContextProperties } from 'src/types';
const client = CreateClient("fake")

const args = {
    node: ThreeNodes[0],
    nodeList: ThreeNodes,
    client: client,
    convoId: "abc",
    contextProperties: defaultContextProperties,
    setContextProperties: () => null as Dispatch<SetStateAction<ContextProperties>>
}

const StartEndingSequence = makeStartEndingSequence(args);

export default {
    title: "EndingSequence/StartEndingSequence",
    component: StartEndingSequence,
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
            <StartEndingSequence {...args} />
        </div>
    </MemoryRouter>
);


export const Primary = TemplateContinue.bind({});
Primary.args = {
    setSelectedOption: () => { },
}
