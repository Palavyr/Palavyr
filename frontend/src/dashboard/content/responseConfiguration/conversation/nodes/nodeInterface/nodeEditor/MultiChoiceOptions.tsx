import React, { Dispatch, SetStateAction } from "react";
import { Grid, Button } from "@material-ui/core";
import { MultiChoiceOption } from "./MultiChoiceOption";

interface IMultiChoiceOptions {
    options: string[];
    setOptions: (options: string[]) => void;
    switchState: boolean;
    setSwitchState: Dispatch<SetStateAction<boolean>>;
    addMultiChoiceOptionsOnClick: () => void;
}

export const MultiChoiceOptions = ({ options, setOptions, switchState, setSwitchState, addMultiChoiceOptionsOnClick }: IMultiChoiceOptions) => {
    return (
        <>
            <Grid container spacing={1} alignItems="center">
                {options.map((option, optionIndex) => (
                    <MultiChoiceOption key={optionIndex} option={option} optionIndex={optionIndex} options={options} setOptions={setOptions} setSwitchState={setSwitchState} switchState={switchState} />
                ))}
            </Grid>
            <Button onClick={addMultiChoiceOptionsOnClick}>Add option</Button>
        </>
    );
};
