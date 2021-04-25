import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, TextField, Paper } from "@material-ui/core";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import { PercentOfThresholdData } from "@Palavyr-Types";
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
    tableStyles: {
        background: "transparent",
    },
}));

const getter = (x: PercentOfThresholdData) => x.rowOrder;

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
            <TableContainer className={cls.tableStyles} component={Paper}>
                <PercentOfThresholdHeader />
                <TableBody>
                    {sortByPropertyNumeric(getter, itemData).map((row: PercentOfThresholdData, index: number) => {
                        row.rowOrder = index;
                        const itemLength = itemData.length;
                        return <PercentOfThresholdRow key={row.rowId} itemData={itemData} itemLength={itemLength} tableData={tableData} row={row} modifier={modifier} baseValue={index === 0 ? true : false} />;
                    })}
                </TableBody>
            </TableContainer>
            <PercentOfThresholdItemToolbar addRowOnClick={addRowOnClick} removeItem={removeItem} itemId={itemId} />
        </>
    );
};

interface IPercentOfThresholdToolbar {
    addRowOnClick(): void;
    removeItem(itemId: string): void;
    itemId: string;
}

export const PercentOfThresholdItemToolbar = ({ addRowOnClick, removeItem, itemId }: IPercentOfThresholdToolbar) => {
    return (
        <>
            <br></br>
            <div style={{ marginBottom: "1rem" }}>
                <div style={{ float: "left", marginLeft: "1rem" }}>
                    <Button variant="contained" style={{ width: "25ch" }} color="primary" onClick={addRowOnClick}>
                        Add Threshold
                    </Button>
                </div>
                <div style={{ float: "right", marginRight: "1rem" }}>
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
