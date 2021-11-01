import React, { useContext } from "react";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
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

export const TwoNestedCategories = ({ tableId, tableTag, tableMeta, tableData, setTableData, areaIdentifier, deleteAction, showDebug }: Omit<DynamicTableProps, "setTableMeta">) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const modifier = new TwoNestedCategoriesModifier(setTableData);

    const addOuterCategory = () => modifier.addOuterCategory(tableData, repository, areaIdentifier, tableId);
    const addInnerCategory = () => modifier.addInnerCategory(tableData, repository, areaIdentifier, tableId);

    const onSave = async () => {
        const result = modifier.validateTable(tableData);

        if (result) {
            const savedData = await repository.Configuration.Tables.Dynamic.saveDynamicTable<TwoNestedCategoryData[]>(areaIdentifier, DynamicTableTypes.TwoNestedCategory, tableData, tableId, tableTag);
            setTableData(savedData);
            return true;
        } else {
            return false;
        }
    };

    return (
        <>
            <TwoNestedCategoriesContainer addInnerCategory={addInnerCategory} tableData={tableData} modifier={modifier} />
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
            {showDebug && <DisplayTableData tableData={tableData} properties={["category", "subCategory"]} />}
        </>
    );
};
