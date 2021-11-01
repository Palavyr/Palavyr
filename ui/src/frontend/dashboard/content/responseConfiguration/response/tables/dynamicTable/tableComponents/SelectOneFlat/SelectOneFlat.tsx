import React, { useContext } from "react";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableContainer, Paper, Table, Button, FormControlLabel, Checkbox, AccordionActions, makeStyles } from "@material-ui/core";
import { SelectOneFlatHeader } from "./SelectOneFlatHeader";
import { SelectOneFlatBody } from "./SelectOneFlatBody";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { DynamicTableProps } from "@Palavyr-Types";
import AddBoxIcon from "@material-ui/icons/AddBox";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { cloneDeep } from "lodash";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

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

export const SelectOneFlat = ({ showDebug, tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: DynamicTableProps) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const modifier = new SelectOneFlatModifier(setTableData);

    const useOptionsAsPathsOnChange = async (event: { target: { checked: boolean } }) => {
        tableMeta.valuesAsPaths = event.target.checked;
        setTableMeta(cloneDeep(tableMeta));
    };

    const onSave = async () => {
        const result = modifier.validateTable(tableData);

        if (result) {
            const newTableMeta = await repository.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableMeta);
            const savedData = await repository.Configuration.Tables.Dynamic.saveDynamicTable<SelectOneFlatModifier[]>(areaIdentifier, DynamicTableTypes.SelectOneFlat, tableData, tableId, tableTag);
            setTableMeta(newTableMeta);
            setTableData(savedData);
            return true;
        } else {
            return false;
        }
    };

    const addOptionOnClick = () => modifier.addOption(tableData, repository, areaIdentifier, tableId);

    return (
        <>
            <TableContainer className={cls.tableStyles} component={Paper}>
                <Table className={cls.table}>
                    <SelectOneFlatHeader />
                    <SelectOneFlatBody tableData={tableData} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button startIcon={<AddBoxIcon />} className={cls.add} onClick={addOptionOnClick} color="primary" variant="contained">
                            Add Option
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={tableMeta.valuesAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel position="right" onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableData} properties={["option", "valueMin", "valueMax", "range", "rowOrder"]} />}
        </>
    );
};
