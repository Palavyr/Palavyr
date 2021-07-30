import * as React from "react";
import { SelectedOption, WidgetPreferences } from "../../globalTypes";
import { useHistory, useLocation } from "react-router-dom";
import { makeStyles, Card, Box, TextField } from "@material-ui/core";
import Autocomplete, { AutocompleteRenderInputParams } from "@material-ui/lab/Autocomplete";
import classNames from "classnames";
import { sortByPropertyAlphabetical } from "common/sorting";
import { getWidgetPreferences, openUserDetails } from "@store-dispatcher";
import { BrandingStrip } from "common/BrandingStrip";

const useStyles = makeStyles(() => ({
    root: {
        "& .MuiAutocomplete-popper": {
            backgroundColor: "black",
            zIndex: 99999999,
        },
    },
    container: {
        display: "flex",
        flexDirection: "column",
        width: "100%",
        wordWrap: "break-word",
    },
    innerShadow: {
        boxShadow: "inset 0px 0px 63px -30px rgba(77,13,77,1)",
    },
    header: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.headerColor,
        color: prefs.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        minHeight: "20%",
        maxHeight: "30%",
        wordWrap: "break-word",
        borderRadius: "0px",
    }),
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "18%",
    },
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        width: "100%",
        paddingLeft: "2rem",
        paddingRight: "2rem",
    },
    autocomplete: {
        paddingTop: "1rem",
    },
    mainList: {
        maxHeight: "97%",
        height: "97%",
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
    }),
}));

export interface DropdownListProps {
    setSelectedOption: (option: SelectedOption) => void;
    options: SelectedOption[];
}

export const DropdownListOptions = ({ setSelectedOption, options }: DropdownListProps) => {
    const history = useHistory();
    var secretKey = new URLSearchParams(useLocation().search).get("key");
    var preferences = getWidgetPreferences();
    const cls = useStyles(preferences);

    const onChange = (event: any, newOption: SelectedOption) => {
        setSelectedOption(newOption);
        history.push(`/widget?key=${secretKey}`);
        openUserDetails();
    };
    const sortGetter = (opt: SelectedOption) => opt.areaDisplay;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    return (
        <Box height="100%" className={cls.container}>
            <Card className={cls.header}>{preferences && <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.landingHeader }} />}</Card>
            <div className={classNames(cls.selectListBgColor, cls.selectListFontColor, cls.mainList)}>
                {options && (
                    <Autocomplete
                        size="small"
                        open={true}
                        classes={{ popper: cls.popper, root: cls.selectbox, paper: classNames(cls.selectListBgColor, cls.selectListFontColor) }}
                        disableClearable
                        clearOnEscape
                        className={classNames(cls.root, cls.autocomplete, cls.mainList, cls.selectListBgColor, cls.selectListFontColor)}
                        onChange={onChange}
                        options={opts}
                        ListboxProps={{ className: cls.listbox }}
                        getOptionLabel={(option: SelectedOption) => option.areaDisplay}
                        renderInput={(params: AutocompleteRenderInputParams) => (
                            <TextField
                                {...params}
                                id="field1"
                                className={cls.inputLabel}
                                label="What can I help you with today?"
                                inputProps={{
                                    ...params.inputProps,
                                    autoComplete: "new-password",
                                }}
                            />
                        )}
                    />
                )}
            </div>
            <BrandingStrip />
        </Box>
    );
};
