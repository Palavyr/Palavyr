import React, { useState, useCallback, useEffect, ChangeEvent } from "react";
import { StaticTableMeta, StaticTableMetas, StaticTableRow, StaticTableValidationResult } from "@Palavyr-Types";
import { StaticTablesModifier } from "./tables/statictable/staticTableModifier";
import { LogueModifier } from "./logueModifier";
import { cloneDeep } from "lodash";
import { ExpandableTextBox } from "@common/components/ExpandableTextBox";
import { DynamicTableConfiguration } from "./tables/dynamicTable/DynamicTableConfiguration";
import { StaticTableConfiguration } from "./tables/statictable/StaticFeeTableConfiguration";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { OsTypeToggle } from "../areaSettings/enableAreas/OsTypeToggle";
import { useContext } from "react";
import { makeStyles, Paper } from "@material-ui/core";

const getStaticTableValidationResult = (staticTables: StaticTableMetas): StaticTableValidationResult => {
    let validationResult = true;
    staticTables.map((staticTable: StaticTableMeta) => {
        staticTable.staticTableRows.map((staticTableRow: StaticTableRow) => {
            if (staticTableRow.range) {
                if (staticTableRow.fee.max <= staticTableRow.fee.min) {
                    validationResult = false;
                }
            }
        });
    });

    if (validationResult) {
        return {
            result: true,
            message: "",
        };
    } else {
        return {
            result: false,
            message: "A static table fee upper value is less than or equal to the lower value. Please provide a range where the upper value is greater than the lower value.",
        };
    }
};

const useStyles = makeStyles(theme => ({
    paper: {
        marginLeft: "2.5rem",
        marginRight: "2.5rem",
        padding: "2rem",
        backgroundColor: theme.palette.grey["100"],
    },
}));

export const ResponseConfiguration = () => {
    const [, setLoaded] = useState(false);
    const [prologue, setPrologue] = useState<string>("");
    const [staticTables, setStaticTables] = useState<StaticTableMetas>([]);
    const [epilogue, setEpilogue] = useState<string>("");
    const cls = useStyles();
    const { repository, areaIdentifier } = useContext(DashboardContext);

    const staticTablesModifier = new StaticTablesModifier(setStaticTables, repository);
    const prologueModifier = new LogueModifier(setPrologue);
    const epilogueModifier = new LogueModifier(setEpilogue);

    const savePrologue = async () => {
        const _prologue_ = await repository.Configuration.updatePrologue(areaIdentifier, prologue);
        setPrologue(_prologue_);
        return true;
    };

    const saveEpilogue = async () => {
        const _epilogue_ = await repository.Configuration.updateEpilogue(areaIdentifier, epilogue);
        setEpilogue(_epilogue_);
        return true;
    };

    const updateEpilogue = (event: ChangeEvent<HTMLInputElement>) => {
        event.preventDefault();
        epilogueModifier.simpleUpdateState(event.target.value);
    };

    const updatePrologue = (event: ChangeEvent<HTMLInputElement>) => {
        event.preventDefault();
        prologueModifier.simpleUpdateState(event.target.value);
    };

    const tableSaver = async () => {
        staticTables.forEach(table => {
            table.id = null;
            table.staticTableRows.forEach(row => {
                row.id = null;
                row.fee.id = null;
            });
        });

        const validationResult = getStaticTableValidationResult(staticTables);
        if (validationResult.result === false) {
            alert(validationResult.message); // Temp fix to prevent incorrect values HARDLY. This is not a nice UI experience.
            return false;
        } // TODO: the table saver needs to return the validation result and the SaveOrCancel component needs to require this standard type for the error message.

        const updatedStaticTables = await repository.Configuration.Tables.Static.updateStaticTablesMetas(areaIdentifier, staticTables);
        setStaticTables([]); // This is a hack to get the darn tables to save and rerender correctly.
        setStaticTables(cloneDeep(updatedStaticTables));
        return true;
    };

    const tableCanceler = async () => {
        loadEstimateConfiguration();
    };

    const loadEstimateConfiguration = useCallback(async () => {
        const { prologue, epilogue, staticTablesMetas, sendPdfResponse } = await repository.Configuration.getEstimateConfiguration(areaIdentifier);
        setPrologue(cloneDeep(prologue));
        setEpilogue(cloneDeep(epilogue));
        setStaticTables(staticTablesMetas);
        setSendPdfWithResponse(sendPdfResponse);
        setLoaded(true);
    }, [areaIdentifier]);

    useEffect(() => {
        loadEstimateConfiguration();
        return () => {
            setLoaded(false);
        };
    }, [areaIdentifier, loadEstimateConfiguration]);

    const [sendPdfWithResponse, setSendPdfWithResponse] = useState<boolean | null>(null);

    const onToggleSendPdfWithResponse = async () => {
        const toSend = await repository.Area.toggleSendPdfResponse(areaIdentifier);
        setSendPdfWithResponse(toSend);
    };

    return (
        <>
            <HeaderStrip
                title="Chat Response PDF"
                subtitle="Below are the four sections of your response PDF, which you can customize to your liking. This PDF is structured to provide fee estimate information."
            />
            {sendPdfWithResponse !== null && (
                <OsTypeToggle controlledState={sendPdfWithResponse} onChange={onToggleSendPdfWithResponse} enabledLabel="Send response PDF with email" disabledLabel="Do not send PDF with email" />
            )}
            <Paper className={cls.paper}>
                <ExpandableTextBox initialState={true} title="Introductory statement" updatableValue={prologue} onChange={updatePrologue} onSave={savePrologue}>
                    <HeaderStrip
                        divider
                        light
                        title="Create an introduction for your response PDF"
                        subtitle="This may be useful to explain your estimate. For example, you can make it clear that actual costs might vary, or provide additional context to help understand the subsequent content."
                    />
                </ExpandableTextBox>

                <DynamicTableConfiguration title="Pricing Strategies" areaIdentifier={areaIdentifier}>
                    <HeaderStrip
                        divider
                        light
                        title="Configure a pricing strategy"
                        subtitle="Pricing strategies are fee tables that require input from the customer to determine the fee. By selecting one, you are telling Palavyr.com that you will be including a node in the conversation that will ask the customer for their input."
                    />
                </DynamicTableConfiguration>

                <StaticTableConfiguration areaIdentifier={areaIdentifier} title="Static Fees" staticTables={staticTables} tableSaver={tableSaver} tableCanceler={tableCanceler} modifier={staticTablesModifier}>
                    <HeaderStrip
                        divider
                        light
                        title="Configure a static fee table"
                        subtitle="Static fees are those fees that don't depend on your customer's responses. If you set any fee a 'per individual', then a corresponding 'Num individals' node must be included in the chat conversation."
                    />
                </StaticTableConfiguration>

                <ExpandableTextBox title="Ending statement" updatableValue={epilogue} onChange={updateEpilogue} onSave={saveEpilogue}>
                    <HeaderStrip
                        divider
                        light
                        title="Create an ending statement for your response PDF"
                        subtitle="This section will appear at the end of the response PDF. You can make it clear that fees are estimate only, or provide context for your client to understand their estimate."
                    />
                </ExpandableTextBox>
            </Paper>
        </>
    );
};
