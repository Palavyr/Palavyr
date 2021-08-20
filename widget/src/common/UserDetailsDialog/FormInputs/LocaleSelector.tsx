import { makeStyles, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import { LocaleMap, LocaleResource } from "@Palavyr-Types";
import { sortByPropertyAlphabetical } from "common/sorting";
import React from "react";

export interface LocaleSelectorProps {
    options: LocaleMap;
    onChange(event: any, newOption: LocaleResource): void;
}

const useStyles = makeStyles(theme => ({
    select: {
        zIndex: 99999,
        backgroundColor: theme.palette.common.white,
    },
    root: {
        marginTop: "2rem",
        backgroundColor: theme.palette.common.white,
    },
    paper: {
        backgroundColor: theme.palette.common.white
    }
}));

export const LocaleSelector = ({ options, onChange }: LocaleSelectorProps) => {
    const sortGetter = (opt: LocaleResource) => opt.displayName;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    const cls = useStyles();

    return (
        options && (
            <Autocomplete
                size="small"
                classes={{root: cls.root, paper: cls.paper}}
                className={cls.root}
                disableClearable
                clearOnEscape
                onChange={onChange}
                options={opts}
                getOptionLabel={(option: LocaleResource) => option.displayName}
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
