import { Dispatch, SetStateAction } from "react";
import { TableData, SelectOneFlatData } from "./SelectOneFlatTypes";
import { DynamicTableTypes } from "../../DynamicTableTypes";
import { cloneDeep } from "lodash";
import { ApiClient } from "@api-client/Client";


export class SelectOneFlatModifier {

    onClick: Dispatch<SetStateAction<TableData>>;
    tableType: string;

    constructor(onClick: Dispatch<SetStateAction<TableData>>) {
        this.onClick = onClick;
        this.tableType = DynamicTableTypes.SelectOneFlat;
    }

    setTables(newState: TableData) {
        this.onClick(cloneDeep(newState));
    }

    async addOption(tableData: TableData, client: ApiClient, areaIdentifier: string, tableId: string) {

        const newTableTemplate = (await client.Configuration.Tables.Dynamic.getDynamicTableDataTempate(areaIdentifier, this.tableType, tableId)).data as SelectOneFlatData;
        tableData.push(newTableTemplate);
        this.setTables(tableData);
    }

    removeOption(tableData: TableData, dataIndex: number,) {
        const newRows: TableData = [];
        if (tableData.length > 1) {
            tableData.forEach((row, index) => {
                if (index !== dataIndex) {
                    newRows.push(row);
                }
            })
            this.setTables(newRows);
        } else {
            alert("Table must have at least 1 Option")
        }

    }

    setOptionText(tableData: TableData, index: number, newText: string) {
        tableData[index].option = newText;
        this.setTables(tableData);
    }

    setOptionValue(tableData: TableData, index: number, newValue: number) {
        tableData[index].valueMin = newValue;
        this.setTables(tableData);
    }

    setOptionMaxValue(tableData: TableData, index: number, newValue: number) {
        tableData[index].valueMax = newValue;
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: TableData, index: number) {
        tableData[index].range = !tableData[index].range;
        this.setTables(tableData);
    }
}