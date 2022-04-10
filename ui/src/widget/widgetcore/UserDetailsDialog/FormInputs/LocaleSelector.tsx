import { makeStyles, Paper, PaperProps, Popper, PopperProps, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import { LocaleMap, LocaleResource, WidgetPreferences } from "@Palavyr-Types";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import React, { useContext } from "react";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import ArrowDropUpIcon from "@material-ui/icons/ArrowDropUp";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import classNames from "classnames";

export interface LocaleSelectorProps {
    options: LocaleMap;
    onChange(event: any, newOption: LocaleResource): void;
    disabled: boolean;
}

const useStyles = makeStyles(theme => ({
    select: (props: WidgetPreferences) => ( {
        backgroundColor: props.selectListColor,
    }),
    root: (props: WidgetPreferences) => ({
        marginTop: "2.2rem",
        background: "none",
        // borderBottom: "1px solid " + props.chatFontColor,
    }),
    paper: {
        backgroundColor: theme.palette.common.white,
    },
    inputLabel: (props: WidgetPreferences) => ({
        borderBottom: "1px solid " + props.chatFontColor,
        fontFamily: props.fontFamily,
        color: props.chatFontColor,
        "& .MuiFormLabel-root": {
            fontFamily: props.fontFamily,
            color: props.chatFontColor,
            fontSize: "10pt",
            justifyContent: "center",
        },
        "& .MuiInputBase-input": {
            color: props.chatFontColor,
            background: "none",
        },
        "& .MuiInput-underline:before": {
            borderBottomColor: props.chatFontColor, // Semi-transparent underline
        },
        "& .MuiInput-underline:hover:before": {
            borderBottomColor: props.chatFontColor, // Solid underline on hover
        },
        "& .MuiInput-underline:after": {
            borderBottomColor: props.chatFontColor, // Solid underline on focus
        },
    }),

    icon: (prefs: WidgetPreferences) => ({
        color: prefs.chatFontColor,
    }),
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
        fontFamily: prefs.fontFamily,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        fontFamily: prefs.fontFamily,
        color: prefs.listFontColor,
    }),
    selectbox: (props: WidgetPreferences) => ({
        marginTop: "1rem",
        backgroundColor: props.selectListColor,
        // padding: "0px",
    }),
}));

const PopperComponent = ({ children, ...rest }: { children: React.ReactNode } & PopperProps) => {
    return (
        <Popper popperOptions={{ placement: "bottom" }} style={{ boxShadow: "none" }} {...rest}>
            {children}
        </Popper>
    );
};

export const LocaleSelector = ({ options, onChange, disabled }: LocaleSelectorProps) => {
    const sortGetter = (opt: LocaleResource) => opt.displayName;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    const PaperComponent = ({ children, ...rest }: { children: React.ReactNode } & PaperProps) => {
        return (
            <Paper style={{ boxShadow: "none", backgroundColor: cls.selectListBgColor, border: "1px solid lightgray", margin: "0px", padding: "0px", marginTop: "0.3rem" }} {...rest}>
                {children}
            </Paper>
        );
    };

    return (
        <>
            {options && (
                <Autocomplete
                    popupIcon={<ArrowDropDownIcon className={cls.icon} />}
                    closeIcon={<ArrowDropUpIcon className={cls.icon} />}
                    disabled={disabled}
                    size="small"
                    classes={{ root: cls.selectbox, listbox: cls.selectbox, paper: classNames(cls.paper, cls.selectListBgColor, cls.selectListFontColor) }}
                    className={cls.root}
                    disableClearable
                    clearOnEscape
                    PopperComponent={PopperComponent}
                    PaperComponent={PaperComponent}
                    onChange={onChange}
                    options={opts}
                    getOptionLabel={(option: LocaleResource) => option.displayName}
                    renderInput={(params: AutocompleteRenderInputParams) => (
                        <TextField
                            {...params}
                            disabled={disabled}
                            className={cls.inputLabel}
                            id="field1"
                            label="Select your locale..."
                            inputProps={{
                                ...params.inputProps,
                                autoComplete: "new-password",
                            }}
                            InputProps={{
                                ...params.InputProps,
                                disableUnderline: true,
                                style: { borderBottom: "1px solid black" },
                            }}
                            SelectProps={{
                                className: cls.select,
                            }}
                        />
                    )}
                />
            )}
        </>
    );
};
