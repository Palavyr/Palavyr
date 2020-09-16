import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText } from "@material-ui/core";
import { NodeOptions, NodeTypeOptions } from "./NodeTypeOptions";
import { uuid } from "uuidv4";


const useStyles = makeStyles(() => ({
    formControl: {
        minWidth: 120,
        width: "100%",
        textAlign: "center"
    },
}));

export interface ISelectNodeType {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option: string;
    completeNodeTypes: NodeTypeOptions;
}

// TODO: merge this with the dynamic table select and create a common reusable component
export const CustomNodeSelect = ({ onChange, option, completeNodeTypes }: ISelectNodeType) => {
    const classes = useStyles();

    return (
        <div>
            <FormControl className={classes.formControl}>
                <InputLabel id="simple-select-helper-label"></InputLabel>
                <Select labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                    {
                        Object.keys(completeNodeTypes).map(key => {
                            var nodeObj: NodeOptions = completeNodeTypes[key];
                            return <MenuItem key={uuid()} value={nodeObj.value}>{nodeObj.text}</MenuItem>
                        })
                    }
                </Select>
                <FormHelperText className={classes.formControl}>Select the type of node</FormHelperText>
            </FormControl>
        </div>
    );
};
