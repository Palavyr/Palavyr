import { sortByPropertyNumeric } from '@common/utils/sorting'
import { TableBody } from '@material-ui/core'
import { BasicThresholdData, IDynamicTableBody } from '@Palavyr-Types';
import React from 'react'
import { BasicThresholdRow } from './BasicThresholdRow'

const getter = (x: BasicThresholdData) => x.rowOrder;
export const BasicThresholdBody = ({tableData, modifier}: IDynamicTableBody) => {

    return (
        <TableBody>
            {sortByPropertyNumeric(getter, tableData).map((row: BasicThresholdData, rowIndex: number) => {
                return <BasicThresholdRow key={row.rowId} rowIndex={rowIndex} tableData={tableData} row={row} modifier={modifier} />
            })}
        </TableBody>
    )
}