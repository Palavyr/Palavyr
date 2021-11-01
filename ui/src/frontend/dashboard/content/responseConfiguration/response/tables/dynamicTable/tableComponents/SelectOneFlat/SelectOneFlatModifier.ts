import { SelectOneFlatData, SetState } from "@Palavyr-Types";
import { cloneDeep } from "lodash";
import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { DynamicTableTypes } from "../../DynamicTableRegistry";

export class SelectOneFlatModifier {
    onClick: SetState<SelectOneFlatData[]>;
    tableType: string;

    constructor(onClick: SetState<SelectOneFlatData[]>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.SelectOneFlat;
    }

    setTables(newState: SelectOneFlatData[]) {
        this.onClick(cloneDeep(newState));
    }

    async addOption(tableData: SelectOneFlatData[], repository: PalavyrRepository, areaIdentifier: string, tableId: string) {
        // this is a difficult situation - we need to allow for an array of objects of various types (dynamic table types)
        const newTableTemplate = await repository.Configuration.Tables.Dynamic.getDynamicTableDataTemplate<SelectOneFlatData>(areaIdentifier, this.tableType, tableId);
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
        return true; // TODO: validation logic
    }
}
