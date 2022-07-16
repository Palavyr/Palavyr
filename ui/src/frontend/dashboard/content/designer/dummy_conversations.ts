export const convoA = (areaId: string) => [
    {
        intentId: areaId,
        nodeId: "ba765849-ef8c-4e5e-9d64-9203d7cdb541",
        text: "Hello, this is a test. A really good test because it has a  long text. A realllllllly long text. To check stylings!",
        nodeType: "ProvideInfo",
        nodeChildrenString: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
        isRoot: true,
        isCritical: false,
        optionPath: "",
        valueOptions: "Placeholder",
        nodeComponentType: "ProvideInfo",
        isDynamicTableNode: false,
        dynamicType: null,
        resolveOrder: 0,
        fileAssetResource: null,
    },
    {
        intentId: areaId,
        nodeId: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
        nodeType: "ProvideInfo",
        dynamicType: null,
        text: "You are a great person.",
        isRoot: false,
        optionPath: "Continue",
        isCritical: false,
        valueOptions: "Placeholder",

        isDynamicTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: 0,
        nodeChildrenString: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
        nodeType: "MultipleChoiceAsPath",
        dynamicType: null,

        text: "How are you feeling today? Hello, this is a test. A really good test because it has a  long text. A realllllllly long text. To check stylings!",
        isRoot: false,
        optionPath: "Continue",
        isCritical: false,
        valueOptions: "Good|peg|Bad",

        isDynamicTableNode: false,
        nodeComponentType: "MultipleChoiceAsPath",

        resolveOrder: 0,
        nodeChildrenString: "bee2c623-98d5-4831-bb6d-4a94a1fc7205,fc237abf-fdc5-45d4-a145-beff96c6bcf2",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "fc237abf-fdc5-45d4-a145-beff96c6bcf2",
        nodeType: "SendResponse",
        dynamicType: null,

        text: "We all have bad days. You'll make it through. Just be strong.",
        isRoot: false,
        optionPath: "Bad",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isDynamicTableNode: false,
        nodeComponentType: "SendResponse",

        resolveOrder: 0,
        nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "bee2c623-98d5-4831-bb6d-4a94a1fc7205",
        nodeType: "SendResponse",
        dynamicType: null,

        text: "Good on you. Now take that light and spread it around the world.",
        isRoot: false,
        optionPath: "Good",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isDynamicTableNode: false,
        nodeComponentType: "SendResponse",

        resolveOrder: 0,
        nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "cd16584a-6b06-412a-881b-7402c2b9986e",
        nodeType: "YesNo",
        dynamicType: null,

        text: "We'd like to send you an email with some information regarding your enquiry. Would that be okay?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "No|peg|Yes",

        isDynamicTableNode: false,
        nodeComponentType: "YesNo",

        resolveOrder: null,
        nodeChildrenString: "5e95a84a-de6b-4453-8ed9-2c7ebb2e8b1c,4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "5e95a84a-de6b-4453-8ed9-2c7ebb2e8b1c",
        nodeType: "SendEmail",
        dynamicType: null,

        text: "Wait just a moment while I send you a confirmation email with some information",
        isRoot: false,
        optionPath: "Yes",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: null,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        nodeType: "Restart",
        dynamicType: null,

        text: "Thanks for your time. If you'd like to start again, click the button below.",
        isRoot: false,
        optionPath: "No",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isDynamicTableNode: false,
        nodeComponentType: "Restart",

        resolveOrder: null,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "EmailSuccessfulNodeId",
        nodeType: "ProvideInfo",
        dynamicType: null,

        text: "We've sent through an email to the address you provided. Sometimes these emails are picked up as spam, so if you don't see it, be sure to check your spam folder.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: null,
        nodeChildrenString: "Restart",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "Restart",
        nodeType: "Restart",
        dynamicType: null,

        text: "Thanks for your time. If you'd like to start again, click the button below.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isTerminalType: true,

        isDynamicTableNode: false,
        nodeComponentType: "Restart",

        resolveOrder: null,
        nodeChildrenString: "Terminate",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "EmailFailedNodeId",
        nodeType: "EmailSendFailedFirstAttempt",
        dynamicType: null,

        text: "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: null,
        nodeChildrenString: "11640f68-7551-41cb-b4f3-f414c94c8c47",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "11640f68-7551-41cb-b4f3-f414c94c8c47",
        nodeType: "SendEmail",
        dynamicType: null,

        text: "Wait just a moment while I try that again.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: null,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "FallbackEmailFailedNodeId",
        nodeType: "EmailSendFailedFirstAttempt",
        dynamicType: null,

        text: "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "ProvideInfo",

        resolveOrder: null,
        nodeChildrenString: "e8d6d29c-62e3-46d2-aa8b-75aed7125500",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "e8d6d29c-62e3-46d2-aa8b-75aed7125500",
        nodeType: "SendEmail",
        dynamicType: null,

        text: "Wait just a moment while I try that again.",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "SendEmail",

        resolveOrder: null,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "4c452b9e-d471-425a-80fc-15f5bb9ad04a",
        nodeType: "YesNo",
        dynamicType: null,

        text: "We'd like to send you a follow-up email with some general information regarding your enquiry. Would that be okay?",
        isRoot: false,
        optionPath: "",
        isCritical: false,
        valueOptions: "No|peg|Yes",

        isDynamicTableNode: false,
        nodeComponentType: "YesNo",

        resolveOrder: null,
        nodeChildrenString: "bf068b90-f087-48cd-951a-415f733ef42f,4523a9ec-1acb-4e7d-bbaf-d2ef0cfaa3e2",
        fileAssetResource: null,
    },
    {
        intentId: areaId,

        nodeId: "bf068b90-f087-48cd-951a-415f733ef42f",
        nodeType: "SendTooComplicatedEmail",
        dynamicType: null,

        text: "Wait just a moment while I send an email.",
        isRoot: false,
        optionPath: "Yes",
        isCritical: false,
        valueOptions: "",

        isDynamicTableNode: false,
        nodeComponentType: "SendTooComplicatedEmail",

        resolveOrder: null,
        nodeChildrenString: "Placeholder",
        fileAssetResource: null,
    },
];

