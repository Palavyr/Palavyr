import * as React from 'react';

import { makeStyles } from '@material-ui/core';
import { SimpleIconTypes } from './IconTypes';
import Battery from "../../common/svgs/misc/battery.svg";

export interface IBrandingIcon {
    iconType: SimpleIconTypes;
    iconColor?: string;
    iconSize?: number;
}

const useStyles = makeStyles(theme => ({
    icon: {
        display: "flex",
        justifyContent: "space-between",
        textAlign: "center",
        marginBottom: "1.2rem",
        width: "100%",
    }
}))


export const BrandingIcon = ({ iconType, iconColor, iconSize}: IBrandingIcon) => {

    const classes = useStyles();
    const style = {
        color: `${iconColor ?? "blue"}`,
        fontSize: iconSize ?? 24
    }
    return (
        <div className={classes.icon}>
            <div style={{width: "25%"}}></div>
            <Battery />
            <div style={{width: "25%"}}></div>
        </div>
    )
}
