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
        marginTop: "1.2rem",
        marginBottom: "1.2rem",
        textAlign: "center"
    }
}));

export type ThreeItemRowObject = {
    text: string;
    type: SimpleIconTypes;
    title: string;
}

export interface IThreeItemRow {
    listOfThree: Array<ThreeItemRowObject>;
}

export const ThreeItemRow = ({ listOfThree }: IThreeItemRow) => {

    const classes = useStyles();

    return (
        <Grid container className={classes.container}>
            {
                listOfThree.map((x: ThreeItemRowObject, idx: number) => {
                    return (
                        <Grid item key={idx} xs={4} className={classNames(classes.colStyle, "align-center")}>
                            <IconBox iconType={x.type} iconTitle={x.title} iconSize={"large"}>
                                {x.text}
                            </IconBox>
                        </Grid>

                    )
                })
            }
        </Grid>
    )
}