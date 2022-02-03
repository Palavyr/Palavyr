import React, { useContext } from "react";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { AccordionActions, Button, makeStyles } from "@material-ui/core";
import { DynamicTableProps, TwoNestedCategoryData } from "@Palavyr-Types";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { TwoNestedCategoriesContainer } from "./TwoNestedCategoriesContainer";
import { DisplayTableData } from "../DisplayTableData";
import { DynamicTableTypes } from "../../DynamicTableRegistry";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";

const useStyles = makeStyles(theme => ({
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

export const TwoNestedCategories = ({ tableId, tableTag, tableMeta, tableRows, setTableRows, areaIdentifier, deleteAction, showDebug }: Omit<DynamicTableProps, "setTableMeta">) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const modifier = new TwoNestedCategoriesModifier(setTableRows);

    const addOuterCategory = () => modifier.addOuterCategory(tableRows, repository, areaIdentifier, tableId);
    const addInnerCategory = () => modifier.addInnerCategory(tableRows, repository, areaIdentifier, tableId);

    const onSave = async () => {
        const result = modifier.validateTable(tableRows);

        if (result) {
            const savedData = await repository.Configuration.Tables.Dynamic.saveDynamicTable<TwoNestedCategoryData[]>(areaIdentifier, DynamicTableTypes.TwoNestedCategory, tableRows, tableId, tableTag);
            setTableRows(savedData);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            <TwoNestedCategoriesContainer addInnerCategory={addInnerCategory} tableData={tableRows} modifier={modifier} />
            <AccordionActions>
                <div className={cls.trayWrapper}>
                    <div className={cls.alignLeft}>
                        <Button className={cls.add} onClick={addOuterCategory} color="primary" variant="contained">
                            Add Outer Category
                        </Button>
                    </div>
                    <div className={cls.alignRight}>
                        <SaveOrCancel onDelete={deleteAction} onSave={onSave} onCancel={async () => window.location.reload()} />
                    </div>
                </div>
            </AccordionActions>
            {showDebug && <DisplayTableData tableData={tableRows} properties={["category", "subCategory"]} />}
        </>
    );
};
