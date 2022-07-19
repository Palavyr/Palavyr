import React from "react";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { TableBody } from "@material-ui/core";
import { BasicThresholdResource, IPricingStrategyBody } from "@Palavyr-Types";
import { BasicThresholdRow } from "./BasicThresholdRow";

const getter = (x: BasicThresholdResource) => x.rowOrder;
export const BasicThresholdBody = ({ tableData, modifier, unitGroup, unitPrettyName }: IPricingStrategyBody) => {
    return (
        <TableBody>
            {sortByPropertyNumeric(getter, tableData).map((row: BasicThresholdResource, rowIndex: number) => {
                row.rowOrder = rowIndex;
                return (
                    <React.Fragment key={rowIndex}>
                        {unitGroup && unitPrettyName ? (
                            <BasicThresholdRow key={row.rowId} rowIndex={rowIndex} tableData={tableData} row={row} modifier={modifier} unitGroup={unitGroup} unitPrettyName={unitPrettyName} />
                        ) : (
                            <></>
                        )}
                    </React.Fragment>
                );
            })}
        </TableBody>
    );
};
