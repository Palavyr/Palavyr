import React from "react";
import { TextField, InputAdornment, IconButton, TextFieldProps } from "@material-ui/core";
import VisibilityIcon from "@material-ui/icons/Visibility";
import VisibilityOffIcon from "@material-ui/icons/VisibilityOff";


export type IVisibilityPasswordTextField = TextFieldProps & {
    isVisible: boolean;
    onVisibilityChange: any;
}


export const VisibilityPasswordTextField = ({ isVisible, onVisibilityChange, ...rest }: IVisibilityPasswordTextField) => {
    return (
        <TextField
            {...rest}
            type={isVisible ? "text" : "password"}
            InputProps={{
                endAdornment: (
                    <InputAdornment position="end">
                        <IconButton
                            aria-label="Toggle password visibility"
                            onClick={() => {
                                onVisibilityChange(!isVisible);
                            }}
                            onMouseDown={(event) => {
                                event.preventDefault();
                            }}
                        >
                            {isVisible ? <VisibilityIcon /> : <VisibilityOffIcon />}
                        </IconButton>
                    </InputAdornment>
                ),
            }}
        ></TextField>
    );
}
