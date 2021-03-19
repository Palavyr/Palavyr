import React, { useEffect } from "react";
import { makeStyles, FormControl, InputLabel, FormHelperText, TextField } from "@material-ui/core";
import { NodeOption, NodeTypeOptions } from "@Palavyr-Types";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";

const useStyles = makeStyles(() => ({
    formControl: {
        minWidth: 120,
        width: "100%",
        textAlign: "center",
    },
    autocomplete: {
        marginTop: "1rem",
        height: "50px",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        border: "0px dashed green",
    },
    selectbox: {
        color: "black",
        textAlign: "center",
        border: "0px solid yellow",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        backgroundColor: "white",
    },
    otherbox: {
        textAlign: "center",
        // paddingLeft: "2rem",
        // paddingRight: "2rem",
    },
    inputLabel: {
        "& .MuiFormLabel-root": {
            color: "black",
            fontSize: "12pt",
            textAlign: "center",
        },
    },
}));

export interface ISelectNodeType {
    onChange: (event: any, nodeOption: NodeOption) => void;
    nodeTypeOptions: NodeTypeOptions;
    label: string;
    shouldDisabledNodeTypeSelector: boolean;
    reRender: () => void;
}

//https://github.com/mui-org/material-ui/issues/19173 to help resolve the label not resetting to '' when unsetting the node.
export const CustomNodeSelect = ({
    onChange,
    label,
    nodeTypeOptions,
    shouldDisabledNodeTypeSelector,
    reRender
}: ISelectNodeType) => {
    const cls = useStyles();
    const groupGetter = (val: NodeOption) => val.groupName;

    return (
        <div>
            <FormControl className={cls.formControl}>
                <InputLabel id="autodcomplete-label"></InputLabel>
                {nodeTypeOptions && (
                    <Autocomplete
                        size="small"
                        disabled={shouldDisabledNodeTypeSelector}
                        disableClearable
                        clearOnEscape
                        className={cls.autocomplete}
                        classes={{ root: cls.otherbox }}
                        onChange={onChange}
                        options={sortByPropertyAlphabetical(groupGetter, nodeTypeOptions)}
                        groupBy={(nodeOption) => nodeOption.groupName}
                        getOptionLabel={(option) => option.text}
                        renderInput={(params) => <TextField {...params} InputLabelProps={{ className: cls.inputLabel }} className={cls.inputLabel} data-lpignore="true" label={label} />}
                    />
                )}
                <FormHelperText className={cls.formControl}>Select</FormHelperText>
            </FormControl>
        </div>
    );
};
