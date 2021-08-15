import React from "react";
import { makeStyles, FormControl, InputLabel, Select, MenuItem, FormHelperText, Tooltip } from "@material-ui/core";

type StyleProps = {
    align?: "center" | "left" | "right";
    width?: number;
    minWidth?: number;
    maxWidth?: number;
};

const useStyles = makeStyles((theme) => ({
    formControl: (props: StyleProps) => {
        let styles = {};
        if (props.minWidth) {
            styles = { ...styles, minWidth: props.minWidth };
        }
        if (props.maxWidth) {
            styles = { ...styles, maxWidth: props.maxWidth };
        }
        if (props.width) {
            styles = { ...styles, width: props.width };
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
            cursor: "help",
        };
        return styles;
    },
    label: {
        cursor: "help",
    },
    toolTip: {
        // fontSize: "12pt"
    },
}));

export interface ISelect {
    onChange: (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => void;
    option?: string;
    options: Array<string>;
    align?: "left" | "center" | "right";
    width?: string;
    minWidth?: number;
    maxWidth?: number;
    fullWidth?: boolean;
    inputLabel?: string;
    helperText?: string;
    disabled?: boolean;
    toolTipTitle?: string;
}

export const CustomSelect = ({ toolTipTitle, disabled, onChange, option, options, align, minWidth, maxWidth, fullWidth, inputLabel, helperText }: ISelect) => {
    const cls = useStyles({ minWidth, maxWidth, align });
    return (
        <FormControl fullWidth={fullWidth} className={cls.formControl}>
            <InputLabel className={cls.label}>{inputLabel}</InputLabel>
            <Tooltip className={cls.toolTip} title={toolTipTitle ?? ""} arrow placement="top-start">
                <Select disabled={disabled} className={cls.selectbox} value={option} onChange={onChange}>
                    {options.map((opt, index) => {
                        return (
                            <MenuItem key={index} value={opt}>
                                {opt}
                            </MenuItem>
                        );
                    })}
                </Select>
            </Tooltip>
            <FormHelperText style={{ textAlign: align }}>{helperText}</FormHelperText>
        </FormControl>
    );
};
