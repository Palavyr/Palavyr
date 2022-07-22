import React from "react";
import { NodeTypeOptionResource, NodeTypeOptionResources } from "@Palavyr-Types";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { TextField } from "@material-ui/core";

export interface ISelectNodeType {
    onChange: (event: any, nodeOption: NodeTypeOptionResource) => void;
    nodeTypeOptions: NodeTypeOptionResources;
    label: string;
    shouldDisabledNodeTypeSelector: boolean;
}

//https://github.com/mui-org/material-ui/issues/19173 to help resolve the label not resetting to '' when unsetting the node.
export const CustomNodeSelect = ({ onChange, label, nodeTypeOptions, shouldDisabledNodeTypeSelector }: ISelectNodeType) => {
    const groupGetter = (val: NodeTypeOptionResource) => val.groupName;
    const sortedNodeOptions = sortByPropertyAlphabetical(groupGetter, nodeTypeOptions);
    return (
        <PalavyrAutoComplete<NodeTypeOptionResource>
            label={label}
            options={sortedNodeOptions}
            disabled={shouldDisabledNodeTypeSelector}
            onChange={onChange}
            groupBy={(nodeOption: NodeTypeOptionResource) => nodeOption.groupName}
            getOptionLabel={(option: NodeTypeOptionResource) => option.text}
            getOptionSelected={(option: NodeTypeOptionResource, value: NodeTypeOptionResource) => option.value === value.value}
            renderInput={params => <TextField {...params} label={label} variant="standard" />}
        />
    );
};
