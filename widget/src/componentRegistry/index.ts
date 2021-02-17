import { Registry, ConvoTableRow, ConversationUpdate } from "../types";
import { renderCustomComponent, addUserMessage, toggleMsgLoader, addKeyValue } from "src/widgetCore/store/dispatcher";
import { IClient } from "../client/Client";
import { dummyFailComponent } from "./DummyComponentDev";
import { makeProvideInfo } from "./standardComponents/MakeProvideInfo";
import { makeStartEndingSequence } from "./endingSequenceComponents/MakeStartEndingSequence";
import { makeSelectOneFlat } from "./dynamicTableComponents/MakeSelectOneFlat";
import { makeSendEmail } from "./endingSequenceComponents/MakeSendEmail";
import { makeRestart } from "./endingSequenceComponents/MakeRestart";
import { makeTakeText } from "./standardComponents/MakeTakeText";
import { makeTakeNumber } from "./standardComponents/MakeTakeNumber";
import { makeMultipleChoiceAsPathButtons } from "./standardComponents/MakeMultipeChoiceAsPathsButtons";
import { makeMultipleChoiceContinueButtons } from "./standardComponents/MakeMultipleChoiceContinueButtons";
import { makeTakeCurrency } from "./standardComponents/MakeTakeCurrency";
import { makePercentOfThreshold } from "./dynamicTableComponents/MakePercentOfThreshold";
import { makeTooComplicated } from "./standardComponents/MakeTooComplicated";
import { random } from "./random";
import { makeSendFallbackEmail } from "./endingSequenceComponents/MakeSendFallbackEmail";

export interface IProgressTheChat {
    node: ConvoTableRow;
    nodeList: Array<ConvoTableRow>;
    client: IClient;
    convoId: string;
}

export const responseAction = (
    node: ConvoTableRow,
    child: ConvoTableRow,
    nodeList: Array<ConvoTableRow>,
    client: IClient,
    convoId: string,
    response: string,
    callback: () => void = null
) => {

    if (response) {
        if (node.isCritical) {
            addKeyValue({ [node.text]: response.toString() })
        }

        if (child.optionPath !== null && child.optionPath !== "" && child.optionPath !== "Continue") {
            addUserMessage(child.optionPath);
        } else {
            addUserMessage(response);
        }
    }
    toggleMsgLoader();

    var updatePayload: ConversationUpdate = {
        ConversationId: convoId,
        Prompt: node.text,
        UserResponse: response,
        NodeId: node.nodeId,
        NodeCritical: node.isCritical,
        NodeType: node.nodeType,
    };

    client.Widget.Access.postUpdateAsync(updatePayload); // no need to await for this
    setTimeout(() => {
        if (callback) callback();
        renderNextComponent(child, nodeList, client, convoId); // convoId should come from redux store in the future
        toggleMsgLoader();
    }, random(1000, 3000, undefined));
};

export const renderNextComponent = (
    node: ConvoTableRow,
    nodeList: Array<ConvoTableRow>,
    client: IClient,
    convoId: string,
) => {
    //TODO: make this impossible by geting the configuration right
    if (node.nodeType === "" || node.nodeType === null || node.nodeChildrenString === "" || node.nodeChildrenString === null) {
        return renderCustomComponent(dummyFailComponent, {}, false);
    }
    var makeNextComponent = ComponentRegistry[node.nodeType.split("-")[0]]; // This is a bit fragile

    var nextComponent = makeNextComponent({ node, nodeList, client, convoId });
    return renderCustomComponent(nextComponent, {}, false);
};

export enum NodeTypes {
    // standard types
    YesNo = "YesNo",
    TooComplicated = "TooComplicated",
    YesNoNotSure = "YesNoNotSure",
    YesNotSureCombined = "YesNotSureCombined",
    NoNotSureCombined = "NoNotSureCombined",
    TakeText = "TakeText",
    TakeNumber = "TakeNumber",
    TakeCurrency = "TakeCurrency",
    TakeNumberIndividuals = "TakeNumberIndividuals",
    ProvideInfo = "ProvideInfo",
    MultipleChoiceAsPath = "MultipleChoiceAsPath",
    MultipleChoiceContinue = "MultipleChoiceContinue",

    // under consideration
    HowMany = "HowMany",
    HowMuch = "HowMuch",

    // we currently only support one ending sequence (see above)
    EndingSequence = "EndingSequence",

    // Dynamic Table Type Nodes
    SelectOneFlat = "SelectOneFlat",
    PercentOfThreshold = "PercentOfThreshold",

    SendEmail = "SendEmail",
    FirstEmailFailed = "EmailSendFailedFirstAttempt",
    SendTooComplicatedEmail = "SendTooComplicatedEmail",
    SendResponse = "SendResponse",
    Restart = "Restart",
}

export const ComponentRegistry: Registry = {
    [NodeTypes.YesNo]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.YesNoNotSure]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.YesNotSureCombined]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.NoNotSureCombined]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.MultipleChoiceAsPath]: makeMultipleChoiceAsPathButtons,

    [NodeTypes.MultipleChoiceContinue]: makeMultipleChoiceContinueButtons,

    [NodeTypes.TakeCurrency]: makeTakeCurrency,
    [NodeTypes.TakeText]: makeTakeText,
    [NodeTypes.ProvideInfo]: makeProvideInfo,
    [NodeTypes.HowMany]: makeTakeNumber,
    [NodeTypes.HowMuch]: makeTakeCurrency,
    [NodeTypes.TakeNumber]: makeTakeNumber,
    [NodeTypes.TakeNumberIndividuals]: makeTakeNumber,
    [NodeTypes.TooComplicated]: makeTooComplicated,

    [NodeTypes.EndingSequence]: makeStartEndingSequence,

    // Dynamic Types
    [NodeTypes.SelectOneFlat]: makeSelectOneFlat, // could be replaced with makeMultiple choice continue,
    [NodeTypes.PercentOfThreshold]: makePercentOfThreshold,

    // Ending sequence nodes
    [NodeTypes.SendResponse]: makeProvideInfo,
    [NodeTypes.SendEmail]: makeSendEmail,
    [NodeTypes.FirstEmailFailed]: makeProvideInfo,
    [NodeTypes.SendTooComplicatedEmail]: makeSendFallbackEmail,

    [NodeTypes.Restart]: makeRestart,
};

export const ConvoContextProperties = {
    dynamicResponses: "dynamicResponses", // dynamic table responses
    name: "name",
    emailAddress: "emailAddress",
    phoneNumber: "phoneNumber",
    keyValues: "keyValues", // values reported at head of PDF response
    region: "region",
};
