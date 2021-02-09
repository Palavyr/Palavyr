import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText, TextField } from "@material-ui/core";
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
    nodeOptionList: NodeTypeOptions;
    label: string;
}

// TODO: merge this with the dynamic table select and create a common reusable component
export const CustomNodeSelect = ({ onChange, label, nodeOptionList }: ISelectNodeType) => {
    const cls = useStyles();
    const groupGetter = (val: NodeOption) => val.groupName;

    return (
        <div>
            <FormControl className={cls.formControl}>
                <InputLabel id="autodcomplete-label"></InputLabel>
                {nodeOptionList && (
                    <Autocomplete
                        size="small"
                        disableClearable
                        clearOnEscape
                        className={cls.autocomplete}
                        classes={{ root: cls.otherbox }}
                        onChange={onChange}
                        options={sortByPropertyAlphabetical(groupGetter, nodeOptionList)}
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
