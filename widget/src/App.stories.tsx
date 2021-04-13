import * as React from 'react';
import { App } from "./App";
import { Meta } from '@storybook/react';
import {MemoryRouter } from 'react-router';
import axios from "axios";
import MockAdapter from "axios-mock-adapter";
import { WidgetClient } from 'client/Client';
import { AreaTable } from '@Palavyr-Types';

const fakeKey = "secret-key";
const client = new WidgetClient(fakeKey)

const fakeAreaTables: Array<AreaTable> = [
    {
        areaIdentifier: "abc-123",
        areaDisplayTitle: "Test Display Title",
    }
]

var mock = new MockAdapter(axios);
mock.onGet(`api/widget/areas?key=${fakeKey}`).reply(200, fakeAreaTables);

export default {
    title: "Main/App",
    component: App,
    argTypes: {}

} as Meta;

const Template = () => <MemoryRouter><App /></MemoryRouter>;

export const Primary = Template.bind({});
Primary.args = {}



