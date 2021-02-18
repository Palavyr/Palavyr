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
    onSave(): Promise<boolean>;
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
        background: "#C7ECEE",

    },
    header: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
        // borderTopRightRadius: "8px",
        // borderTopLeftRadius: "8px",
    },
    title: {
        fontWeight: "bold"
        // fontSize: "28pt"
    },
    body: {
        border: "0px solid black"
    },
    accordianActions: {
        display: "flex",
        padding: "8px",
        alignItems: "center",
        justifyContent: "flex-end",
    }
}))


export const ExpandableTextBox = ({ updatableValue, title, onChange, onSave, children }: IExpandableTextBox) => {

    const classes = useStyles();

    return (
        <Accordion className={classes.body}>
            <AccordionSummary
                className={classes.header}
                expandIcon={<ExpandMoreIcon style={{color: "white"}}/>}
                aria-controls="panel-content"
                id="panel-header"
            >
                <Typography className={classes.title}>{title}</Typography>
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
            <AccordionActions className={classes.accordianActions}>
                <SaveOrCancel onSave={onSave} />
            </AccordionActions>

        </Accordion>
    );
};
