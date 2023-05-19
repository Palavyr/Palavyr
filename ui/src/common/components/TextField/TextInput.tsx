import { makeStyles, TextField, TextFieldProps } from "@material-ui/core";
import React from "react";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    input: {
        width: "30ch"
    },
}));

export const TextInput = ({ ...rest }: TextFieldProps) => {
    const cls = useStyles();
    return <TextField className={cls.input} {...rest} />;
};
