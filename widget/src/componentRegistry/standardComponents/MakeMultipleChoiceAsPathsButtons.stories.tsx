import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ThreeNodes } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import {  makeMultipleChoiceAsPathButtons } from './MakeMultipeChoiceAsPathsButtons';

const client = CreateClient("fake")

const args = {
    node: ThreeNodes[0],
    nodeList: ThreeNodes,
    client: client,
    convoId: "abc",

}

const MultipleChoiceAsPaths = makeMultipleChoiceAsPathButtons(args);

export default {
    title: "Standard/MultipleChoiceAsPaths",
    component: MultipleChoiceAsPaths,
    argTypes: {}
} as Meta;

const frame = {
    height: "500px",
    width: "320px",
    borderRadius: "9px",
    border: "0px",
    zIndex: 999
}

const TemplatePaths = (args) => (
    <MemoryRouter>
        <div style={frame}>
            <MultipleChoiceAsPaths {...args} />
        </div>
    </MemoryRouter>
);

export const Paths = TemplatePaths.bind({});
Paths.args = {
    setSelectedOption: () => { },
}