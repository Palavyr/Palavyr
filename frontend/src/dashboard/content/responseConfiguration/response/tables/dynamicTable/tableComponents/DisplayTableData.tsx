import React from "react";
import { TableData } from "../DynamicTableTypes";

export interface IDisplayTableData {
    tableData: TableData;
    properties?: string[];
}

export const DisplayTableData = ({ tableData, properties }: IDisplayTableData) => {
    return (
        <>
            {tableData.map((x: any) => {
                return (
                    <div style={{ fontSize: "14pt" }}>
                        <pre>{JSON.stringify(x, properties, "")}</pre>
                    </div>
                );
            })}
        </>
    );
};
