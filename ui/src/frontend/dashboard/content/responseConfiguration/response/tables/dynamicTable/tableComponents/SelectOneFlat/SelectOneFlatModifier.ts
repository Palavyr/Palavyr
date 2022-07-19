import { Modifier, SelectOneFlatData, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PricingStrategyTypes } from "../../PricingStrategyRegistry";

export class SelectOneFlatModifier implements Modifier {
    onClick: SetState<SelectOneFlatData[]>;
    tableType: string;

    constructor(onClick: SetState<SelectOneFlatData[]>) {
        this.onClick = onClick;
        this.tableType = PricingStrategyTypes.SelectOneFlat;
    }

    setTables(newState: SelectOneFlatData[]) {
        this.onClick(cloneDeep(newState));
    }

    async addOption(tableData: SelectOneFlatData[], repository: PalavyrRepository, intentId: string, tableId: string) {
        // this is a difficult situation - we need to allow for an array of objects of various types (dynamic table types)
        const newTableTemplate = await repository.Configuration.Tables.Dynamic.GetPricingStrategyDataTemplate<SelectOneFlatData>(intentId, this.tableType, tableId);
        newTableTemplate.rowOrder = tableData.length;
        tableData.push(newTableTemplate);
        this.setTables(tableData);
    }

    removeOption(tableData: SelectOneFlatData[], dataIndex: number) {
        const newRows: SelectOneFlatData[] = [];
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

    setOptionText(tableData: SelectOneFlatData[], index: number, newText: string) {
        tableData[index].option = newText;
        this.setTables(tableData);
    }

    setOptionValue(tableData: SelectOneFlatData[], index: number, newValue: number) {
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setOptionMaxValue(tableData: SelectOneFlatData[], index: number, newValue: number) {
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: SelectOneFlatData[], index: number) {
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }

    validateTable(tableData: SelectOneFlatData[]) {
        const tableRows = tableData;
        const isValid = true;

        return { isValid, tableRows };
    }
}
