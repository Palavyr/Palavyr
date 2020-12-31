import React from "react";
import { TableBody } from "@material-ui/core";
import { SelectOneFlatRow } from "./SelectOneFlatRow";
import { IDynamicTableBody } from "../../DynamicTableTypes";


export const SelectOneFlatBody = ({ tableData, modifier }: IDynamicTableBody) => {

    return (
        <TableBody>
            {
                tableData.map((row, index) => {
                    var rowId = row.tableId.toString() + index.toString();
                    return <SelectOneFlatRow key={rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} />
                })
            }
        </TableBody>
    )
}