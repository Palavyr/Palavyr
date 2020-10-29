import { makeStyles, Typography } from '@material-ui/core';
import React from 'react'

const useStyles = makeStyles((theme) => ({

    sliverDiv: {
        color: "lighgray",
        textAlign: "center",
        height: "30px",
        width: "100%",
        backgroundColor: "gray"
    },
    sliver: {
        fontSize: "16pt"
    },
}));


export const Sliver = () => {

    const classes = useStyles();

    return (
        <div className={classes.sliverDiv}>
            <Typography data-aos="fade-right" className={classes.sliver}>
                Questions? Get in touch: info.palavyr@gmail.com
            </Typography>
        </div>
    )
}