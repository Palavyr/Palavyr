import { NodeTypeCodeEnum, UnitIdEnum } from "@Palavyr-Types";

export const convoA = (intentId: string) => [
    {
        id: 0,
        intentId: intentId,
        nodeId: "ba765849-ef8c-4e5e-9d64-9203d7cdb541",
        text: "Hello, this is a test. A really good test because it has a  long text. A realllllllly long text. To check stylings!",
        nodeType: "ProvideInfo",
        nodeChildrenString: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
        isRoot: true,
        isCritical: false,
        optionPath: "",
        valueOptions: "Placeholder",
        nodeComponentType: "ProvideInfo",
        isPricingStrategyTableNode: false,
        pricingStrategyType: "",
        resolveOrder: 0,
        fileAssetResource: null,
        nodeTypeCodeEnum: NodeTypeCodeEnum.I,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,
        nodeId: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
        nodeType: "ProvideInfo",
        pricingStrategyType: "",
        text: "You are a great person.",
        isRoot: false,
        optionPath: "Continue",
        isCritical: false,
        valueOptions: "Placeholder",

        isPricingStrategyTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: 0,
        nodeChildrenString: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
        fileAssetResource: null,
        nodeTypeCodeEnum: NodeTypeCodeEnum.I,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
        nodeType: "MultipleChoiceAsPath",
        pricingStrategyType: "",

        text: "How are you feeling today? Hello, this is a test. A really good test because it has a  long text. A realllllllly long text. To check stylings!",
        isRoot: false,
        optionPath: "Continue",
        isCritical: false,
        valueOptions: "Good|peg|Bad",

        isPricingStrategyTableNode: false,
        nodeComponentType: "MultipleChoiceAsPath",

        resolveOrder: 0,
        nodeChildrenString: "bee2c623-98d5-4831-bb6d-4a94a1fc7205,fc237abf-fdc5-45d4-a145-beff96c6bcf2",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "fc237abf-fdc5-45d4-a145-beff96c6bcf2",
        nodeType: "SendResponse",
        pricingStrategyType: "",

        text: "We all have bad days. You'll make it through. Just be strong.",
        isRoot: false,
        optionPath: "Bad",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendResponse",

        resolveOrder: 0,
        nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "bee2c623-98d5-4831-bb6d-4a94a1fc7205",
        nodeType: "SendResponse",
        pricingStrategyType: "",

        text: "Good on you. Now take that light and spread it around the world.",
        isRoot: false,
        optionPath: "Good",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendResponse",

        resolveOrder: 0,
        nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "cd16584a-6b06-412a-881b-7402c2b9986e",
        nodeType: "YesNo",
        pricingStrategyType: "",

        text: "We'd like to send you an email with some information regarding your enquiry. Would that be okay?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "No|peg|Yes",

        isPricingStrategyTableNode: false,
        nodeComponentType: "YesNo",

        resolveOrder: 0,
        nodeChildrenString: "5e95a84a-de6b-4453-8ed9-2c7ebb2e8b1c,4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "5e95a84a-de6b-4453-8ed9-2c7ebb2e8b1c",
        nodeType: "SendEmail",
        pricingStrategyType: "",

        text: "Wait just a moment while I send you a confirmation email with some information",
        isRoot: false,
        optionPath: "Yes",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: 0,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        nodeType: "Restart",
        pricingStrategyType: "",

        text: "Thanks for your time. If you'd like to start again, click the button below.",
        isRoot: false,
        optionPath: "No",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isPricingStrategyTableNode: false,
        nodeComponentType: "Restart",

        resolveOrder: 0,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "EmailSuccessfulNodeId",
        nodeType: "ProvideInfo",
        pricingStrategyType: "",

        text: "We've sent through an email to the address you provided. Sometimes these emails are picked up as spam, so if you don't see it, be sure to check your spam folder.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: 0,
        nodeChildrenString: "Restart",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "Restart",
        nodeType: "Restart",
        pricingStrategyType: "",

        text: "Thanks for your time. If you'd like to start again, click the button below.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isPricingStrategyTableNode: false,
        nodeComponentType: "Restart",

        resolveOrder: 0,
        nodeChildrenString: "Terminate",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "EmailFailedNodeId",
        nodeType: "EmailSendFailedFirstAttempt",
        pricingStrategyType: "",

        text: "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: 0,
        nodeChildrenString: "11640f68-7551-41cb-b4f3-f414c94c8c47",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "11640f68-7551-41cb-b4f3-f414c94c8c47",
        nodeType: "SendEmail",
        pricingStrategyType: "",

        text: "Wait just a moment while I try that again.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: 0,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "FallbackEmailFailedNodeId",
        nodeType: "EmailSendFailedFirstAttempt",
        pricingStrategyType: "",

        text: "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: 0,
        nodeChildrenString: "e8d6d29c-62e3-46d2-aa8b-75aed7125500",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "e8d6d29c-62e3-46d2-aa8b-75aed7125500",
        nodeType: "SendEmail",
        pricingStrategyType: "",

        text: "Wait just a moment while I try that again.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: 0,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "4c452b9e-d471-425a-80fc-15f5bb9ad04a",
        nodeType: "YesNo",
        pricingStrategyType: "",

        text: "We'd like to send you a follow-up email with some general information regarding your enquiry. Would that be okay?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "No|peg|Yes",

        isPricingStrategyTableNode: false,
        nodeComponentType: "YesNo",

        resolveOrder: 0,
        nodeChildrenString: "bf068b90-f087-48cd-951a-415f733ef42f,4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        fileAssetResource: null,
        unitId: "currency",
    },
    {
        id: 0,
        intentId: intentId,

        nodeId: "bf068b90-f087-48cd-951a-415f733ef42f",
        nodeType: "SendTooComplicatedEmail",
        pricingStrategyType: "",

        text: "Wait just a moment while I send an email.",
        isRoot: false,
        optionPath: "Yes",
        isCritical: false,
        valueOptions: "",

        isPricingStrategyTableNode: false,
        nodeComponentType: "SendTooComplicatedEmail",

        resolveOrder: 0,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
        unitId: "currency",
    },
];

