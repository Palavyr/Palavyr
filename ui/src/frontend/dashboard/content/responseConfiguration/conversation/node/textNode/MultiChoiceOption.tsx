import React from "react";
import { Grid, TextField, Button } from "@material-ui/core";

interface IMultiChoiceOption {
    option: string;
    optionIndex: number;
    options: string[];
    setOptions: (options: string[]) => void;
    switchState: boolean;
    setSwitchState: any;
}


export const MultiChoiceOption = ({option, optionIndex, options, setOptions, switchState, setSwitchState}: IMultiChoiceOption) => {

    return (
        <>
            <Grid item xs={9} >
                <TextField
                    style={{margin: "1rem"}}
                    label="option"
                    type="text"
                    variant="outlined"
                    value={option}
                    color="primary"
                    onChange={(event) => {
                        event.preventDefault();
                        const val = event.target.value;
                        options[optionIndex] = val;
                        setOptions(options);
                        setSwitchState(!switchState);
                    }}
                />
            </Grid>
            <Grid item xs={3} >
                <Button
                    onClick={() => {
                        options.splice(optionIndex, 1);
                        setOptions(options);
                        setSwitchState(!switchState);
                    }}
                >Remove</Button>
            </Grid>
        </>
    )
}
