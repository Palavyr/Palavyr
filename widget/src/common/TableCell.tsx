import { makeStyles, TableCell, TableRow } from '@material-ui/core'
import React from 'react'


const useStyles = makeStyles(theme => ({
    cell: {
        border: "0px solid white"
    }
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
    return (
        <TableRow>
            <NoBorderTableCell align={align}>
                {children}
            </NoBorderTableCell>
        </TableRow>
    )
}
