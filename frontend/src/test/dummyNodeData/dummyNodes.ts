import { ConvoNode } from "@Palavyr-Types"

export const ParentNode: ConvoNode = {
    nodeId: "xyz-1",
    nodeType: "YesNo",
    fallback: false,
    text: "Test Text",
    nodeChildrenString: "xyz-2,xyz-3",
    isCritical: true,
    isRoot: true,
    areaIdentifier: "abc-123",
    optionPath: "",
    valueOptions: "", // an array, but bc of the dtabase we store as a string delimited by |peg|
    id: 0
}

export const ChildNode1: ConvoNode = {
    nodeId: "xyz-2",
    nodeType: "",
    fallback: false,
    text: "Test Text",
    nodeChildrenString: "",
    isCritical: true,
    isRoot: true,
    areaIdentifier: "abc-123",
    optionPath: "Yes",
    valueOptions: "", // an array, but bc of the dtabase we store as a string delimited by |peg|
    id: 0
}

export const ChildNode2: ConvoNode = {
    nodeId: "xyz-3",
    nodeType: "",
    fallback: false,
    text: "Test Text",
    nodeChildrenString: "",
    isCritical: true,
    isRoot: true,
    areaIdentifier: "abc-123",
    optionPath: "No",
    valueOptions: "", // an array, but bc of the dtabase we store as a string delimited by |peg|
    id: 0
}

export const DummyConvo = [
    ParentNode,
    ChildNode1,
    ChildNode2
]
