import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, Checkbox, FormControlLabel, makeStyles, Paper, Table, TableContainer, TableRow } from "@material-ui/core";
import { groupBy } from "lodash";
import React from "react";
import { DynamicTableTypes, IDynamicTableBody, IDynamicTableProps, PercentOfThresholdData } from "../../DynamicTableTypes";
import { PercentOfThresholdItemTable } from "./PercentOfThresholdItemTable";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";

const useStyles = makeStyles((theme) => ({
    root: {
        borderTop: "3px solid red",
    },
    tableStyles: {},
    trayWrapper: {},
    add: {},
    alignLeft: {},
    alignRight: {},
}));

interface IPercentOfThresholdContainer extends IDynamicTableBody {
    addRowOnClickFactory(itemId: string): () => void;
}

type TableGroup = {
    [itemGroup: string]: PercentOfThresholdData[];
}

export const PercentOfThresholdContainer = ({ tableData, modifier, addRowOnClickFactory }: IPercentOfThresholdContainer) => {
    const tableGroups: TableGroup = groupBy(tableData, (x) => x.itemId);
    const cls = useStyles();

    return (
        <>
            {Object.keys(tableGroups).map((itemId: string, index: number) => {
                const itemData: PercentOfThresholdData[] = tableGroups[itemId];

                return (
                    <>
                        <TableRow classes={{ root: cls.root }}></TableRow>
                        <PercentOfThresholdItemTable
                            key={index}
                            tableData={tableData}
                            itemData={itemData}
                            itemName={itemData[0].itemName} // TODO: is there a better way to get this?
                            modifier={modifier}
                            addRowOnClick={addRowOnClickFactory(itemId)}
                        />
                        <TableRow style={{ borderTop: "3px solid red" }}></TableRow>
                    </>
                );
            })}
        </>
    );
};

export const PercentOfThreshold = ({ tableMeta, setTableMeta, tableId, tableTag, tableData, setTableData, areaIdentifier, deleteAction }: IDynamicTableProps) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new PercentOfThresholdModifier(setTableData);

    const addItemOnClick = () => {
        modifier.addItem(tableData, client, areaIdentifier, tableId);
    };

    const addRowOnClickFactory = (itemId: string) => () => {
        modifier.addRow(tableData, client, areaIdentifier, tableId, itemId);
    };

    const onSave = async () => {
        const { data } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.PercentOfThreshold, tableData, tableId, tableTag);
        setTableData(tableData);
    };

    const useOptionsAsPathsOnChange = () => {
        return null;
    };

    return (
        <>
            <TableContainer className={classes.tableStyles} component={Paper}>
                <PercentOfThresholdContainer tableData={tableData} modifier={modifier} addRowOnClickFactory={addRowOnClickFactory} />
            </TableContainer>
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Item
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
