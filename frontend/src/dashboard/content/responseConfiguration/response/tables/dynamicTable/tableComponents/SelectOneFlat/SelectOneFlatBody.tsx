import React from "react";
import { TableBody } from "@material-ui/core";
import { SelectOneFlatRow } from "./SelectOneFlatRow";
import { IDynamicTableBody, SelectOneFlatData } from "@Palavyr-Types";
import { sortByPropertyNumeric } from "@common/utils/sorting";

export const SelectOneFlatBody = ({ tableData, modifier }: IDynamicTableBody) => {
    return (
        <TableBody>
            {sortByPropertyNumeric((x: SelectOneFlatData) => x.rowOrder, tableData).map((row: SelectOneFlatData, index: number) => {
                const rowId = row.tableId.toString() + index.toString();
                return <SelectOneFlatRow key={rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} />;
            })}
        </TableBody>
    );
};
