import React from "react";
import { Grid, Button } from "@material-ui/core";
import { MultiChoiceOption } from "./MultiChoiceOption";


interface IMultiChoiceOptions {
    options: string[];
    setOptions: (options: string[]) => void;
    switchState: boolean;
    setSwitchState: any;
}


export const MultiChoiceOptions = ({ options, setOptions, switchState, setSwitchState }: IMultiChoiceOptions) => {

    return (
        <>
            <Grid container spacing={1}>
                {
                    options.map((option, optionIndex) => (
                        <MultiChoiceOption
                            key={optionIndex}
                            option={option}
                            optionIndex={optionIndex}
                            options={options}
                            setOptions={setOptions}
                            setSwitchState={setSwitchState}
                            switchState={switchState}
                        />
                    ))
                }
            </Grid>
            <Button
                onClick={() => {
                    options.push("");
                    setOptions(options)
                    setSwitchState(!switchState)
                }}
            >
                Add option
            </Button>
        </>
    )
}
