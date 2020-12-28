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
    tableStyles: {
        backgroundColor: "transparent",
        marginBottom: "3rem"
    },
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
                    <div className={cls.tableStyles}>
                        <PercentOfThresholdItemTable
                            key={index}
                            tableData={tableData}
                            itemId={itemId}
                            itemData={itemData}
                            itemName={itemData[0].itemName} // TODO: is there a better way to get this?
                            modifier={modifier}
                            addRowOnClick={addRowOnClickFactory(itemId)}
                        />
                    </div>
                );
            })}
        </>
    );
};
