import * as React from "react";
import { Meta } from "@storybook/react";
import { LoginPage } from "./LoginPage";
import { MemoryRouter } from "react-router-dom";

export default {
    title: "Landing/LandingPage",
    component: LoginPage,
} as Meta;

const Template = () => (
    <MemoryRouter>
        <LoginPage />
    </MemoryRouter>
);
export const Primary = Template.bind({});
Primary.args = {};
