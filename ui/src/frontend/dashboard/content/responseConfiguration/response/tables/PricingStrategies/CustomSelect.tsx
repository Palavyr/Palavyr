import React from "react";
import { makeStyles, FormControl, Select, MenuItem, FormHelperText, Tooltip } from "@material-ui/core";
import KeyboardArrowDownIcon from "@material-ui/icons/KeyboardArrowDown";
import classNames from "classnames";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

type StyleProps = {
    align?: "center" | "left" | "right";
    width?: number;
    minWidth?: number;
    maxWidth?: number;
};

const useStyles = makeStyles(theme => ({
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
    form: {
        margin: "0.3rem",
        border: "0px solid black",
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
    },
    selectbox: (props: StyleProps) => {
        let styles = {
            paddingLeft: ".7rem",
            paddingRight: ".7rem",
            border: "opx solid gray",
            borderBottom: "0px solid black",
            borderRadius: "5px",
            borderBottomLeftRadius: "3px",
            borderBottomRightRadius: "3px",
            textAlign: props.align ? props.align : "center",
            backgroundColor: "white",
            cursor: "help",
        };
        return styles;
    },
    label: {
        cursor: "help",
        color: theme.palette.common.black,
        textAlign: "center",
    },
    toolTip: {
        // fontSize: "12pt"
    },
    menu: {
        border: "none",
    },
    input: {
        border: "0px solid black",
        marginLeft: theme.spacing(1),
        marginTop: theme.spacing(1),
        marginBottom: theme.spacing(1),
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
    styles?: any;
    selectIcon?: React.ReactNode;
}

export const CustomSelect = ({ toolTipTitle, disabled, onChange, option, options, align, minWidth, maxWidth, fullWidth, inputLabel, helperText, styles }: ISelect) => {
    const cls = useStyles({ minWidth, maxWidth, align });
    return (
        <FormControl fullWidth={fullWidth} className={classNames(cls.formControl, cls.form)} style={styles}>
            <Tooltip className={cls.toolTip} title={toolTipTitle ? toolTipTitle : ""} arrow placement="top-start">
                <FormHelperText>
                    <PalavyrText align="center" varient="body2">
                        {helperText}
                    </PalavyrText>
                </FormHelperText>
            </Tooltip>

            <Select
                disableUnderline
                inputProps={{ className: cls.input }}
                MenuProps={{ className: cls.menu }}
                IconComponent={KeyboardArrowDownIcon}
                disabled={disabled}
                className={cls.selectbox}
                value={option}
                onChange={onChange}
            >
                {options.map((opt, index) => {
                    return (
                        <MenuItem key={index} value={opt}>
                            {opt}
                        </MenuItem>
                    );
                })}
            </Select>
        </FormControl>
    );
};
