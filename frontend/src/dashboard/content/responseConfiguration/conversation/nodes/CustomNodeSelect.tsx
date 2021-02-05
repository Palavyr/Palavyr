import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText, TextField } from "@material-ui/core";
import { v4 as uuid } from "uuid";
import { NodeOption, NodeTypeOptions } from "@Palavyr-Types";
import Autocomplete from '@material-ui/lab/Autocomplete';


const useStyles = makeStyles(() => ({
    formControl: {
        minWidth: 120,
        width: "100%",
        textAlign: "center",
    },
    selectbox: {
        border: "1px solid gray",
        borderBottom: "0px solid black",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        backgroundColor: "white",
        height: "50px"
    }
}));

export interface ISelectNodeType {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option: string;
    nodeOptionList: NodeTypeOptions;
}

// TODO: merge this with the dynamic table select and create a common reusable component
export const CustomNodeSelect = ({ onChange, option, nodeOptionList }: ISelectNodeType) => {
    const classes = useStyles();

    return (
        <div>
            <FormControl className={classes.formControl}>
                <InputLabel id="simple-select-helper-label"></InputLabel>
                <Autocomplete
                    className={classes.selectbox}
                    options={nodeOptionList}
                    groupBy={(nodeOption) => nodeOption.value}
                    getOptionLabel={(option) => option.value}
                    renderInput={(params) => <TextField {...params} label="Select a node type..." variant="outlined" />}
                />
                {/* <Select className={classes.selectbox} labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                    {
                        Object.keys(nodeOptionList).map(key => {
                            let nodeObj: NodeOption = nodeOptionList[key];
                            return <MenuItem key={uuid()} value={nodeObj.value}>{nodeObj.text}</MenuItem>
                        })
                    }
                </Select> */}
                <FormHelperText className={classes.formControl}>Select</FormHelperText>
            </FormControl>
        </div>
    );
};
