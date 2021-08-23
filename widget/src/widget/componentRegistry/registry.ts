import { Registry } from "../../globalTypes";
import { StandardComponents } from "./standardComponentRegistry";

const components = new StandardComponents();

export const ComponentRegistry: Registry = {
    YesNo: components.makeMultipleChoiceAsPathButtons,
    YesNoNotSure: components.makeMultipleChoiceAsPathButtons,
    YesNotSureCombined: components.makeMultipleChoiceAsPathButtons,
    NoNotSureCombined: components.makeMultipleChoiceAsPathButtons,
    MultipleChoiceAsPath: components.makeMultipleChoiceAsPathButtons,
    MultipleChoiceContinue: components.makeMultipleChoiceContinueButtons,

    TakeCurrency: components.makeTakeCurrency,
    TakeText: components.makeTakeText,
    ProvideInfo: components.makeProvideInfo,
    TakeNumber: components.makeTakeNumber,
    TakeNumberIndividuals: components.makeTakeNumberIndividuals,
    ShowImage: components.makeShowImage,

    TooComplicated: components.makeProvideInfo,
    SendResponse: components.makeProvideInfo,
    SendEmail: components.makeSendEmail,
    FirstEmailFailed: components.makeProvideInfo,
    SendTooComplicatedEmail: components.makeSendFallbackEmail,
    Restart: components.makeRestart,
    EndWithoutEmail: components.makeEndWithoutEmail,

    Selection: components.makeSelectOptions,
    // Selection: components.makeCollectDetails
    // CollectDetails: components.makeCollectDetails,
};

export const ConvoContextProperties = {
    dynamicResponses: "dynamicResponses", // dynamic table responses
    name: "name",
    emailAddress: "emailAddress",
    phoneNumber: "phoneNumber",
    keyValues: "keyValues", // values reported at head of PDF response
    region: "region",
    numIndividuals: "numIndividuals",
};
