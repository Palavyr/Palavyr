import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText } from "@material-ui/core";

const useStyles = makeStyles(() => ({
    formControl: {
        minWidth: 120,
        maxWidth: 185,
        width: "100%",
    },
    selectbox: {
        border: "1px solid gray",
        borderBottom: "0px solid black",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        textAlign: "center",
        backgroundColor: "white"
    }
}));

export interface ISelect {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option: string;
    options: Array<string>;
}

export const CustomSelect = ({ onChange, option, options }: ISelect) => {
    const classes = useStyles();
    return (

            <FormControl className={classes.formControl}>
                <InputLabel id="simple-select-helper-label"></InputLabel>
                <Select className={classes.selectbox} labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                    {
                        options.map((opt, index) => {
                            return <MenuItem key={index} value={opt}>{opt}</MenuItem>
                        })
                    }
                </Select>
                <FormHelperText style={{textAlign: "center"}}>Select table type</FormHelperText>
            </FormControl>

    );
};
