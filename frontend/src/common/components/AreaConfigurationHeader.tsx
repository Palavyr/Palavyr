import { Divider, makeStyles, Typography } from "@material-ui/core";
import React from "react";

interface IAreaConfigurationHeader {
    title: string;
    subtitle?: string;
    divider?: boolean;
    light?: boolean;
    gutterBottom?: boolean;
}
type StyleProps = {
    light: boolean;
};

const useStyles = makeStyles((theme) => ({
    container: (props: StyleProps) => ({
        backgroundColor: props.light ? "white" : theme.palette.background.default,
        width: "100%",
        paddingTop: "1.2rem",
    }),
    subtitle: {
        padding: "1rem 3rem 0rem 3rem",
    },
    gutter: {
        height: "1.5rem"
    }
}));

export const AreaConfigurationHeader = ({ title, subtitle, divider = false, light = false, gutterBottom = false }: IAreaConfigurationHeader) => {
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
