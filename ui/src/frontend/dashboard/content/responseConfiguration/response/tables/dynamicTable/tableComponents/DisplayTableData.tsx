import { isDevelopmentStage } from "@common/client/clientUtils";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TableData } from "@Palavyr-Types";
import React from "react";

export interface IDisplayTableData {
    tableData: TableData;
    properties?: string[];
}

export const DisplayTableData = ({ tableData, properties }: IDisplayTableData) => {
    return (
        <>
            {isDevelopmentStage() &&
                tableData &&
                tableData.map((x: any, index: number) => {
                    return (
                        <div key={index} style={{ position: "inherit",  fontSize: "14pt" }}>
                            <PalavyrText noWrap component="pre">
                                {JSON.stringify(x, properties, "")}
                            </PalavyrText>
                        </div>
                    );
                })}
        </>
    );
};
