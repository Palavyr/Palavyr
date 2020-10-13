import { makeStyles, Grid } from "@material-ui/core";
import React from "react";
import classNames from "classnames";
import { IconBox } from "../IconBox";
import { ItemRowObject } from "../TwoItemRow/TwoItemRow";


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


export interface IItemRow {
    listOfThree: Array<ItemRowObject>;
}

export const ThreeItemRow = ({ listOfThree }: IItemRow) => {

    const classes = useStyles();

    return (
        <Grid container className={classes.container}>
            {
                listOfThree.map((x: ItemRowObject, idx: number) => {
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