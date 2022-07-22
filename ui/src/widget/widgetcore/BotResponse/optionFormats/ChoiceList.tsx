import { PopperProps, Popper, TextField, CircularProgress, makeStyles, Paper, PaperProps } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import { SelectedOption, SetState } from "@Palavyr-Types";
import classNames from "classnames";
import React, { useContext, useEffect, useState } from "react";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";

const useStyles = makeStyles(() => ({
    selectListBgColor: (prefs: WidgetPreferencesResource) => ({
        backgroundColor: prefs.selectListColor,
        fontFamily: prefs.fontFamily,
    }),
    selectListFontColor: (prefs: WidgetPreferencesResource) => ({
        fontFamily: prefs.fontFamily,
        color: prefs.listFontColor,
    }),
    selectbox: {
        margin: "0px",
        padding: "0px",
    },
    autocomplete: {
        padding: "0px",
        margin: "0px",
    },

    paper: {
        boxShadow: "none",
    },
    inputLabel: (props: WidgetPreferencesResource) => ({
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
    listbox: (prefs: WidgetPreferencesResource) => ({
        color: prefs.chatFontColor,
        // the dropdown menu styles
        fontFamily: prefs.fontFamily,
        backgroundColor: prefs.selectListColor, // TODO: make customizable with new  option
        padding: "0rem",
        boxShadow: "none",
    }),

    icon: (prefs: WidgetPreferencesResource) => ({
        color: prefs.chatFontColor,
    }),
}));

export interface ChoiceListProps {
    options: any;
    disabled: boolean;
    onChange(event: any, newOption: SelectedOption): void;
    open?: boolean;
    setOpen: SetState<boolean> | null;
}
const PopperComponent = ({ children, ...rest }: { children: React.ReactNode } & PopperProps) => {
    return (
        <Popper popperOptions={{ placement: "bottom" }} style={{ boxShadow: "none" }} {...rest}>
            {children}
        </Popper>
    );
};
export const ChoiceList = ({ options, disabled, onChange, setOpen = null, open = false }: ChoiceListProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    const [label, setLabel] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        // if (preferences && preferences.sel) {
            // TODO: make this a preference in the server DBs
            // setLabel(preferences.selectionLabel);
        // } else {
        //     setLabel("What can I help you with today?");
        // }
    }, []);

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
                    disabled={disabled}
                    size="small"
                    open={open !== undefined ? open : true}
                    loading={loading}
                    classes={{ root: cls.selectbox, listbox: cls.selectbox, paper: classNames(cls.paper, cls.selectListBgColor, cls.selectListFontColor) }}
                    clearOnEscape
                    onOpen={() => {
                        if (setOpen) setOpen(true);
                        setLoading(false);
                    }}
                    onClose={() => {
                        if (setOpen) setOpen(false);
                        setLoading(false);
                    }}
                    getOptionSelected={(option: SelectedOption, value: SelectedOption) => option.intentId === value.intentId}
                    onChange={onChange}
                    PopperComponent={PopperComponent}
                    PaperComponent={PaperComponent}
                    options={options}
                    getOptionLabel={(option: SelectedOption) => option.intentDisplay}
                    renderInput={(params: AutocompleteRenderInputParams) => (
                        <TextField
                            {...params}
                            id="asyncLoader"
                            className={cls.inputLabel}
                            label={label}
                            disabled={disabled}
                            onChange={e => {
                                setLoading(true);
                                setTimeout(() => {
                                    setLabel(e.target.value);
                                    if (setOpen !== null) setOpen(true);
                                    setLoading(false);
                                }, 1500);
                            }}
                            inputProps={{
                                ...params.inputProps,
                                autoComplete: "new-password",
                            }}
                            InputProps={{
                                ...params.InputProps,
                                disableUnderline: true,
                                endAdornment: (
                                    <>
                                        {loading ? <CircularProgress color="inherit" size={20} /> : null}
                                        {params.InputProps.endAdornment}
                                    </>
                                ),
                            }}
                        />
                    )}
                />
            )}
        </>
    );
};
