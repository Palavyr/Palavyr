import { makeStyles } from '@material-ui/core'
import React from 'react'


interface IAlignCenter {
    children: React.ReactNode;
}

const useStyles = makeStyles(theme => ({
    center: {
        display: "flex",
        justifyContent: "center"
    }
}))

export const AlignCenter = ({ children }: IAlignCenter) => {

    const cls = useStyles();

    return (
        <div className={cls.center}>
            {children}
        </div>
    )
}