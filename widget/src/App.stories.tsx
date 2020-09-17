import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';
import { App } from "./App";
import {MemoryRouter } from 'react-router';
import axios from "axios";
import MockAdapter from "axios-mock-adapter";
import CreateClient from './client/Client';
import { AreaTable } from './types';

const fakeKey = "secret-key";
const client = CreateClient(fakeKey)

const fakeAreaTables: Array<AreaTable> = [
    {
        areaIdentifier: "abc-123",
        areaDisplayTitle: "Test Display Title",
    }
]

var mock = new MockAdapter(axios);
mock.onGet(`api/widget/${fakeKey}/areas`).reply(200, fakeAreaTables);


export default {
    title: "Main/App",
    component: App,
    argTypes: {}

} as Meta;

const Template = () => <MemoryRouter><App /></MemoryRouter>;

export const Primary = Template.bind({});
Primary.args = {}



