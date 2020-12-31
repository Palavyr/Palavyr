import React, { Dispatch, SetStateAction } from "react";
import { DynamicTableMeta } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableContainer, Paper, Table, Button, FormControlLabel, Checkbox, AccordionActions, makeStyles } from "@material-ui/core";
import { SelectOneFlatHeader } from "./SelectOneFlatHeader";
import { SelectOneFlatBody } from "./SelectOneFlatBody";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { DynamicTableTypes, IDynamicTableProps, SelectOneFlatData, TableData } from "../../DynamicTableTypes";

export interface ISelectOneFlat {
    tableData: Array<SelectOneFlatData>;
    setTableData: Dispatch<SetStateAction<TableData>>;
    areaIdentifier: string;
    tableId: string;
    tableTag: string;
    tableMeta: DynamicTableMeta;
    setTableMeta: any;
    deleteAction(): void;
}

const useStyles = makeStyles({
    tableStyles: {
        width: "100%",
        padding: ".3rem",
        backgroundColor: "transparent",
        borderTop: "1px solid gray",
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
});

export const SelectOneFlat = ({ tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: IDynamicTableProps) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new SelectOneFlatModifier(setTableData);

    const useOptionsAsPathsOnChange = async (event: { target: { checked: boolean } }) => {
        tableMeta.valuesAsPaths = event.target.checked;
        const { data: newTableMeta } = await client.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableMeta);
        setTableMeta(newTableMeta);
    };

    const onSave = async () => {
        const { data } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.SelectOneFlat, tableData, tableId, tableTag);
        setTableData(tableData);
    };

    const addOptionOnClick = () => {
        modifier.addOption(tableData, client, areaIdentifier, tableId);
    };

    return (
        <>
            <TableContainer className={classes.tableStyles} component={Paper}>
                <Table>
                    <SelectOneFlatHeader />
                    <SelectOneFlatBody tableData={tableData} modifier={modifier} />
                </Table>
            </TableContainer>
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addOptionOnClick} color="primary" variant="contained">
                            Add Option
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={tableMeta.valuesAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={() => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
        </>
    );
};
