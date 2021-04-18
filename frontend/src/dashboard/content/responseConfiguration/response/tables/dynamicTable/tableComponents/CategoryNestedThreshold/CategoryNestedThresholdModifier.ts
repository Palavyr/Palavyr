import { ApiClient } from "@api-client/Client";
import { SetState, TableGroup } from "@Palavyr-Types";
import { cloneDeep, groupBy } from "lodash";
import { CategoryNestedThresholdData, DynamicTableTypes, TableData } from "../../DynamicTableTypes";


export class CategoryNestedThresholdModifier {
    onClick: SetState<TableData>;
    tableType: string = DynamicTableTypes.TwoNestedCategory;

    constructor(onClick: SetState<TableData>) {
        this.onClick = onClick;
    }

    setTables(newState: CategoryNestedThresholdData[]) {
        this.onClick(cloneDeep(newState));
    }

    groupByOuterCategory(tableData: CategoryNestedThresholdData[]): TableGroup<CategoryNestedThresholdData[]> {
        return groupBy(tableData, (x) => x.itemId);
    }

    async addCategory(tableData: CategoryNestedThresholdData[], client: ApiClient, areaIdentifier: string, tableId: string) {
        let { data: template } = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        template = template as CategoryNestedThresholdData;

        const newTableData = [...tableData, ...template];
        this.setTables(newTableData);
    }

    async addThreshold(tableData: CategoryNestedThresholdData[], categoryIndex: number, client: ApiClient, areaIdentifier: string, tableId: string) {
        // TODO: Fix this up
        let { data: template } = await client.Configuration.Tables.Dynamic.getDynamicTableDataTemplate(areaIdentifier, this.tableType, tableId);
        template = template as CategoryNestedThresholdData;

        tableData.push(template);

        this.setTables(cloneDeep(tableData));
    }


    removeCategory(tableData: CategoryNestedThresholdData[], categoryId: string) {
        this.setTables(tableData);
    }

    setCategoryName(tableData: CategoryNestedThresholdData[], categoryId: string, value: string){
        this.setTables(tableData);
    }

    setThreshold(tableData: CategoryNestedThresholdData[], rowId: string, value: number){
        this.setTables(tableData);
    }

    setValueMin(tableData: CategoryNestedThresholdData[], rowId: string, value: number) {
        this.setTables(tableData);
    }

    setValueMax(tableData: CategoryNestedThresholdData[], rowId: string, value: number) {
        this.setTables(tableData);
    }

    setRangeOrValue(tableData: CategoryNestedThresholdData[], rowId: string) {
        this.setTables(tableData);
    }

    removeThreshold(tableData: CategoryNestedThresholdData[], rowId: string) {
        this.setTables(tableData);
    }
}