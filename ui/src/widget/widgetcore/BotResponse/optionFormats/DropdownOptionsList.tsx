import * as React from "react";
import { SelectedOption } from "@Palavyr-Types";

import { useHistory, useLocation } from "react-router-dom";
import { makeStyles, Card, Box } from "@material-ui/core";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { useContext } from "react";
import { ChoiceList } from "./ChoiceList";
import classNames from "classnames";
import { WidgetPreferencesResource } from "@common/types/api/EntityResources";


const useStyles = makeStyles<{}>(() => ({
    container: {
        display: "flex",
        flexDirection: "column",
        width: "100%",
        wordWrap: "break-word",
    },
    innerShadow: {
        boxShadow: "inset 0px 0px 63px -30px rgba(77,13,77,1)",
    },
    header: (prefs: WidgetPreferencesResource) => ({
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
    selectListBgColor: (prefs: WidgetPreferencesResource) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferencesResource) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        paddingLeft: "1rem",
        paddingRight: "1rem",
    },
    autocomplete: {
        paddingTop: "0rem",
    },
    mainList: {
        maxHeight: "97%",
        height: "97%",
    },
    paper: {
        boxShadow: "none",
    },
    inputLabel: (prefs: WidgetPreferencesResource) => ({
        "& .MuiFormLabel-root": {
            color: prefs.listFontColor,
            fontSize: "10pt",
        },
    }),
    listbox: (prefs: WidgetPreferencesResource) => ({
        // the dropdown menu styles
        backgroundColor: prefs.selectListColor, // TODO: make customizable with new option
        padding: "0rem",
        boxShadow: "none",
    }),
    popper: {
        boxShadow: "none",
        backgroundColor: "white",
        border: "2px dashed black",
    },
}));


export interface DropdownListProps {
    options: SelectedOption[];
    onChange(event: any, newOption: SelectedOption): void;
    disabled: boolean;
}

export const DropdownListOptions = ({ disabled, options, onChange }: DropdownListProps) => {
    const history = useHistory();
    var secretKey = new URLSearchParams(useLocation().search).get("key");

    const { preferences } = useContext(WidgetContext);
    const cls = useStyles(preferences);

    const sortGetter = (opt: SelectedOption) => opt.intentDisplay;
    const opts = sortByPropertyAlphabetical(sortGetter, options);
    return (
        <Box height="100%" className={cls.container}>
            <Card className={cls.header}>{preferences && <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.landingHeader }} />}</Card>
            <div className={classNames(cls.selectListBgColor, cls.selectListFontColor, cls.mainList)}>
                <ChoiceList setOpen={null} options={opts} disabled={disabled} onChange={onChange} />
            </div>
        </Box>
    );
};
