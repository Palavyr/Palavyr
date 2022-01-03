import React, { Dispatch, SetStateAction } from "react";
import { Grid, Button } from "@material-ui/core";
import { MultiChoiceOption } from "./MultiChoiceOption";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { Alert } from "@material-ui/lab";

interface IMultiChoiceOptions {
    options: string[];
    setOptions: (options: string[]) => void;
    switchState: boolean;
    setSwitchState: Dispatch<SetStateAction<boolean>>;
    addMultiChoiceOptionsOnClick: () => void;
    locked?: boolean;
}

export const MultiChoiceOptions = ({ options, setOptions, switchState, setSwitchState, addMultiChoiceOptionsOnClick, locked }: IMultiChoiceOptions) => {
    return locked ? (
        <div style={{ marginTop: "2rem" }}>
            <PalavyrText>
                <Alert severity="error">
                    <strong>Path options are currently locked.</strong>
                </Alert>
            </PalavyrText>
        </div>
    ) : (
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