export const shortStaticConvoSequence = (intentId: string) => {
    return [
        {
            id: 0,
            intentId: intentId,

            nodeId: "ba765849-ef8c-4e5e-9d64-9203d7cdb541",
            nodeType: "ProvideInfo",
            pricingStrategyType: "",

            text: "Hello, this is a test.",
            isRoot: true,
            optionPath: "",
            isCritical: false,
            valueOptions: "Placeholder",

            isPricingStrategyTableNode: false,
            nodeComponentType: "ProvideInfo",

            resolveOrder: 0,
            nodeChildrenString: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
            fileAssetResource: null,

            unitId: "currency",
        },
        {
            id: 0,
            intentId: intentId,

            nodeId: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
            nodeType: "ProvideInfo",
            pricingStrategyType: "",

            text: "You are a great person.",
            isRoot: false,
            optionPath: "Continue",
            isCritical: false,
            valueOptions: "Placeholder",

            isPricingStrategyTableNode: false,
            nodeComponentType: "ProvideInfo",

            resolveOrder: 0,
            nodeChildrenString: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
            fileAssetResource: null,

            unitId: "currency",
        },
        {
            id: 0,
            intentId: intentId,

            nodeId: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
            nodeType: "MultipleChoiceAsPath",
            pricingStrategyType: "",

            text: "How are you feeling today?",
            isRoot: false,
            optionPath: "Continue",
            isCritical: false,
            valueOptions: "Good|peg|Bad",

            isPricingStrategyTableNode: false,
            nodeComponentType: "MultipleChoiceAsPath",

            resolveOrder: 0,
            nodeChildrenString: "bee2c623-98d5-4831-bb6d-4a94a1fc7205,fc237abf-fdc5-45d4-a145-beff96c6bcf2",
            fileAssetResource: null,

            unitId: "currency",
        },
        {
            userResponse: "Good",
            fileAssetResource: null,

            unitId: "currency",
        },
        {
            id: 0,
            intentId: intentId,

            nodeId: "bee2c623-98d5-4831-bb6d-4a94a1fc7205",
            nodeType: "SendResponse",
            pricingStrategyType: "",

            text: "Good on you. Now take that light and spread it around the world.",
            isRoot: false,
            optionPath: "Good",
            isCritical: false,
            valueOptions: "",

            isTerminalType: true,

            isPricingStrategyTableNode: false,
            nodeComponentType: "SendResponse",

            resolveOrder: 0,
            nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
            fileAssetResource: null,

            unitId: "currency",
        },
    ];
};
