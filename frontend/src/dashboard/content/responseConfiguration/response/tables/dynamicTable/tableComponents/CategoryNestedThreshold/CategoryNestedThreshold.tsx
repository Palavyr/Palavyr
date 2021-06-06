import React from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { CategoryNestedThresholdData, DynamicTableProps } from "@Palavyr-Types";

import { DisplayTableData } from "../DisplayTableData";
import { CategoryNestedThresholdContainer } from "./CategoryNestedThresholdContainer";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

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

export const CategoryNestedThreshold = ({ tableId, tableTag, tableMeta, tableData, setTableData, areaIdentifier, deleteAction, showDebug }: Omit<DynamicTableProps, "setTableMeta">) => {
    const repository = new PalavyrRepository();
    const classes = useStyles();

    const modifier = new CategoryNestedThresholdModifier(setTableData);

    const onSave = async () => {
        const result = modifier.validateTable(tableData);

        if (result) {
            const savedData = await repository.Configuration.Tables.Dynamic.saveDynamicTable<CategoryNestedThresholdData[]>(areaIdentifier, DynamicTableTypes.CategoryNestedThreshold, tableData, tableId, tableTag);
            setTableData(savedData);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            <CategoryNestedThresholdContainer tableData={tableData} modifier={modifier} tableId={tableId} areaIdentifier={areaIdentifier} />
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={() => modifier.addCategory(tableData, repository, areaIdentifier, tableId)} color="primary" variant="contained">
                            Add Category
                        </Button>
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableData} properties={["category", "triggerFallback", "threshold", "valueMin", "valueMax", "rowOrder", "rowId", "itemOrder", "itemId"]} />}
        </>
    );
};
