import { makeStyles, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import { LocaleMap, LocaleMapItem } from "@Palavyr-Types";
import { sortByPropertyAlphabetical } from "common/sorting";
import React from "react";


export interface LocaleSelectorProps {
    options: LocaleMap;
    onChange(event: any, newOption: LocaleMapItem): void;
}

const useStyles = makeStyles(theme => ({
    select: {
        zIndex: 99999,
    },
    root: {
        marginTop: "2rem"
    }
}));

export const LocaleSelector = ({ options, onChange }: LocaleSelectorProps) => {
    const sortGetter = (opt: LocaleMapItem) => opt.countryName;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    const cls = useStyles();

    return (
        options && (
            <Autocomplete
                size="small"
                className={cls.root}
                disableClearable
                clearOnEscape
                onChange={onChange}
                options={opts}
                getOptionLabel={(option: LocaleMapItem) => option.countryName}
                renderInput={(params: AutocompleteRenderInputParams) => (
                    <TextField
                        {...params}
                        id="field1"
                        label="Select your locale..."
                        inputProps={{
                            ...params.inputProps,
                            autoComplete: "new-password",
                        }}
                        SelectProps={{
                            className: cls.select,
                        }}
                    />
                )}
            />
        )
    );
};
