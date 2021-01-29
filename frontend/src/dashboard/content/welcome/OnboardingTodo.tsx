import { makeStyles } from '@material-ui/core'
import React from 'react'

const useStyles = makeStyles(theme => ({
    list: {
        textAlign: "left"
    }
}))

export const OnboardingTodo = () => {

    const cls = useStyles();
    return (
        <ul className={cls.list}>
            <li>First thing</li>
            <li>Second thing</li>
        </ul>
    )
}