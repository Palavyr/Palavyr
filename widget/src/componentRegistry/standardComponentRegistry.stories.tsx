import React from "react";
import { Meta, Story } from "@storybook/react";
import { StandardComponents } from "./standardComponentRegistry";
import { ConvoTableRow, IProgressTheChat } from "@Palavyr-Types";
import { ConfigureMockClient } from "test/testUtils/ConfigureMockClient";
import { WidgetClient } from "client/Client";
import { convoA } from "@test-data/conversationNodes";
import { MessageWrapper } from "componentRegistry/MessageWrapper";
import { widgetPreferences } from "@test-data/widgetPreferences";

const registry = new StandardComponents();

const node: ConvoTableRow = convoA("246")[0];
const nodeList: Array<ConvoTableRow> = [node];

const fakeKey = "secret-key";
const conf = new ConfigureMockClient();
const client = new WidgetClient(fakeKey);
const convoId = "1234";

export default {
    title: "Widget/ChatComponents",
    component: registry.makeProvideInfo({ node, nodeList, client, convoId }),
    argTypes: {},
    decorators: [
        Story => (
            <div style={{ border: "1px solid lightgray", position: "static", width: "350px" }}>
                <MessageWrapper customPreferences={widgetPreferences}>
                    <Story />
                </MessageWrapper>
            </div>
        ),
    ],
} as Meta;

const ProvideInfo = registry.makeProvideInfo({ node, nodeList, client, convoId });
export const MakeProvideInfo: Story = (() => <ProvideInfo />).bind({});

const MultipleChoiceContinue = registry.makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId });
export const MakeMultipleChoiceContinueButtons: Story = (() => <MultipleChoiceContinue />).bind({});

const MultipleChoiceAsPathButtons = registry.makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId });
export const MakeMultipleChoiceAsPathButtons: Story = (() => <MultipleChoiceAsPathButtons />).bind({});

const TakeNumber = registry.makeTakeNumber({ node, nodeList, client, convoId });
export const MakeTakeNumber: Story = (() => <TakeNumber />).bind({});

const TakeCurrency = registry.makeTakeCurrency({ node, nodeList, client, convoId });
export const MakeTakeCurrency: Story = (() => <TakeCurrency />).bind({});

const TakeText = registry.makeTakeText({ node, nodeList, client, convoId });
export const MakeTakeText: Story = (() => <TakeText />).bind({});

const TakeNumberIndividuals = registry.makeTakeNumberIndividuals({ node, nodeList, client, convoId });
export const MakeTakeNumberIndividuals: Story = (() => <TakeNumberIndividuals />).bind({});

const SendEmail = registry.makeSendEmail({ node, nodeList, client, convoId });
export const MakeSendEmail: Story = (() => <SendEmail />).bind({});

const Restart = registry.makeRestart({ node, nodeList, client, convoId });
export const MakeRestart: Story = (() => <Restart />).bind({});

const SendEmailFailedFirstAttempt = registry.makeSendEmailFailedFirstAttempt({ node, nodeList, client, convoId });
export const MakeSendEmailFailedFirstAttempt: Story = (() => <SendEmailFailedFirstAttempt />).bind({});

const SendFallbackEmail = registry.makeSendFallbackEmail({ node, nodeList, client, convoId });
export const MakeSendFallbackEmail: Story = (() => <SendFallbackEmail />).bind({});

