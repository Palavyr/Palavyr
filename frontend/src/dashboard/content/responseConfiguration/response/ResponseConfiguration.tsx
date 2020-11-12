import React, { useState, useCallback, useEffect } from "react";
import { StaticTableMetas } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { StaticTablesModifier } from "./tables/statictable/staticTableModifier";
import { LogueModifier } from "./logueModifier";
import { cloneDeep } from "lodash";
import { ExpandableTextBox } from "@common/components/ExpandableTextBox";
import { Statement } from "@common/components/Statement";
import { DynamicTableConfiguration } from "./tables/dynamicTable/DynamicTableConfiguration";
import { StaticTableConfiguration } from "./tables/statictable/StaticFeeTableConfiguration";
import { makeStyles } from "@material-ui/core";
import { EstimateHelp } from "dashboard/content/help/EstimateHelp";
import { useParams } from "react-router-dom";


const useStyles = makeStyles((theme) => ({
    titleText: {
        textAlign: "center",
        fontWeight: "bold",
    },
}));

export const ResponseConfiguration = () => {

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [, setLoaded] = useState(false);
    const [prologue, setPrologue] = useState<string>("");
    const [staticTables, setStaticTables] = useState<StaticTableMetas>([]);
    const [epilogue, setEpilogue] = useState<string>("");

    var client = new ApiClient();
    const staticTablesModifier = new StaticTablesModifier(setStaticTables);
    const prologueModifier = new LogueModifier(setPrologue);
    const epilogueModifier = new LogueModifier(setEpilogue);

    const savePrologue = async () => {
        await client.Configuration.updatePrologue(areaIdentifier, prologue);
    };

    const saveEpilogue = async () => {
        await client.Configuration.updateEpilogue(areaIdentifier, epilogue);
    };

    const updateEpilogue = (event: { target: { value: string } }) => {
        epilogueModifier.simpleUpdateState(event.target.value);
    };

    const updatePrologue = (event: { target: { value: string } }) => {
        prologueModifier.simpleUpdateState(event.target.value);
    };

    const tableSaver = async () => {
        staticTables.forEach((table) => {
            table.id = null;
            table.staticTableRows.forEach((row) => {
                row.id = null;
                row.fee.id = null;
            });
        });
        const { data } = await client.Configuration.Tables.Static.updateStaticTablesMetas(areaIdentifier, staticTables);
        setStaticTables(data);
    };

    const loadEstimateConfiguration = useCallback(async () => {
        const { data } = await client.Configuration.getEstimateConfiguration(areaIdentifier);
        const {prologue, epilogue, staticTablesMetas} = data;
        setPrologue(cloneDeep(prologue));
        setEpilogue(cloneDeep(epilogue));
        setStaticTables(staticTablesMetas);
        setLoaded(true);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        loadEstimateConfiguration();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadEstimateConfiguration]);

    const classes = useStyles();
    return (
        <>
            <EstimateHelp />
            <ExpandableTextBox title="Introductory statement" updatableValue={prologue} onChange={updatePrologue} onSave={savePrologue}>
                <Statement title="Intro Statement">
                    <>
                        <span>Use this section to create an introduction statement for your estimate.</span>
                        <span>You can make it clear that fees are estimate only, or provide context for your client to understand their estimate.</span>
                    </>
                </Statement>
            </ExpandableTextBox>

            <DynamicTableConfiguration title="Customized Fees" areaIdentifier={areaIdentifier} />

            <StaticTableConfiguration areaIdentifier={areaIdentifier} title="Static Fees" staticTables={staticTables} tableSaver={tableSaver} modifier={staticTablesModifier} />

            <ExpandableTextBox title="Outro statement" updatableValue={epilogue} onChange={updateEpilogue} onSave={saveEpilogue}>
                <Statement title="Epilogue Statement">
                    <>
                        <span>Use this section to create an outro statement for your estimate.</span>
                        <span>E.g. You can make it clear that fees are estimate only, or provide context for your client to understand their estimate.</span>
                    </>
                </Statement>
            </ExpandableTextBox>
        </>
    );
};
