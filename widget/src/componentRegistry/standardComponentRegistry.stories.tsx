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
    title: "ChatComponents",
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
export const MakeProvideInfo: Story = () => <ProvideInfo />;

const MultipleChoiceContinue = registry.makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId });
export const MakeMultipleChoiceContinueButtons: Story = () => <MultipleChoiceContinue />;

const MultipleChoiceAsPathButtons = registry.makeMultipleChoiceContinueButtons({ node, nodeList, client, convoId });
export const MakeMultipleChoiceAsPathButtons: Story = () => <MultipleChoiceAsPathButtons />;

const TakeNumber = registry.makeTakeNumber({ node, nodeList, client, convoId });
export const MakeTakeNumber: Story = () => <TakeNumber />;

const TakeCurrency = registry.makeTakeCurrency({ node, nodeList, client, convoId });
export const MakeTakeCurrency: Story = () => <TakeCurrency />;

const TakeText = registry.makeTakeText({ node, nodeList, client, convoId });
export const MakeTakeText: Story = () => <TakeText />;

const TakeNumberIndividuals = registry.makeTakeNumberIndividuals({ node, nodeList, client, convoId });
export const MakeTakeNumberIndividuals: Story = () => <TakeNumberIndividuals />;

const SendEmail = registry.makeSendEmail({ node, nodeList, client, convoId });
export const MakeSendEmail: Story = () => <SendEmail />;

const Restart = registry.makeRestart({ node, nodeList, client, convoId });
export const MakeRestart: Story = () => <Restart />;

const SendEmailFailedFirstAttempt = registry.makeSendEmailFailedFirstAttempt({ node, nodeList, client, convoId });
export const MakeSendEmailFailedFirstAttempt: Story = () => <SendEmailFailedFirstAttempt />;

const SendFallbackEmail = registry.makeSendFallbackEmail({ node, nodeList, client, convoId });
export const MakeSendFallbackEmail: Story = () => <SendFallbackEmail />;

const allComponents = [
    ProvideInfo,
    MultipleChoiceContinue,
    MultipleChoiceAsPathButtons,
    TakeNumber,
    TakeCurrency,
    TakeText,
    TakeNumberIndividuals,
    SendEmail,
    Restart,
    SendEmailFailedFirstAttempt,
    SendFallbackEmail
];

export const AllComponents: Story = () => {
    return (
        <div>
            {allComponents.map(Component => (
                <>
                    <Component />
                    <div style={{ height: "1rem", backgroundColor: "black" }}></div>
                </>
            ))}
        </div>
    );
};
