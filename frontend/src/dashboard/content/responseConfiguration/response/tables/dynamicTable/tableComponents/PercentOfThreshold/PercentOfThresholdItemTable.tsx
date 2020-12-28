import { Button, makeStyles, TableBody, TextField } from "@material-ui/core";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import { PercentOfThresholdData } from "../../DynamicTableTypes";
import { PercentOfThresholdHeader } from "./PercentOfThresholdHeader";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdRow } from "./PercentOfThresholdRow";

interface IPercentOfThreshold {
    tableData: PercentOfThresholdData[];
    itemData: PercentOfThresholdData[];
    itemName: string;
    itemId: string;
    modifier: PercentOfThresholdModifier;
    addRowOnClick(): void;
}

const useStyles = makeStyles((theme) => ({
    input: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
}));

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const PercentOfThresholdItemTable = ({ tableData, itemData, itemName, itemId, modifier, addRowOnClick }: IPercentOfThreshold) => {
    const [name, setItemName] = useState<string>("");

    const removeItem = (itemId: string) => {
        modifier.removeItem(tableData, itemId);
    };

    const cls = useStyles();

    useEffect(() => {
        setItemName(itemName);
    }, []);

    return (
        <>
            <TextField
                className={cls.input}
                variant="standard"
                type="text"
                value={itemName}
                color="primary"
                onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                    event.preventDefault();
                    modifier.setItemName(tableData, itemId, event.target.value);
                    setItemName(event.target.value);
                }}
            />
            <PercentOfThresholdHeader />
            <TableBody>
                {itemData.map((data: PercentOfThresholdData, index: number) => {
                    return (
                        <>
                            <PercentOfThresholdRow dataIndex={index} tableData={tableData} row={data} modifier={modifier} />
                        </>
                    );
                })}
            </TableBody>
            <br></br>
            <div style={{marginBottom: "1rem"}}>
                <div style={{float: "left", marginLeft: "1rem"}}>
                    <Button variant="contained" style={{ width: "25ch" }} color="primary" onClick={addRowOnClick}>
                        Add Threshold
                    </Button>
                </div>
                <div style={{float: "right", marginRight: "1rem"}}>
                    <Button variant="contained" style={{ width: "18ch" }} color="primary" onClick={() => removeItem(itemId)}>
                        Delete Item
                    </Button>
                </div>
            </div>
            <br></br>
            <hr></hr>
        </>
    );
};
