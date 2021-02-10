import { makeStyles, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import React from "react";
import { sortByPropertyAlphabetical } from "src/common/sorting";
import { LocaleMap, LocaleMapItem } from "src/types";

export interface LocaleSelectorProps {
    options: LocaleMap;
    onChange(event: any, newOption: LocaleMapItem): void;
}

const useStyles = makeStyles(theme => ({
    select: {
        zIndex: 99999,
    },
}));

export const LocaleSelector = ({ options, onChange }: LocaleSelectorProps) => {
    const sortGetter = (opt: LocaleMapItem) => opt.countryName;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    const cls = useStyles();

    return (
        options && (
            <Autocomplete
                size="small"
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