export const shortStaticConvoSequence = (areaId: string) => {
    return [
        {
            intentId: areaId,

            nodeId: "ba765849-ef8c-4e5e-9d64-9203d7cdb541",
            nodeType: "ProvideInfo",
            dynamicType: null,

            text: "Hello, this is a test.",
            isRoot: true,
            optionPath: "",
            isCritical: false,
            valueOptions: "Placeholder",

            isDynamicTableNode: false,
            nodeComponentType: "ProvideInfo",

            resolveOrder: 0,
            nodeChildrenString: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
            fileAssetResource: null,
        },
        {
            intentId: areaId,

            nodeId: "4ff32fc7-ecac-4b29-aa38-46b33f29b9a5",
            nodeType: "ProvideInfo",
            dynamicType: null,

            text: "You are a great person.",
            isRoot: false,
            optionPath: "Continue",
            isCritical: false,
            valueOptions: "Placeholder",

            isDynamicTableNode: false,
            nodeComponentType: "ProvideInfo",

            resolveOrder: 0,
            nodeChildrenString: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
            fileAssetResource: null,
        },
        {
            intentId: areaId,

            nodeId: "d1cf63d8-f182-4989-bb67-4f1fedfa9c10",
            nodeType: "MultipleChoiceAsPath",
            dynamicType: null,

            text: "How are you feeling today?",
            isRoot: false,
            optionPath: "Continue",
            isCritical: false,
            valueOptions: "Good|peg|Bad",

            isDynamicTableNode: false,
            nodeComponentType: "MultipleChoiceAsPath",

            resolveOrder: 0,
            nodeChildrenString: "bee2c623-98d5-4831-bb6d-4a94a1fc7205,fc237abf-fdc5-45d4-a145-beff96c6bcf2",
            fileAssetResource: null,
        },
        {
            userResponse: "Good",
            fileAssetResource: null,
        },
        {
            intentId: areaId,

            nodeId: "bee2c623-98d5-4831-bb6d-4a94a1fc7205",
            nodeType: "SendResponse",
            dynamicType: null,

            text: "Good on you. Now take that light and spread it around the world.",
            isRoot: false,
            optionPath: "Good",
            isCritical: false,
            valueOptions: "",

            isTerminalType: true,

            isDynamicTableNode: false,
            nodeComponentType: "SendResponse",

            resolveOrder: 0,
            nodeChildrenString: "cd16584a-6b06-412a-881b-7402c2b9986e",
            fileAssetResource: null,
        },
    ];
};
