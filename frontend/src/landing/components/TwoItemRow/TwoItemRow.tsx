import { makeStyles, Grid, Typography } from "@material-ui/core";
import React from "react";
import classNames from "classnames";
import { IconBox } from "../IconBox";

const useStyles = makeStyles((theme) => ({
    colStyle: {
        alignContent: "center",
        marginLeft: "3rem",
        marginRight: "3rem",
    },
    container: {
        justifyContent: "center",
        paddingTop: "5rem",
        paddingBottom: "5rem",
        textAlign: "center",
    },
    iconcolor: {
        color: "white",
    },
    text: {
        fontSize: "16pt",
    },
}));

export type ItemRowObject = {
    text: string;
    type: React.FunctionComponent<React.SVGAttributes<SVGElement>>;
    title: string;
    size?: number;
    height?: number;
    width?: number;
};

export interface IItemRow {
    dataList: Array<ItemRowObject>;
}

const calculateFade = (arrayLength: number, idx: number) => {
    if (idx === 0) {
        return "fade-right";
    } else if (idx === arrayLength - 1) {
        return "fade-left";
    } else {
        return "fade-in";
    }
};

export const TwoItemRow = ({ dataList }: IItemRow) => {
    const classes = useStyles();
    const width = 12 / dataList.length;

    return (
        <Grid container className={classes.container}>
            {dataList.map((x: ItemRowObject, idx: number) => {
                return (
                    <Grid
                        data-aos={calculateFade(dataList.length, idx)}
                        // data-aos-offset={(100 * idx).toString()}
                        item
                        key={idx}
                        xs={width === 6 ? 4 : 6}
                        className={classNames(classes.colStyle, "align-center")}
                    >
                        <IconBox IconJsx={x.type} iconTitle={x.title} iconSize={x.size || 40}>
                            <Typography variant="body2" className={classes.text}>
                                {x.text}
                            </Typography>
                        </IconBox>
                    </Grid>
                );
            })}
        </Grid>
    );
};
