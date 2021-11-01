import * as React from "react";
import { SelectedOption, WidgetPreferences } from "../../../../common/types/globalTypes-widget";
import { useHistory, useLocation } from "react-router-dom";
import { makeStyles, Card, Box } from "@material-ui/core";
import { sortByPropertyAlphabetical } from "common/sorting";
import { WidgetContext } from "widget/context/WidgetContext";
import { useContext } from "react";
import { ChoiceList } from "./ChoiceList";
import classNames from "classnames";

const useStyles = makeStyles(() => ({
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

    const sortGetter = (opt: SelectedOption) => opt.areaDisplay;
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
