import React from "react";
import { TableBody } from "@material-ui/core";
import { SelectOneFlatRow } from "./SelectOneFlatRow";
import { IDynamicTableBody, SelectOneFlatData } from "@Palavyr-Types";

export const SelectOneFlatBody = ({ tableData, modifier }: IDynamicTableBody) => {
    return (
        <TableBody>
            {tableData.map((row: SelectOneFlatData, index: number) => {
                const rowId = row.tableId.toString() + index.toString();
                return <SelectOneFlatRow key={rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} />;
            })}
        </TableBody>
    );
};
