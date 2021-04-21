import { isDevelopmentStage } from "@api-client/clientUtils";
import { TableData } from "@Palavyr-Types";
import React from "react";

export interface IDisplayTableData {
    tableData: TableData;
    properties?: string[];
}

export const DisplayTableData = ({ tableData, properties }: IDisplayTableData) => {
    return (
        <>
            {isDevelopmentStage() && tableData && tableData.map((x: any) => {
                return (
                    <div style={{ fontSize: "14pt" }}>
                        <pre>{JSON.stringify(x, properties, "")}</pre>
                    </div>
                );
            })}
        </>
    );
};
