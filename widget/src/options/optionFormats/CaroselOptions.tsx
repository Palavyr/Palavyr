import * as React from 'react';
import { SelectedOption } from '../../types';
import { useHistory, useParams } from 'react-router-dom';
import { Paper, makeStyles, List, Card, Typography, CardContent, ListItem } from '@material-ui/core';


export interface ICaroselOptions {
    setSelectedOption: (option: SelectedOption) => void;
    options: Array<SelectedOption>;
}

const useStyles = makeStyles(theme => ({
    paper: {
        width: "100%",
        height: "100%",
    },
    mainList: {
        maxHeight: "100%",
        height: "100%",
        overflow: 'auto',
    },
    listItem: {
        textAlign: "center",
        justifyContent: "center"
    },



    root: {
        minWidth: 275,
    },
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
}))


export const CaroselOptions = ({ setSelectedOption, options }: ICaroselOptions) => {

    const history = useHistory();
    const { secretKey } = useParams();
    const classes = useStyles();

    return (
        <Paper className={classes.paper}>
            <Card className={classes.root}>
                <CardContent>
                    <Typography variant="h5" component="h2">
                        Welcome!
                    </Typography>
                    <Typography variant="body2" className={classes.pos} color="textSecondary">
                        We want our service costs to be as transparent as possible. Select an option and use the chat to get a service cost estimate emailed directly to you.
                    </Typography>
                </CardContent>
            </Card>
            {
                options &&
                <List
                    className={classes.mainList}
                >
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
                                            history.push(`/widget/${secretKey}`)
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
