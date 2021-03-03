import { sortByPropertyNumeric } from '@common/utils/sorting'
import { TableBody } from '@material-ui/core'
import React from 'react'
import { IDynamicTableBody, ThresholdData } from '../../DynamicTableTypes'
import { ThresholdRow } from './ThresholdRow'

export const ThresholdBody = ({tableData, modifier}: IDynamicTableBody) => {

    const getter = (x: ThresholdData) => x.threshold;

    const sortedTableData = sortByPropertyNumeric(getter, tableData);

    return (
        <TableBody>
            {sortedTableData.map((row: ThresholdData, index: number) => {
                const rowId = row.tableId.toString() + index.toString();
                return <ThresholdRow key={rowId} dataIndex={index} tableData={tableData} row={row} modifier={modifier} />
            })}
        </TableBody>
    )
}