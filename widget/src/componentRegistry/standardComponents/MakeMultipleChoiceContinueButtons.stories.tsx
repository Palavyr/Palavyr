import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { MemoryRouter } from 'react-router';
import { ThreeNodes } from '../../test/dummyData/dummyNodes';
import CreateClient from '../../client/Client';
import { makeMultipleChoiceContinueButtons } from './MakeMultipleChoiceContinueButtons';

const client = CreateClient("fake")

const args = {
    node: ThreeNodes[0],
    nodeList: ThreeNodes,
    client: client,
    convoId: "abc",

}

const MultipleChoiceContinue = makeMultipleChoiceContinueButtons(args);

export default {
    title: "Standard/MultipleChoiceContinue",
    component: MultipleChoiceContinue,
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
            <MultipleChoiceContinue {...args} />
        </div>
    </MemoryRouter>
);


export const Primary = TemplateContinue.bind({});
Primary.args = {
    setSelectedOption: () => { },
}
