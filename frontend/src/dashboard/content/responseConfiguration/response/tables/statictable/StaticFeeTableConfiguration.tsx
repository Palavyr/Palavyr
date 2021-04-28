// save from outside from: https://stackoverflow.com/questions/7020659/submit-form-using-a-button-outside-the-form-tag
// can assign static tables one form id, and the variable table a different form id

import React from "react";
import { StaticTableMetas } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import { ApiClient } from "@api-client/Client";
import { Button, makeStyles, Typography } from "@material-ui/core";
import { StaticFeeTable } from "./StaticFeeTable";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";

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
    buttonContainer: {
        width: "100%",
        display: "flex",
        justifyContent: "flex-end",
    },
    tablebutton: {
        margin: theme.spacing(1),
        marginBottom: "1rem",
    },
}));

export const StaticTableConfiguration = ({ title, staticTables, tableSaver, tableCanceler, modifier, areaIdentifier, children }: IFeeConfiguration) => {
    var client = new ApiClient();
    const cls = useStyles();

    const actions = (
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
    );

    return (
        <PalavyrAccordian title={title} initialState={true} actions={actions}>
            {children}
            {staticTables.length === 0 && (
                <Typography align="center" color="secondary" style={{ padding: "0.8rem" }} variant="h5">
                    No static fee tables configured for this area.
                </Typography>
            )}
            {staticTables
                .sort((a, b) => a.tableOrder - b.tableOrder)
                .map((table, index) => (
                    <StaticFeeTable staticTableMetas={staticTables} staticTableMeta={table} tableModifier={modifier} key={index} />
                ))}
            <div className={cls.buttonContainer}>
                <Button startIcon={<AddBoxIcon />} variant="contained" size="large" color="primary" className={cls.tablebutton} onClick={() => modifier.addTable(staticTables, client, areaIdentifier)}>
                    Add Table
                </Button>
            </div>
        </PalavyrAccordian>
    );
};
