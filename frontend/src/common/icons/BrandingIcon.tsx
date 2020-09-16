import * as React from 'react';

import 'simple-line-icons/css/simple-line-icons.css';
import classNames from 'classnames';
import { makeStyles } from '@material-ui/core';
import { SimpleIconTypes } from './IconTypes';


export interface IBrandingIcon {
    iconType: SimpleIconTypes;
    iconColor?: string;
    iconSize?: number;
}

const useStyles = makeStyles(theme => ({
    icon: {
        marginBottom: "1.2rem"
    }
}))


export const BrandingIcon = ({ iconType, iconColor, iconSize}: IBrandingIcon) => {

    const classes = useStyles();
    const style = {
        color: `${iconColor ?? "blue"}`,
        fontSize: iconSize ?? 24
    }
    return (
        <div
            className={classNames(`icon-${iconType}`, classes.icon)}
            style={style}
        />
    )
}
