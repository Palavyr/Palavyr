import { PopperProps, Popper, TextField, CircularProgress, makeStyles, Paper, PaperProps } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import { SelectedOption, SetState, WidgetPreferences } from "@Palavyr-Types";
import classNames from "classnames";
import React, { useContext, useState } from "react";
import { WidgetContext } from "widget/context/WidgetContext";
import Fade from "react-reveal/Fade";

const useStyles = makeStyles(() => ({
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        margin: "0px",
        padding: "0px",

        // paddingLeft: "1rem",
        // paddingRight: "1rem",
    },
    autocomplete: {
        padding: "0px",
        margin: "0px",

        // paddingTop: "0rem",
    },

    paper: {
        boxShadow: "none",
    },
    inputLabel: (prefs: WidgetPreferences) => ({
        "& .MuiFormLabel-root": {
            color: prefs.listFontColor,
            fontSize: "10pt",
        },
    }),
    listbox: (prefs: WidgetPreferences) => ({
        // the dropdown menu styles
        backgroundColor: prefs.selectListColor, // TODO: make customizable with new option
        padding: "0rem",
        boxShadow: "none",
    }),
    popper: {
        // boxShadow: "none",
        // backgroundColor: "white",
        // border: "2px dashed black",
    },
}));

export interface ChoiceListProps {
    options: any;
    disabled: boolean;
    onChange(event: any, newOption: SelectedOption): void;
    open?: boolean;
    setOpen: SetState<boolean> | null;
}
export const ChoiceList = ({ options, disabled, onChange, setOpen = null, open = false }: ChoiceListProps) => {
    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);
    const [label, setLabel] = useState<string>("What can I help you with today?");
    const [loading, setLoading] = useState<boolean>(false);

    // const ListBoxComponent = ({ children }: { children: React.ReactNode }) => {
    //     return (
    //         <ul role="listbox" style={{ border: "1px solid black" }}>
    //             {children}
    //         </ul>
    //     );
    // };

    const PopperComponent = ({ children, ...rest }: { children: React.ReactNode } & PopperProps) => {
        return (
            <Popper style={{ boxShadow: "none", backgroundColor: "green" }} {...rest}>
                {children}
            </Popper>
        );
    };

    const PaperComponent = ({ children, ...rest }: { children: React.ReactNode } & PaperProps) => {
        return (
            <Fade>
                <Paper style={{ boxShadow: "none", backgroundColor: "white", margin: "0px", padding: "0px" }} {...rest}>
                    {children}
                </Paper>
            </Fade>
        );
    };

    return (
        <>
            {options && (
                <Autocomplete
                    disabled={disabled}
                    size="small"
                    open={open !== undefined ? open : true}
                    loading={loading}
                    classes={{ popper: cls.popper, root: cls.selectbox, listbox: cls.selectbox, paper: classNames(cls.paper, cls.selectListBgColor, cls.selectListFontColor) }}
                    clearOnEscape
                    onOpen={() => {
                        if (setOpen) setOpen(true);
                    }}
                    onClose={() => {
                        if (setOpen) setOpen(false);
                    }}
                    getOptionSelected={(option: SelectedOption, value: SelectedOption) => option.areaId === value.areaId}
                    onChange={onChange}
                    PopperComponent={PopperComponent}
                    PaperComponent={PaperComponent}
                    options={options}
                    getOptionLabel={(option: SelectedOption) => option.areaDisplay}
                    renderInput={(params: AutocompleteRenderInputParams) => (
                        <TextField
                            {...params}
                            id="asyncLoader"
                            className={cls.inputLabel}
                            label={label}
                            disabled={disabled}
                            onChange={e => {
                                setLabel(e.target.value);
                                if (setOpen !== null) setOpen(true);
                            }}
                            inputProps={{
                                ...params.inputProps,
                                autoComplete: "new-password",
                            }}
                            InputProps={{
                                ...params.InputProps,
                                disableUnderline: true,
                                style: { borderBottom: "1px solid black" },
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
