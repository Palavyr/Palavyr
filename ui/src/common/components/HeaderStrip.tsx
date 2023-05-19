import { Divider, makeStyles, Typography } from "@material-ui/core";
import React from "react";

interface HeaderStripProps {
    title: string | React.ReactNode;
    subtitle?: string | React.ReactNode;
    divider?: boolean;
    light?: boolean;
    gutterBottom?: boolean;
}
type StyleProps = {
    light: boolean;
};

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    container: (props: StyleProps) => ({
        backgroundColor: props.light ? theme.palette.common.white : theme.palette.background.default,
        paddingTop: "1.2rem",
        // borderRadius: "10px"
    }),
    subtitle: {
        padding: "1rem 3rem 0rem 3rem",
    },
    gutter: {
        height: "1.5rem",
    },
}));

export const HeaderStrip = ({ title, subtitle, divider = false, light = false, gutterBottom = false }: HeaderStripProps) => {
    const cls = useStyles({ light });
    return (
        <>
            <div className={cls.container}>
                <Typography gutterBottom align="center" variant="h4">
                    {title}
                </Typography>
                {subtitle && (
                    <Typography paragraph gutterBottom className={cls.subtitle} align="center">
                        {subtitle}
                    </Typography>
                )}
            </div>
            {divider && <Divider />}
            {gutterBottom && <div className={cls.gutter}></div>}
        </>
    );
};
