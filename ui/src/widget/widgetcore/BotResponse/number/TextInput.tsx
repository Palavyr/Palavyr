import { StandardTextFieldProps } from "@material-ui/core";
import { TextField } from "@material-ui/core";
import React from "react";

export interface TextInputProps extends StandardTextFieldProps {
    inputPropsClassName?: string;
    inputLabelPropsClassName?: string;
}

export const TextInput = ({ inputPropsClassName, inputLabelPropsClassName, ...rest }: TextInputProps) => {
    return (
        <TextField
            className={rest.className}
            InputProps={{
                type: rest.type,
                className: inputPropsClassName,
                disableUnderline: true,
                style: { borderBottom: "1px solid black" },
            }}
            InputLabelProps={{
                className: inputLabelPropsClassName,
            }}
            fullWidth
            multiline
            label={rest.label ? rest.label : "Provide text"}
            {...rest}
        />
    );
};
