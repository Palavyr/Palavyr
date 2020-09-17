import React from "react";
import { SelectOneFlatData } from "./SelectOneFlatTypes";
import { TableBody } from "@material-ui/core";
import { SelectOneFlatRow } from "./SelectOneFlatRow";

export interface ISelectOneFlatBody {
    tableData: Array<SelectOneFlatData>;
    modifier: any;
}


export const SelectOneFlatBody = ({ tableData, modifier }: ISelectOneFlatBody) => {

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