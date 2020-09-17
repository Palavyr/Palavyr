import { Response } from "@Palavyr-Types";


export type NodeOptions = {
    value: string;
    pathOptions: Array<Response>;
    valueOptions: Array<string>;
    text: string;
};

export type NodeTypeOptions = {[index: string]: NodeOptions};


// used in the dropdown select menu in the convotree
export const NodeTypeOptionsDefinition: NodeTypeOptions = {

    // standard
    YesNo: { value: "YesNo", valueOptions: [], pathOptions: ["No", "Yes"], text: "Yes or No" },
    YesNoNotSure: { value: "YesNoNotSure", valueOptions: [], pathOptions: ["Yes", "No", "Not Sure"], text: "Yes, No, Not Sure" },
    YesNotSureCombined: { value: "YesNotSureCombined", valueOptions: [], pathOptions: ["Yes / Not Sure", "No"], text: "Yes / Not Sure, No"},
    NoNotSureCombined: {value: "NoNotSureCombined", valueOptions: [], pathOptions: ["Yes", "No / Not Sure"], text: "Yes, No / Not Sure"},
    TakeText: {value: "TakeText", valueOptions: [], pathOptions: ["Continue"], text: "Take Text"},
    Info: {value: "Info", valueOptions: [], pathOptions: ["Continue"], text: "Provide Info"},
    MultipleChoiceAsPath: {value: "MultipleChoiceAsPath", valueOptions: [], pathOptions: [], text: "Multiple Choice (as Paths)"},
    MultipleChoiceContinue: {value: "MultipleChoiceContinue", valueOptions: [], pathOptions: ["Continue"], text: "Multiple Choice (Continue)"},

    // ending sequences
    TooComplicated: { value: "TooComplicated", valueOptions: [], pathOptions: [null], text: "Too Complicated"},
    EndingSequence: {value: "EndingSequence", valueOptions: [], pathOptions: [null], text: "Ending Sequence"},
     // dynamic tables required TO BE ADDED DYNAMICALLY in ConvoTree.tsx


     // [Deprecated]
    // HowMany: {value: "HowMany", valueOptions: [], pathOptions: ["Continue"], text: "Ask How Many"},
    // HowMuch: {value: "HowMuch", valueOptions: [], pathOptions: ["Continue"], text: "Ask How Much"},
};

export type NodeTypes = "YesNo"
        | "YesNoNotSure"
        | "YesNotSureCombined"
        | "NoNotSureCombined"
        | "TakeText"
        | "Info"
        | "MultipleChoiceAsPath"
        | "MultipleChoiceContinue"
        | "TooComplicated"
        | "EndingSequence"
