import React, { useContext, useEffect, useState } from "react";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableContainer, Paper, Table, Button, FormControlLabel, Checkbox, AccordionActions, makeStyles } from "@material-ui/core";
import { SelectOneFlatHeader } from "./SelectOneFlatHeader";
import { SelectOneFlatBody } from "./SelectOneFlatBody";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { DynamicTableProps, SelectOneFlatData } from "@Palavyr-Types";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { DynamicTableHeader } from "../../DynamicTableHeader";

const useStyles = makeStyles(theme => ({
    tableStyles: {
        width: "100%",
        backgroundColor: "transparent",
        border: "none",
        borderTop: `4px solid ${theme.palette.primary.main}`,
        borderBottom: `4px solid ${theme.palette.primary.main}`,
        boxShadow: "none",
    },
    table: {
        border: "none",
        boxShadow: "none",
    },
    alignLeft: {
        position: "relative",
        top: "50%",
        float: "left",
        paddingLeft: "0.3rem",
    },
    alignRight: {
        position: "relative",
        top: "50%",
        transform: "translateY(25%)",
        float: "right",
        paddingRight: "0.3rem",
        height: "100%",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {
        marginRight: "0.4rem",
    },
}));

export const SelectOneFlat = ({
    showDebug,
    tableId,
    tables,
    setTables,
    tableMetaIndex,
    areaIdentifier,
    deleteAction,
    onSaveFactory,
    availableDynamicTableOptions,
    tableNameMap,
    unitTypes,
    tableTag,
    setTableTag,
    inUse,
    setLocalTable,
    localTable,
}: DynamicTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [tableRows, setTableRows] = useState<SelectOneFlatData[]>([]);
    const [useOptionsAsPaths, setUseOptionsAsPaths] = useState<boolean>(false);

    useEffect(() => {
        const tableRows = tables[tableMetaIndex].tableRows;
        const useOptionsAsPaths = tables[tableMetaIndex].tableMeta.valuesAsPaths;
        setTableRows(tableRows);
        setUseOptionsAsPaths(useOptionsAsPaths);
    }, []);

    const modifier = new SelectOneFlatModifier(updatedRows => {
        setTableRows(updatedRows);
    });

    const useOptionsAsPathsOnChange = async (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        setUseOptionsAsPaths(checked);
    };

    const onSave = onSaveFactory(modifier, DynamicTableTypes.SelectOneFlat, () => {
        localTable.tableMeta.valuesAsPaths = useOptionsAsPaths;
    });

    const addOptionOnClick = () => modifier.addOption(tableRows, repository, areaIdentifier, tableId);

    return (
        <>
            <DynamicTableHeader
                localTable={localTable}
                setLocalTable={setLocalTable}
                setTables={setTables}
                availableDynamicTableOptions={availableDynamicTableOptions}
                tableNameMap={tableNameMap}
                unitTypes={unitTypes}
                inUse={inUse}
                tableTag={tableTag}
                setTableTag={setTableTag}
            />
            <TableContainer className={cls.tableStyles} component={Paper}>
                <Table className={cls.table}>
                    <SelectOneFlatHeader />
                    <SelectOneFlatBody tableData={tableRows} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addOptionOnClick} color="primary" variant="contained">
                            Add Option
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={useOptionsAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel position="right" onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableRows} properties={["option", "valueMin", "valueMax", "range", "rowOrder"]} />}
        </>
    );
};
