import { groupBy } from "lodash";
import React from "react";
import { IDynamicTableBody, PercentOfThresholdData } from "@Palavyr-Types";
import { PercentOfThresholdItemTable } from "./PercentOfThresholdItemTable";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    container: {
        borderTop: `4px solid ${theme.palette.primary.main}`,
        borderBottom: `4px solid ${theme.palette.primary.main}`,
    },
}));
interface IPercentOfThresholdContainer extends IDynamicTableBody {
    addRowOnClickFactory(itemId: string): () => void;
}

type TableGroup = {
    [itemGroup: string]: PercentOfThresholdData[];
};

export const PercentOfThresholdContainer = ({ tableData, modifier, addRowOnClickFactory }: IPercentOfThresholdContainer) => {
    const tableGroups: TableGroup = groupBy(tableData, x => x.itemId);
    const cls = useStyles();
    return (
        <div className={cls.container}>
            {Object.keys(tableGroups).map((itemId: string, index: number) => {
                const itemData: PercentOfThresholdData[] = tableGroups[itemId];

                return (
                    <PercentOfThresholdItemTable
                        key={index}
                        tableData={tableData}
                        itemId={itemId}
                        itemData={itemData}
                        itemName={itemData[0].itemName} // TODO: is there a better way to get this?
                        modifier={modifier}
                        addRowOnClick={addRowOnClickFactory(itemId)}
                    />
                );
            })}
        </div>
    );
};
