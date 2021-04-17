import { makeStyles, TableCell, TableRow } from '@material-ui/core'
import React from 'react'


const useStyles = makeStyles(theme => ({
    row: {
        width: "100%"
    },
    cell: {
        border: "0px solid white",
        width: "100%"
    },
}))

export interface IHaveNoBorder {
    align?: "left"| "right" | "center";
    children: React.ReactNode;
}
export const NoBorderTableCell = ({align, children}: IHaveNoBorder) => {
    const cls = useStyles();
    return (
        <TableCell align={align} className={cls.cell}>
            {children}
        </TableCell>
    )
}


export const SingleRowSingleCell = ({align, children}: IHaveNoBorder) => {
    const cls = useStyles();
    return (
        <TableRow className={cls.row}>
            <NoBorderTableCell align={align}>
                {children}
            </NoBorderTableCell>
        </TableRow>
    )
}
