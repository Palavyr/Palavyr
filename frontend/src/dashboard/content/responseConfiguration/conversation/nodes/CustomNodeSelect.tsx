import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText } from "@material-ui/core";
import { NodeOptions, NodeTypeOptions } from "./NodeTypeOptions";
import { v4 as uuid } from "uuid";


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
        borderBottomRightRadius: "3px"
    }
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
                <Select className={classes.selectbox} labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                    {
                        Object.keys(completeNodeTypes).map(key => {
                            var nodeObj: NodeOptions = completeNodeTypes[key];
                            return <MenuItem key={uuid()} value={nodeObj.value}>{nodeObj.text}</MenuItem>
                        })
                    }
                </Select>
                <FormHelperText className={classes.formControl}>Select</FormHelperText>
            </FormControl>
        </div>
    );
};
