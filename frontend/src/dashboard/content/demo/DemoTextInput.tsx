import { TextField } from "@material-ui/core";
import React from "react";
// import { uuid } from "uuidv4";

interface IDemoTextInput {
    text: string;
    value: string;
    onChange(e: { target: { value: React.SetStateAction<string>; }; }): void;
    disabled?: boolean
}

export const DemoTextInput = ({ text, value, onChange, disabled }: IDemoTextInput) => {
    return (
        <TextField
            disabled={disabled}
            style={{ margin: 3, marginBottom: "1.6rem", marginTop: "1rem" }}
            placeholder=""
            helperText={text}
            fullWidth
            margin="normal"
            InputLabelProps={{ shrink: true }}
            value={value}
            onChange={onChange}
        />
    );
};
