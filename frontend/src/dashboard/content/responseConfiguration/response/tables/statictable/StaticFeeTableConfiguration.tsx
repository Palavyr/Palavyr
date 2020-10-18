// save from outside from: https://stackoverflow.com/questions/7020659/submit-form-using-a-button-outside-the-form-tag
// can assign static tables one form id, and the variable table a different form id

import React from "react";
import { StaticTableMetas } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import { ApiClient } from "@api-client/Client";
import { Accordion, AccordionSummary, Divider, Button, AccordionActions, makeStyles, Typography } from "@material-ui/core";
import { StaticFeeTable } from "./StaticFeeTable";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';


interface IFeeConfiguration {
    title: string;
    staticTables: StaticTableMetas;
    modifier: StaticTablesModifier;
    tableSaver: any; // tech debt
    areaIdentifier: string;
}

const useStyles = makeStyles(theme => ({
    title: {
        fontWeight: "bold"
        // fontSize: "28pt"
    },
    tablebutton: {
        margin: theme.spacing(1),
    },
    header: {
        background: "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)",
        // borderTopRightRadius: "8px",
        // borderTopLeftRadius: "8px",
    },
}))


export const StaticTableConfiguration = ({ title, staticTables, tableSaver, modifier, areaIdentifier }: IFeeConfiguration) => {
    var client = new ApiClient();
    const classes = useStyles();


    return (
        <Accordion >
            <AccordionSummary className={classes.header} expandIcon={<ExpandMoreIcon style={{color: "white"}} />} aria-controls="panel-content" id="panel-header">
                <Typography className={classes.title}>{title}</Typography>
            </AccordionSummary>
            <Divider />

            <span className={"m-1"}>
                {staticTables.sort((a, b) => a.tableOrder - b.tableOrder).map((table, index) => (
                    <StaticFeeTable staticTableMetas={staticTables} staticTableMeta={table} tableModifier={modifier} key={index} />
                ))}
            </span>

            <Button variant="contained" size="large" color="primary" className={classes.tablebutton} onClick={() => modifier.addTable(staticTables, client, areaIdentifier)}>
                Add Table
            </Button>

            <Divider />
            <AccordionActions>
                <SaveOrCancel
                    onSave={() => {
                        console.log("Saving...");
                        tableSaver(staticTables);
                    }}
                    onCancel={() => {
                        console.log("Canceling...");
                    }}
                />
            </AccordionActions>

        </Accordion>
    );
};
