import * as React from "react";
import { SelectedOption, WidgetPreferences } from "../../types";
import { useHistory, useLocation } from "react-router-dom";
import { makeStyles, Card, Box, TextField } from "@material-ui/core";
import { useEffect } from "react";
import { sortByPropertyAlphabetical } from "src/common/sorting";
import Autocomplete, { AutocompleteRenderInputParams } from "@material-ui/lab/Autocomplete";
import classNames from "classnames";
import { Dispatch } from "react";
import { SetStateAction } from "react";
import { openUserDetails, toggleUserDetails } from "src/widgetCore/store/dispatcher";

const useStyles = makeStyles(() => ({
    root: {
        '& .MuiAutocomplete-popper': {
            backgroundColor: 'black',
            zIndex: 99999999
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
        wordWrap: "break-word",
        borderRadius: "0px",
    }),
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal",
    },
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        paddingLeft: "2rem",
        paddingRight: "2rem",
    },
    autocomplete: {
        paddingTop: "1rem",
    },
    mainList: {
        maxHeight: "100%",
        height: "100%",
    },
    inputLabel: (prefs: WidgetPreferences) => ({
        "& .MuiFormLabel-root": {
            color: prefs.listFontColor,
            fontSize: "10pt",
        },
    }),
}));

export interface DropdownListProps {
    setSelectedOption: (option: SelectedOption) => void;
    options: Array<SelectedOption>;
    preferences: WidgetPreferences;
}

export const DropdownListOptions = ({ setSelectedOption, options, preferences }: DropdownListProps) => {
    const history = useHistory();
    var secretKey = new URLSearchParams(useLocation().search).get("key");
    const cls = useStyles(preferences);

    useEffect(() => {
        console.log(preferences);
    }, []);

    const onChange = (event: any, newOption: SelectedOption) => {
        setSelectedOption(newOption);
        history.push(`/widget?key=${secretKey}`);
        openUserDetails();
    };
    const sortGetter = (opt: SelectedOption) => opt.areaDisplay;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    return (
        <Box height="100%">
            <Card className={cls.header}>{preferences && <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.header }} />}</Card>
            <div className={classNames(cls.selectListBgColor, cls.selectListFontColor, cls.mainList)}>
                {options && (
                    <Autocomplete
                        size="small"
                        classes={{ root: cls.selectbox, paper: classNames(cls.selectListBgColor, cls.selectListFontColor) }}
                        disableClearable
                        clearOnEscape
                        className={classNames(cls.root, cls.autocomplete, cls.mainList, cls.selectListBgColor, cls.selectListFontColor)}
                        onChange={onChange}
                        options={opts}
                        getOptionLabel={(option: SelectedOption) => option.areaDisplay}
                        renderInput={(params: AutocompleteRenderInputParams) => (
                            <TextField
                                {...params}
                                id="field1"
                                className={cls.inputLabel}
                                label="Select an area or start typing..."
                                inputProps={{
                                    ...params.inputProps,
                                    autoComplete: "new-password",
                                }}
                            />
                        )}
                    />
                )}
            </div>
        </Box>
    );
};
