import Divider from "@material-ui/core/Divider";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import AccordionSummary from "@material-ui/core/AccordionSummary";
import React from "react";
import { SaveOrCancel } from "./SaveOrCancel";
import { Accordion, TextField, AccordionActions, makeStyles, Typography } from "@material-ui/core";
import { AnyVoidFunction } from "@Palavyr-Types";

export interface IExpandableTextBox {
    title: string;
    children: React.ReactNode;
    updatableValue: string;
    onChange: AnyVoidFunction;
    onSave: AnyVoidFunction;
}

const useStyles = makeStyles(theme => ({
    textField: {
        margin: "1rem",
        padding: "1rem",
        border: "1px dashed lightgray",
        borderRadius: "8px",
        '&:hover': {
            background: "white"
        },
        background: "#efefef"
    },
    header: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
        // background: "#90caf9",
        borderTopRightRadius: "8px",
        borderTopLeftRadius: "8px",
    },
    title: {
        fontSize: "28pt"
    },
    body: {
        marginBottom: "3rem"
    }
}))


export const ExpandableTextBox = ({ updatableValue, title, onChange, onSave, children }: IExpandableTextBox) => {

    const classes = useStyles();

    return (
        <Accordion className={classes.body} defaultExpanded>
            <AccordionSummary
                className={classes.header}
                expandIcon={<ExpandMoreIcon style={{color: "white"}}/>}
                aria-controls="panel-content"
                id="panel-header"
            >
                <Typography variant="h2" className={classes.title}>{title}</Typography>
            </AccordionSummary>
            <Divider variant="fullWidth" />
            {children}
            <div className={classes.textField}>
                <TextField
                    fullWidth
                    multiline
                    placeholder="Place text here"
                    rows={3}
                    value={updatableValue}
                    onChange={onChange}
                ></TextField>
            </div>
            <Divider light />
            <AccordionActions>
                <SaveOrCancel onSave={onSave} />
            </AccordionActions>

        </Accordion>
    );
};
