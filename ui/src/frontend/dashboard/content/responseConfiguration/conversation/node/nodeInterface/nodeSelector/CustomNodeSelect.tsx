import React from "react";
import { NodeOption, NodeTypeOptions } from "@Palavyr-Types";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";

export interface ISelectNodeType {
    onChange: (event: any, nodeOption: NodeOption) => void;
    nodeTypeOptions: NodeTypeOptions;
    label: string;
    shouldDisabledNodeTypeSelector: boolean;
}

//https://github.com/mui-org/material-ui/issues/19173 to help resolve the label not resetting to '' when unsetting the node.
export const CustomNodeSelect = ({ onChange, label, nodeTypeOptions, shouldDisabledNodeTypeSelector }: ISelectNodeType) => {
    const groupGetter = (val: NodeOption) => val.groupName;
    const sortedNodeOptions = sortByPropertyAlphabetical(groupGetter, nodeTypeOptions);
    return (
        <PalavyrAutoComplete
            label={label}
            options={sortedNodeOptions}
            shouldDisableSelect={shouldDisabledNodeTypeSelector}
            onChange={onChange}
            groupby={(nodeOption: NodeOption) => nodeOption.groupName}
            getOptionLabel={(option: NodeOption) => option.text}
            getOptionSelected={(option: NodeOption, value: NodeOption) => option.value === value.value}
        />
    );
};
