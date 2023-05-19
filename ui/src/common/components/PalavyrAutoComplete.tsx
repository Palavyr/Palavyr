import { CHAT_DEMO_LISTBOX_zINDEX } from "@constants";
import { FormControl, FormHelperText, InputLabel, makeStyles, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteProps, AutocompleteRenderInputParams, UseAutocompleteProps } from "@material-ui/lab";
import classNames from "classnames";
import React from "react";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
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
        zIndex: CHAT_DEMO_LISTBOX_zINDEX + 1300,
    },
}));

export interface PalavyrAutoCompleteProps<T> extends Omit<AutocompleteProps<string | T | (string | T)[], any, true, any>, "renderInput"> {
    label?: string;
    renderInput?: (params: AutocompleteRenderInputParams) => React.ReactNode;
}
export const PalavyrAutoComplete = <T extends {}>(props: PalavyrAutoCompleteProps<T>) => {
    const cls = useStyles();
    return (
        <div>
            <FormControl className={cls.formControl}>
                <InputLabel id="palavyr-autocomplete-label">{props.label}</InputLabel>
                {props.options && (
                    <Autocomplete
                        {...props}
                        onChange={props.onChange ?? undefined}
                        defaultValue={props.defaultValue ?? undefined}
                        value={props.value ?? undefined}
                        className={classNames(cls.autocomplete, props.className)}
                        classes={{ root: cls.otherbox, popper: cls.popper }}
                        disableClearable
                        clearOnEscape
                        renderInput={
                            props.renderInput ??
                            ((params: AutocompleteRenderInputParams) => (
                                <TextField {...params} InputLabelProps={{ className: cls.inputLabel }} className={cls.inputLabel} data-lpignore="true" label={props.label} />
                            ))
                        }
                    />
                )}
                <FormHelperText className={cls.formControl}>Select</FormHelperText>
            </FormControl>
        </div>
    );
};
