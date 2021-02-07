import * as React from "react";
import { SelectedOption, WidgetPreferences } from "../../types";
import { useHistory, useLocation } from "react-router-dom";
import { Paper, makeStyles, Card, TextField } from "@material-ui/core";
import { useEffect } from "react";
import { sortByPropertyAlphabetical } from "src/common/sorting";
import Autocomplete from "@material-ui/lab/Autocomplete";

const useStyles = makeStyles(() => ({
    paper: {
        width: "100%",
        height: "100%",
    },
    mainList: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
        color: prefs.listFontColor,
        maxHeight: "100%",
        height: "100%",
        overflow: "auto",
    }),
    listItem: {
        textAlign: "center",
        justifyContent: "center",
    },
    root: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.headerColor,
        color: prefs.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        borderRadius: "0px",
    }),
    bullet: {
        display: "inline-block",
        margin: "0 2px",
        transform: "scale(0.8)",
    },
    title: {
        fontSize: 14,
    },
    pos: {
        marginBottom: 12,
    },
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal",
    },
    autocomplete: {
        marginTop: "1rem",
    },
    selectbox: {
        paddingLeft: "2rem",
        paddingRight: "2rem",
    },
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
    };
    const sortGetter = (opt: SelectedOption) => opt.areaDisplay;

    return (
        <Paper className={cls.paper}>
            <Card className={cls.root}>{preferences && <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.header }} />}</Card>
            {options && (
                <Autocomplete
                    size="small"
                    classes={{ root: cls.selectbox }}
                    disableClearable
                    clearOnEscape
                    className={cls.autocomplete}
                    onChange={onChange}
                    options={sortByPropertyAlphabetical(sortGetter, options)}
                    getOptionLabel={(option: SelectedOption) => option.areaDisplay}
                    renderInput={params => <TextField data-lpignore="true" label="Select an area..." {...params} />}
                />
            )}
        </Paper>
    );
};
