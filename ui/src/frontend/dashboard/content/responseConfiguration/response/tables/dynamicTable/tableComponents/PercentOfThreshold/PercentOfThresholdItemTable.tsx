import { sortByPropertyNumeric } from "@common/utils/sorting";
import { Button, makeStyles, TableBody, TableContainer, Paper, Table } from "@material-ui/core";
import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import { PercentOfThresholdData, UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { PercentOfThresholdHeader } from "./PercentOfThresholdHeader";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { PercentOfThresholdRow } from "./PercentOfThresholdRow";
import { TextInput } from "@common/components/TextField/TextInput";
import { Align } from "@common/positioning/Align";
import { ButtonBar } from "../../components/SaveBar";

interface IPercentOfThreshold {
    tableData: PercentOfThresholdData[];
    itemData: PercentOfThresholdData[];
    itemName: string;
    itemId: string;
    modifier: PercentOfThresholdModifier;
    addRowOnClick(): void;
    unitGroup?: UnitGroups;
    unitPrettyName?: UnitPrettyNames;
}

const useStyles = makeStyles(theme => ({
    input: {
        margin: "0.6rem",
        width: "50ch",
        paddingLeft: "0.4rem",
    },
    inputPropsCls: {
        paddingLeft: "0.4rem",
    },
    tableStyles: {
        background: "transparent",
        boxShadow: "none",
        paddingBottom: "0.6rem",
    },
}));

const getter = (x: PercentOfThresholdData) => x.rowOrder;

// table data: to update the database (this is done via the unified table data object)
// item data: The grouped data that is used to render and control UI
export const PercentOfThresholdItemTable = ({ tableData, itemData, itemName, itemId, modifier, addRowOnClick, unitGroup, unitPrettyName }: IPercentOfThreshold) => {
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
            <Align direction="flex-start">
                <TextInput
                    className={cls.input}
                    label="Name to use in PDF fee table"
                    variant="standard"
                    type="text"
                    value={itemName || ""}
                    InputLabelProps={{ className: cls.inputPropsCls }}
                    onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                        event.preventDefault();
                        modifier.setItemName(tableData, itemId, event.target.value);
                        setItemName(event.target.value);
                    }}
                />
            </Align>
            <Table>
                <PercentOfThresholdHeader tableData={tableData} modifier={modifier} />
                <TableBody className={cls.tableStyles}>
                    {sortByPropertyNumeric(getter, itemData).map((row: PercentOfThresholdData, index: number) => {
                        row.rowOrder = index;
                        const itemLength = itemData.length;
                        return (
                            <React.Fragment key={index}>
                                {unitGroup && unitPrettyName && row && row.threshold ? (
                                    <PercentOfThresholdRow
                                        unitGroup={unitGroup}
                                        unitPrettyName={unitPrettyName}
                                        key={row.rowId}
                                        itemData={itemData}
                                        itemLength={itemLength}
                                        tableData={tableData}
                                        row={row}
                                        modifier={modifier}
                                        baseValue={index === 0 ? true : false}
                                    />
                                ) : (
                                    <></>
                                )}
                            </React.Fragment>
                        );
                    })}
                </TableBody>
            </Table>
            <ButtonBar
                addInnerButton={
                    <Button variant="contained" style={{ width: "25ch" }} color="primary" onClick={addRowOnClick}>
                        Add Threshold
                    </Button>
                }
                deleteButton={
                    <Button variant="contained" style={{ width: "18ch" }} color="primary" onClick={() => removeItem(itemId)}>
                        Delete Item
                    </Button>
                }
            />
        </>
    );
};
