import { sortByPropertyNumeric } from "@common/utils/sorting";
import { TableBody } from "@material-ui/core";
import { BasicThresholdData, IDynamicTableBody } from "@Palavyr-Types";
import React from "react";
import { BasicThresholdRow } from "./BasicThresholdRow";

const getter = (x: BasicThresholdData) => x.rowOrder;
export const BasicThresholdBody = ({ tableData, modifier, unitGroup, unitPrettyName }: IDynamicTableBody) => {
    return (
        <TableBody>
            {sortByPropertyNumeric(getter, tableData).map((row: BasicThresholdData, rowIndex: number) => {
                row.rowOrder = rowIndex;
                return (
                    <>
                        {unitGroup && unitPrettyName ? (
                            <BasicThresholdRow key={row.rowId} rowIndex={rowIndex} tableData={tableData} row={row} modifier={modifier} unitGroup={unitGroup} unitPrettyName={unitPrettyName} />
                        ) : (
                            <></>
                        )}
                    </>
                );
            })}
        </TableBody>
    );
};
