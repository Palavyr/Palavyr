import { Modifier, CategorySelectTableRowResource, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";

export class CategorySelectModifier implements Modifier {
    onClick: SetState<CategorySelectTableRowResource[]>;
    tableType: string;

    constructor(onClick: SetState<CategorySelectTableRowResource[]>) {
        this.onClick = onClick;
        this.tableType = PricingStrategyTypes.CategorySelect;
    }

    setTables(newState: CategorySelectTableRowResource[]) {
        this.onClick(cloneDeep(newState));
    }

    async addOption(tableData: CategorySelectTableRowResource[], repository: PalavyrRepository, intentId: string, tableId: string) {
        // this is a difficult situation - we need to allow for an array of objects of various types (dynamic table types)
        const newTableTemplate = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<CategorySelectTableRowResource>(intentId, this.tableType, tableId);
        newTableTemplate.rowOrder = tableData.length;
        tableData.push(newTableTemplate);
        this.setTables(tableData);
    }

    removeOption(tableData: CategorySelectTableRowResource[], dataIndex: number) {
        const newRows: CategorySelectTableRowResource[] = [];
        if (tableData.length > 1) {
            let counter = 0;
            tableData.forEach((row, index: number) => {
                if (index !== dataIndex) {
                    row.rowOrder = counter;
                    newRows.push(row);
                    counter++;
                }
            });
            this.setTables(newRows);
        } else {
            alert("Table must have at least one option");
        }
    }

    setOptionText(tableData: CategorySelectTableRowResource[], index: number, newText: string) {
        tableData[index].category = newText;
        this.setTables(tableData);
    }

    setOptionValue(tableData: CategorySelectTableRowResource[], index: number, newValue: number) {
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setOptionMaxValue(tableData: CategorySelectTableRowResource[], index: number, newValue: number) {
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: CategorySelectTableRowResource[], index: number) {
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    validateTable(tableData: CategorySelectTableRowResource[]) {
        const tableRows = tableData;
        const isValid = true;

        return { isValid, tableRows };
    }
}
