import React from "react";
import { makeStyles, TableBody } from "@material-ui/core";
import { CategorySelectRow } from "./CategorySelectRow";
import { IPricingStrategyBody, CategorySelectTableRowResource } from "@Palavyr-Types";
import { sortByPropertyNumeric } from "@common/utils/sorting";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    body: {
        border: "0px solid black",
        boxShadow: "none",
    },
}));

export const CategorySelectBody = ({ tableData, modifier }: IPricingStrategyBody) => {
    const cls = useStyles();
    return (
        <TableBody className={cls.body}>
            {sortByPropertyNumeric((x: CategorySelectTableRowResource) => x.rowOrder, tableData).map((row: CategorySelectTableRowResource, index: number) => {
                const rowId = row.tableId.toString() + index.toString();
                return (
                    <React.Fragment key={rowId}>{row && row.category ? <CategorySelectRow key={"row" + rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} /> : <></>}</React.Fragment>
                );
            })}
        </TableBody>
    );
};
