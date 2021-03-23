import { NodeIdentity, NodeTypeOptions, NodeOption } from "@Palavyr-Types";

const nonBranching = ["Provide Info", "Info Collection", "Terminal"];

export const filteredNodeTypeOptions = (identity: NodeIdentity, nodeTypeOptions: NodeTypeOptions): NodeTypeOptions => {
    let prefiltered = [...nodeTypeOptions];

    if (identity.isInternalToAnabranch) {
        return prefiltered.filter((option: NodeOption) => option);
    } else if (identity.isInternalToSplitMerge) {
        if (identity.isDecendentOfSplitMerge && identity.splitMergeRootSiblingIndex > 0) {
            return prefiltered.filter((option: NodeOption) => nonBranching.includes(option.groupName) || option.value === "MultipleChoiceContinue");
        } else {
            return prefiltered;
        }
    } else {
        return prefiltered;
    }
};
