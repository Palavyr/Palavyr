import { Button, TableBody } from "@material-ui/core";
import React from "react";
import { PercentOfThresholdData } from "../../DynamicTableTypes";
import { PercentOfThresholdHeader } from "./PercentOfThresholdHeader";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdRow } from "./PercentOfThresholdRow";

interface IPercentOfThreshold {
    tableData: PercentOfThresholdData[];
    itemData: PercentOfThresholdData[];
    itemName: string;
    modifier: PercentOfThresholdModifier;
    addRowOnClick(): void;
}

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const PercentOfThresholdItemTable = ({ tableData, itemData, itemName, modifier, addRowOnClick}: IPercentOfThreshold) => {

    return (
        <>
            <PercentOfThresholdHeader />
            <TableBody>
                <div>{itemName}</div>
                {itemData.map((data: PercentOfThresholdData, index: number) => {
                    return (
                        <>
                            <PercentOfThresholdRow dataIndex={index} tableData={tableData} row={data} modifier={modifier} />
                        </>
                    );
                })}
            </TableBody>
            <br></br>
            <Button
                variant="contained"
                style={{ width: "18ch" }}
                color="primary"
                onClick={addRowOnClick}
            >
                Add Row
            </Button>
            <br></br>
            <hr></hr>
            <br></br>
        </>
    );
};
