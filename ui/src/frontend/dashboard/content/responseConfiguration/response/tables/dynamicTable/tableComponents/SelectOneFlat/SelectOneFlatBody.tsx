import React from "react";
import { makeStyles, TableBody } from "@material-ui/core";
import { SelectOneFlatRow } from "./SelectOneFlatRow";
import { IPricingStrategyBody, CategorySelectTableRowResource } from "@Palavyr-Types";
import { sortByPropertyNumeric } from "@common/utils/sorting";

const useStyles = makeStyles(theme => ({
    body: {
        border: "0px solid black",
        boxShadow: "none",
    },
}));

export const SelectOneFlatBody = ({ tableData, modifier }: IPricingStrategyBody) => {
    const cls = useStyles();
    return (
        <TableBody className={cls.body}>
            {sortByPropertyNumeric((x: CategorySelectTableRowResource) => x.rowOrder, tableData).map((row: CategorySelectTableRowResource, index: number) => {
                const rowId = row.tableId.toString() + index.toString();
                return (
                    <React.Fragment key={rowId}>{row && row.category ? <SelectOneFlatRow key={"row" + rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} /> : <></>}</React.Fragment>
                );
            })}
        </TableBody>
    );
};
