import { Registry, ConvoTableRow, ConversationUpdate } from "../types";
import { makeTooComplicated } from "./standardComponents/TooComplicated";
import { renderCustomComponent, addUserMessage } from "src/widgetCore/store/dispatcher";
import { IClient } from "../client/Client";
import { makeName } from "./endingSequenceComponents/MakeName";
import { makeEmail } from "./endingSequenceComponents/MakeEmail";
import { makePhoneNumber } from "./endingSequenceComponents/MakePhoneNumber";
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

export interface IProgressTheChat {
    node: ConvoTableRow;
    nodeList: Array<ConvoTableRow>;
    client: IClient;
    convoId: string;
    convoContext: any;
}

export const responseAction = (node: ConvoTableRow, child: ConvoTableRow, nodeList: Array<ConvoTableRow>, client: IClient, convoId: string, response: string, convoContext: any) => {
    if (response) {
        if (child.optionPath !== null && child.optionPath !== "" && child.optionPath !== "Continue") {
            addUserMessage(child.optionPath);
        } else {
            addUserMessage(response)
        }
    }

    var updatePayload: ConversationUpdate = {
        ConversationId: convoId,
        Prompt: node.text,
        UserResponse: response,
        NodeId: node.nodeId,
        NodeCritical: node.isCritical,
        NodeType: node.nodeType,
    }

    client.Widget.Access.postUpdateAsync(updatePayload); // no need to await for this
    renderNextComponent(child, nodeList, client, convoId, convoContext); // convoId should come from redux store in the future
}


export const renderNextComponent = (node: ConvoTableRow, nodeList: Array<ConvoTableRow>, client: IClient, convoId: string, convoContext: any) => {

    //TODO: make this impossible by geting the configuration right
    if (node.nodeType === "" || node.nodeType === null || node.nodeChildrenString === "" || node.nodeChildrenString === null) {
        return renderCustomComponent(dummyFailComponent, {}, false)
    }
    var makeNextComponent = ComponentRegistry[node.nodeType.split("-")[0]]; // This is a bit fragile

    var nextComponent = makeNextComponent({node, nodeList, client, convoId, convoContext});
    return renderCustomComponent(nextComponent, {}, false);
}

export enum NodeTypes {
    // standard types
    YesNo = "YesNo",
    TooComplicated = "TooComplicated",
    YesNoNotSure = "YesNoNotSure",
    YesNotSureCombined = "YesNotSureCombined",
    NoNotSureCombined = "NoNotSureCombined",
    TakeText = "TakeText",
    Info = "Info",
    MultipleChoiceAsPath = "MultipleChoiceAsPath",
    MultipleChoiceContinue = "MultipleChoiceContinue",

    // under consideration
    HowMany = "HowMany",
    HowMuch = "HowMuch",

    // HIDDEN types frmo dashboard (currently) closing sequence
    Name = "Name",
    Email = "Email",
    Phone = "Phone",

    // we currently only support one ending sequence (see above)
    EndingSequence = "EndingSequence",

    // Dynamic Table Type Nodes
    SelectOneFlat = "SelectOneFlat",
    PercentOfThreshold = "PercentOfThreshold",

    SendEmail = "SendEmail",
    Restart = "Restart"
}

export const ComponentRegistry: Registry = {
    [NodeTypes.YesNo]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.YesNoNotSure]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.YesNotSureCombined]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.NoNotSureCombined]: makeMultipleChoiceAsPathButtons,
    [NodeTypes.MultipleChoiceAsPath]: makeMultipleChoiceAsPathButtons,

    [NodeTypes.MultipleChoiceContinue]: makeMultipleChoiceContinueButtons,

    [NodeTypes.TakeText]: makeTakeText,
    [NodeTypes.Info]: makeProvideInfo,
    [NodeTypes.HowMany]: makeTakeNumber,
    [NodeTypes.HowMuch]: makeTakeNumber,

    [NodeTypes.TooComplicated]: makeTooComplicated,

    [NodeTypes.EndingSequence]: makeStartEndingSequence,
    [NodeTypes.Name]: makeName,
    [NodeTypes.Email]: makeEmail,
    [NodeTypes.Phone]: makePhoneNumber,

    // Dynamic Types
    [NodeTypes.SelectOneFlat]: makeSelectOneFlat, // could be replaced with makeMultiple choice continue,
    [NodeTypes.PercentOfThreshold]: makePercentOfThreshold,

    [NodeTypes.SendEmail]: makeSendEmail,
    [NodeTypes.Restart]: makeRestart
}

export const ConvoContextProperties = {
    DynamicResponses: "DynamicResponses",
    Name: "Name",
    EmailAddress: "EmailAddress",
    PhoneNumber: "PhoneNumber",
    KeyValues: "KeyValues", // values reported at head of PDF response
    Region: "Region"
}
