import { ConvoTableRow } from "../../globalTypes";

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
    nodeComponentType: "YesNo",
    dynamicType: ""
};
