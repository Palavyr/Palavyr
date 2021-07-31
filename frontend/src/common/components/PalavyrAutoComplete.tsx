import { CHAT_DEMO_LISTBOX_zINDEX } from "@constants";
import { FormControl, FormHelperText, InputLabel, makeStyles, TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";
import React from "react";

export interface PalavyrAutoCompleteProps<T> {
    label: string;
    options: T[];
    shouldDisableSelect: boolean;
    onChange: (event: any, option: T) => void;
    groupby?(option: T): string;
    getOptionLabel?(option: T): string;
    getOptionSelected?(option: T, value: T): boolean;
}

const useStyles = makeStyles((theme) => ({
    formControl: {
        minWidth: 120,
        width: "100%",
        textAlign: "center",
    },
    autocomplete: {
        marginTop: "1rem",
        paddingBottom: "0.5rem",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        border: "0px dashed green",
    },
    selectbox: {
        color: "black",
        textAlign: "center",
        border: "0px solid yellow",
        borderRadius: "0px",
        borderBottomLeftRadius: "3px",
        borderBottomRightRadius: "3px",
        backgroundColor: "white",
        height: "200px",
    },
    otherbox: {
        textAlign: "center",
    },
    inputLabel: {
        "& .MuiFormLabel-root": {
            color: "black",
            fontSize: "10pt",
            textAlign: "center",
            wordWrap: "auto",
        },
    },
    popper: {
        zIndex: CHAT_DEMO_LISTBOX_zINDEX,
    },
}));

export const PalavyrAutoComplete = <T extends {}>({ label, options, shouldDisableSelect, onChange, groupby, getOptionLabel, getOptionSelected }: PalavyrAutoCompleteProps<T>) => {
    const cls = useStyles();
    return (
        <div>
            <FormControl className={cls.formControl}>
                <InputLabel id="autocomplete-label"></InputLabel>
                {options && (
                    <Autocomplete
                        size="small"
                        disabled={shouldDisableSelect}
                        getOptionSelected={getOptionSelected ? getOptionSelected : undefined}
                        disableClearable
                        clearOnEscape
                        className={cls.autocomplete}
                        classes={{ root: cls.otherbox, popper: cls.popper }}
                        onChange={onChange}
                        options={options}
                        groupBy={groupby && groupby}
                        getOptionLabel={getOptionLabel}
                        renderInput={(params) => <TextField {...params} InputLabelProps={{ className: cls.inputLabel }} className={cls.inputLabel} data-lpignore="true" label={label} />}
                    />
                )}
                <FormHelperText className={cls.formControl}>Select</FormHelperText>
            </FormControl>
        </div>
    );
};
