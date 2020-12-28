import { makeStyles, TableRow } from "@material-ui/core";
import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, PercentOfThresholdData } from "../../DynamicTableTypes";
import { PercentOfThresholdItemTable } from "./PercentOfThresholdItemTable";

interface IPercentOfThresholdContainer extends IDynamicTableBody {
    addRowOnClickFactory(itemId: string): () => void;
}

type TableGroup = {
    [itemGroup: string]: PercentOfThresholdData[];
};

const useStyles = makeStyles((theme) => ({
    root: {
        borderTop: "3px solid red",
    },
    tableStyles: {},
    trayWrapper: {},
    add: {},
    alignLeft: {},
    alignRight: {},
}));

export const PercentOfThresholdContainer = ({ tableData, modifier, addRowOnClickFactory }: IPercentOfThresholdContainer) => {
    const tableGroups: TableGroup = groupBy(tableData, (x) => x.itemId);
    const cls = useStyles();

    return (
        <>
            {Object.keys(tableGroups).map((itemId: string, index: number) => {
                const itemData: PercentOfThresholdData[] = tableGroups[itemId];

                return (
                    <>
                        <TableRow classes={{ root: cls.root }}></TableRow>
                        <PercentOfThresholdItemTable
                            key={index}
                            tableData={tableData}
                            itemId={itemId}
                            itemData={itemData}
                            itemName={itemData[0].itemName} // TODO: is there a better way to get this?
                            modifier={modifier}
                            addRowOnClick={addRowOnClickFactory(itemId)}
                        />
                        <TableRow style={{ borderTop: "3px solid red" }}></TableRow>
                    </>
                );
            })}
        </>
    );
};
