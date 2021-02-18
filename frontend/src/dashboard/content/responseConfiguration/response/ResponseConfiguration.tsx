import React, { useState, useCallback, useEffect } from "react";
import { StaticTableMetas } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { StaticTablesModifier } from "./tables/statictable/staticTableModifier";
import { LogueModifier } from "./logueModifier";
import { cloneDeep } from "lodash";
import { ExpandableTextBox } from "@common/components/ExpandableTextBox";
import { DynamicTableConfiguration } from "./tables/dynamicTable/DynamicTableConfiguration";
import { StaticTableConfiguration } from "./tables/statictable/StaticFeeTableConfiguration";
import { makeStyles } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
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

    const { setIsLoading } = React.useContext(DashboardContext);

    const savePrologue = async () => {
        const { data: _prologue_ } = await client.Configuration.updatePrologue(areaIdentifier, prologue);
        setPrologue(_prologue_);
    };

    const saveEpilogue = async () => {
        const { data: _epilogue_ } = await client.Configuration.updateEpilogue(areaIdentifier, epilogue);
        setEpilogue(_epilogue_);
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
        setIsLoading(true);
        const { data } = await client.Configuration.getEstimateConfiguration(areaIdentifier);
        const { prologue, epilogue, staticTablesMetas } = data;
        setPrologue(cloneDeep(prologue));
        setEpilogue(cloneDeep(epilogue));
        setStaticTables(staticTablesMetas);
        setLoaded(true);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier]);

    useEffect(() => {
        loadEstimateConfiguration();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadEstimateConfiguration]);

    return (
        <>
            <AreaConfigurationHeader title="Custom and Static Tables" subtitle="Use this editor to configure the fee tables, as well as associated information, that will be sent in the response PDF for this area." />
            <ExpandableTextBox title="Introductory statement" updatableValue={prologue} onChange={updatePrologue} onSave={savePrologue}>
                <AreaConfigurationHeader divider title="Create an introduction for your estimate" subtitle="You can make it clear that fees are estimate only, or provide context for your client to understand their estimate." />
            </ExpandableTextBox>

            <DynamicTableConfiguration title="Customized Fees" areaIdentifier={areaIdentifier}>
                <AreaConfigurationHeader divider title="Configure a custom fee table" subtitle="When you configure a custom fee table, it creates a corresponding palavyr node that must be included in the chat conversation." />
            </DynamicTableConfiguration>

            <StaticTableConfiguration areaIdentifier={areaIdentifier} title="Static Fees" staticTables={staticTables} tableSaver={tableSaver} modifier={staticTablesModifier}>
                <AreaConfigurationHeader
                    divider
                    title="Configure a static fee table"
                    subtitle="Static fees are those fees that don't depend on your customer's responses. If you set any fee a 'per individual', then a corresponding 'Num individals' node must be included in the chat conversation."
                />
            </StaticTableConfiguration>

            <ExpandableTextBox title="Ending statement" updatableValue={epilogue} onChange={updateEpilogue} onSave={saveEpilogue}>
                <AreaConfigurationHeader divider title="Create an ending statement for your estimate" subtitle="You can make it clear that fees are estimate only, or provide context for your client to understand their estimate." />
            </ExpandableTextBox>
        </>
    );
};
