
import * as React from 'react';
import { Meta } from '@storybook/react/types-6-0';

import { CaroselOptions, ICaroselOptions } from "./CaroselOptions";
import { MemoryRouter } from 'react-router';

export default {
    title: "WidgetOptions/CaroselOptions",
    component: CaroselOptions,
    argTypes: {}

} as Meta;

const frame = {
    height: "500px",
    width: "320px",
    borderRadius: "9px",
    border: "0px",
    zIndex: 999
}

const Template = (args: ICaroselOptions) => (
    <MemoryRouter>
        <div style={frame}>
            <CaroselOptions {...args} />
        </div>
    </MemoryRouter>
);

export const Primary = Template.bind({});
Primary.args = {
    setSelectedOption: () => { },
    options: [
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-1"
        },
        {
            areaDisplay: "Other Title",
            areaId: "abc-2"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        },
        {
            areaDisplay: "Diplay Title",
            areaId: "abc-123"
        }
    ]
}




