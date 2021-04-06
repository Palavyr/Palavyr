import React from "react";
import { ApiClient } from "@api-client/Client";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableTypes, IDynamicTableProps } from "../../DynamicTableTypes";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesContainer } from "./TwoNestedCategoriesContainer";
import { DisplayTableData } from "../DisplayTableData";

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

export const TwoNestedCategories = ({ tableId, tableTag, tableMeta, tableData, setTableData, areaIdentifier, deleteAction }: Omit<IDynamicTableProps, "setTableMeta">) => {
    const client = new ApiClient();
    const classes = useStyles();

    const modifier = new TwoNestedCategoriesModifier(setTableData);

    const addOuterCategory = () => modifier.addOuterCategory(tableData, client, areaIdentifier, tableId);
    const addInnerCategory = () => modifier.addInnerCategory(tableData, client, areaIdentifier, tableId);

    const onSave = async () => {
        const { data: savedData } = await client.Configuration.Tables.Dynamic.saveDynamicTable(areaIdentifier, DynamicTableTypes.TwoNestedCategory, tableData, tableId, tableTag);
        setTableData(savedData);
        return true;
    };

    return (
        <>
            <TwoNestedCategoriesContainer addInnerCategory={addInnerCategory} tableData={tableData} modifier={modifier} />
            <AccordionActions>
                <div className={classes.trayWrapper}>
                    <div className={classes.alignLeft}>
                        <Button className={classes.add} onClick={addOuterCategory} color="primary" variant="contained">
                            Add Outer Category
                        </Button>
                    </div>
                    <div className={classes.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            <DisplayTableData tableData={tableData} properties={["category", "subCategory"]} />
        </>
    );
};
