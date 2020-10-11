import { makeStyles, Grid } from "@material-ui/core";
import { SimpleIconTypes } from "@common/icons/IconTypes";
import React from "react";
import classNames from "classnames";
import { IconBox } from "../IconBox";


const useStyles = makeStyles(theme => ({
    colStyle: {
        alignContent: "center",
    },
    container: {
        justifyContent: "center",
        paddingTop: "5rem",
        paddingBottom: "5rem",
        textAlign: "center"
    },
    iconcolor: {
        color: "navy"
    }
}));

export type TwoItemRowObject = {
    text: string;
    type: SimpleIconTypes;
    title: string;
}

export interface ITwoItemRow {
    listOfTwo: Array<TwoItemRowObject>;
}

export const TwoItemRow = ({ listOfTwo }: ITwoItemRow) => {

    const classes = useStyles();

    return (
        <Grid container className={classes.container}>
            {
                listOfTwo.map((x: TwoItemRowObject, idx: number) => {
                    return (
                        <Grid item key={idx} xs={4} className={classNames(classes.colStyle, "align-center")}>
                            <IconBox iconType={x.type} iconTitle={x.title} iconSize={"xlarge"} iconColor={classes.iconcolor}>
                                {x.text}
                            </IconBox>
                        </Grid>

                    )
                })
            }
        </Grid>
    )
}