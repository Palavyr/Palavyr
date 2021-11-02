import * as React from 'react';

import { makeStyles } from '@material-ui/core';

export interface IBrandingIcon {
    IconJsx: React.FunctionComponent<React.SVGAttributes<SVGElement>>;
    iconSize: string;
}
type styleProps = {
    iconSize: string;
}

const useStyles = makeStyles(theme => ({
    icon: (props: styleProps) => ({
        textAlign: "center",
        marginBottom: "1.2rem",
        width: props.iconSize
    }),
    wrapper: {
        display: "flex",
        justifyContent: "center"
    }
}))


export const BrandingIcon = ({ IconJsx, iconSize }: IBrandingIcon) => {

    const cls = useStyles({iconSize});

    return (
        <div className={cls.wrapper}>
            <div className={cls.icon}>
                <IconJsx />
            </div>
        </div>
    )
}
