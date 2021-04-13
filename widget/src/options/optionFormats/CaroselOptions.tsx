import * as React from 'react';
import { SelectedOption, WidgetPreferences } from '../../globalTypes';
import { useHistory, useLocation } from 'react-router-dom';
import { Paper, makeStyles, List, Card, ListItem } from '@material-ui/core';
import { useEffect } from 'react';


export interface ICaroselOptions {
    setSelectedOption: (option: SelectedOption) => void;
    options: Array<SelectedOption>;
    preferences: WidgetPreferences;
}

const useStyles = makeStyles(theme => ({
    paper: {
        width: "100%",
        height: "100%",
    },
    mainList: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
        color: prefs.listFontColor,
        maxHeight: "100%",
        height: "100%",
        overflow: 'auto',
    }),
    listItem: {
        textAlign: "center",
        justifyContent: "center"
    },
    root: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.headerColor,
        color: prefs.headerFontColor,
        textAlign: "center",
        minWidth: 275,
        borderRadius: "0px"
    }),
    bullet: {
        display: 'inline-block',
        margin: '0 2px',
        transform: 'scale(0.8)',
    },
    title: {
        fontSize: 14,
    },
    pos: {
        marginBottom: 12,
    },
    headerBehavior: {
        // hyphens: "auto",
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal"
    }
}))


export const CaroselOptions = ({ setSelectedOption, options, preferences }: ICaroselOptions) => {

    const history = useHistory();
    var secretKey = (new URLSearchParams(useLocation().search)).get("key")
    const classes = useStyles(preferences);

    useEffect(() => {
        console.log(preferences);
    }, [])

    return (
        <Paper className={classes.paper}>
            <Card className={classes.root}>
                {
                    preferences &&
                    <div
                        className={classes.headerBehavior}
                        dangerouslySetInnerHTML={{ __html: preferences.header}}
                    />
                }
            </Card>
            {
                options &&
                <List className={classes.mainList}>
                    {
                        options.map((opt, index) => {
                            return (
                                <ListItem
                                    key={index + opt.areaDisplay}
                                    button
                                    disableGutters
                                    className={classes.listItem}
                                    onClick={
                                        () => {
                                            setSelectedOption(opt)
                                            history.push(`/widget?key=${secretKey}`)
                                        }}
                                >
                                    {opt.areaDisplay}
                                </ListItem>
                            )
                        })
                    }
                </List>
            }
        </Paper>
    )
}
