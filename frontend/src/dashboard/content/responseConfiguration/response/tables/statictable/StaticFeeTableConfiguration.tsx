// save from outside from: https://stackoverflow.com/questions/7020659/submit-form-using-a-button-outside-the-form-tag
// can assign static tables one form id, and the variable table a different form id

import React from "react";
import { StaticTableMetas } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import { ApiClient } from "@api-client/Client";
import { Accordion, AccordionSummary, Divider, Button, AccordionActions, makeStyles, Typography } from "@material-ui/core";
import { StaticFeeTable } from "./StaticFeeTable";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import AddBoxIcon from "@material-ui/icons/AddBox";

interface IFeeConfiguration {
    title: string;
    staticTables: StaticTableMetas;
    modifier: StaticTablesModifier;
    tableSaver(staticTables: StaticTableMetas): Promise<boolean>;
    tableCanceler(): Promise<any>;
    areaIdentifier: string;
    children: React.ReactNode;
}

const useStyles = makeStyles((theme) => ({
    title: {
        fontWeight: "bold",
    },
    tablebutton: {
        margin: theme.spacing(1),
        marginBottom: "1rem",
    },
    header: {
        background: "linear-gradient(354deg, rgb(1,161,214,1) 10%, rgba(0,212,255,1) 70%)",
    },
}));

export const StaticTableConfiguration = ({ title, staticTables, tableSaver, tableCanceler, modifier, areaIdentifier, children }: IFeeConfiguration) => {
    var client = new ApiClient();
    const classes = useStyles();

    return (
        <Accordion>
            <AccordionSummary className={classes.header} expandIcon={<ExpandMoreIcon style={{ color: "white" }} />} aria-controls="panel-content" id="panel-header">
                <Typography className={classes.title}>{title}</Typography>
            </AccordionSummary>
            {children}
            <span className={"m-1"}>
                {staticTables
                    .sort((a, b) => a.tableOrder - b.tableOrder)
                    .map((table, index) => (
                        <StaticFeeTable staticTableMetas={staticTables} staticTableMeta={table} tableModifier={modifier} key={index} />
                    ))}
            </span>

            <Button startIcon={<AddBoxIcon />} variant="contained" size="large" color="primary" className={classes.tablebutton} onClick={() => modifier.addTable(staticTables, client, areaIdentifier)}>
                Add Table
            </Button>

            <Divider />
            <AccordionActions>
                <SaveOrCancel
                    onSave={async () => {
                        const result = await tableSaver(staticTables);
                        if (result) {
                            return true;
                        }
                        return false;
                    }}
                    onCancel={async () => {
                        await tableCanceler();
                        return true;
                    }}
                />
            </AccordionActions>
        </Accordion>
    );
};
