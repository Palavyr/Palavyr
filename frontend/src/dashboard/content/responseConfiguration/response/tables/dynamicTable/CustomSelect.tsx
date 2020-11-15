import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText } from "@material-ui/core";

type StyleProps = {
    align: "center" | "left" | "right";
    width: number;
    minWidth: number;
    maxWidth: number;
};

const useStyles = makeStyles((theme) => ({
    formControl: (props: StyleProps) => {

        let styles = {};
        if (props.minWidth) {
            styles = {...styles, minWidth: props.minWidth};
        }
        if (props.maxWidth) {
            styles = {...styles, maxWidth: props.maxWidth};
        }
        if (props.width) {
            styles = {...styles, width: props.width}
        }
        return styles;
    },
    selectbox: (props: StyleProps) => {

        let styles = {
            paddingLeft: ".7rem",
            paddingRight: ".7rem",
            border: "1px solid gray",
            borderBottom: "0px solid black",
            borderRadius: "0px",
            borderBottomLeftRadius: "3px",
            borderBottomRightRadius: "3px",
            textAlign: props.align ?? "center",
            backgroundColor: "white",
        };
        return styles;
    }
}));

export interface ISelect {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option: string;
    options: Array<string>;

    align?: "left" | "center" | "right" ;
    width?: string;
    minWidth?: number;
    maxWidth?: number;
    fullWidth?: boolean;
    inputLabel?: string;
    helperText?: string;
}

export const CustomSelect = ({ onChange, option, options, align, minWidth, maxWidth, fullWidth, inputLabel, helperText }: ISelect) => {
    const classes = useStyles({ minWidth, maxWidth, align });
    return (
        <FormControl fullWidth={fullWidth} className={classes.formControl}>
            <InputLabel id="simple-select-helper-label">{inputLabel}</InputLabel>
            <Select className={classes.selectbox} labelId="simple-select-helper-label" id="simple-select-helper" value={option} onChange={onChange}>
                {options.map((opt, index) => {
                    return (
                        <MenuItem key={index} value={opt}>
                            {opt}
                        </MenuItem>
                    );
                })}
            </Select>
            <FormHelperText style={{ textAlign: align }}>{helperText}</FormHelperText>
        </FormControl>
    );
};
