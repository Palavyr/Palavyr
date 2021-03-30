import { ConvoTableRow } from "../../types";

export const ConvoNode: ConvoTableRow = {
    id: 0,
    nodeId: "abc",
    nodeType: "YesNo",
    isCritical: true,
    fallback: false,
    text: "This is a test",
    nodeChildrenString: "",
    isRoot: true,
    areaIdentifier: "abc123",
    optionPath: "",
    valueOptions: "", // needs to be split by ",",
    isDynamicTableNode: false,
    nodeComponentType: "YesNo"
};


export const RootOfThree = {
    id: 0,
    nodeId: "abc1",
    nodeType: "YesNo",
    isCritical: true,
    fallback: false,
    text: "This is a test",
    nodeChildrenString: "abc2,abc3",
    isRoot: true,
    areaIdentifier: "abc123",
    optionPath: "",
    valueOptions: "Yes|peg|No", // needs to be split by ",",
    isDynamicTableNode: false,
    nodeComponentType: "YesNo"
}

export const ThreeNodes = [
    RootOfThree,
    {
        id: 1,
        nodeId: "abc2",
        nodeType: "",
        isCritical: true,
        fallback: false,
        text: "This is a test",
        nodeChildrenString: "",
        isRoot: false,
        areaIdentifier: "abc123",
        optionPath: "No",
        valueOptions: "", // needs to be split by ",",
        isDynamicTableNode: false,
        nodeComponentType: ""
    },
    {
        id: 2,
        nodeId: "abc3",
        nodeType: "",
        isCritical: true,
        fallback: false,
        text: "This is a test",
        nodeChildrenString: "",
        isRoot: false,
        areaIdentifier: "abc123",
        optionPath: "Yes",
        valueOptions: "", // needs to be split by ",",
        isDynamicTableNode: false,
        nodeComponentType: ""
    }
]