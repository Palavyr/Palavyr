import React from "react";
import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, Checkbox, FormControlLabel, makeStyles } from "@material-ui/core";
import { DynamicTableTypes, IDynamicTableProps } from "../../DynamicTableTypes";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesContainer } from "./TwoNestedCategoriesContainer";

const useStyles = makeStyles((theme) => ({
    root: {
        borderTop: "3px solid red",
    },
    tableStyles: {
        background: "transparent",
    },
    trayWrapper: {
        width: "100%",
    },
    add: {},
    alignLeft: {
        float: "left",
    },
    alignRight: {
        float: "right",
    },
}));

export const TwoCategoriesNested = ({ tableId, tableTag, tableMeta, tableData, setTableData, areaIdentifier, deleteAction }: Omit<IDynamicTableProps, "setTableMeta">) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new TwoNestedCategoriesModifier(setTableData);

    const addItemOnClick = () => modifier.addItem(tableData, client, areaIdentifier, tableId);
    const addRowOnClickFactory = (itemId: string) => () => modifier.addRow(tableData, client, areaIdentifier, tableId, itemId);

    const onSave = async () => {
        // const reorderedData = reOrderPercentOfThresholdTableData(tableData);
        // const { data: savedData } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.TwoNestedCategory, reorderedData, tableId, tableTag);
        // setTableData(savedData);
        // return true;
        return true;
    };


    const useOptionsAsPathsOnChange = async (event: { target: { checked: boolean } }) => {
        // tableMeta.valuesAsPaths = event.target.checked;
        // const { data: newTableMeta } = await client.Configuration.Tables.Dynamic.modifyDynamicTableMeta(tableMeta);
        // setTableMeta(newTableMeta);
    };

    return (
        <>
            <TwoNestedCategoriesContainer tableData={tableData} modifier={modifier} addRowOnClickFactory={addRowOnClickFactory} />
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addItemOnClick} color="primary" variant="contained">
                            Add Category
                        </Button>
                        <FormControlLabel label="Use Options as Paths" control={<Checkbox checked={tableMeta.valuesAsPaths} onChange={useOptionsAsPathsOnChange} />} />
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
        </>
    );
};
