import { makeStyles, TextField, TextFieldProps } from "@material-ui/core";
import React from "react";

const useStyles = makeStyles(theme => ({
    input: {
        width: "30ch"
    },
}));

export const TextInput = ({ ...rest }: TextFieldProps) => {
    const cls = useStyles();
    return <TextField className={cls.input} {...rest} />;
};
