import { Grid, Typography } from "@material-ui/core";
import { makeStyles } from "@material-ui/core";
import React, { useState } from "react";
import paulface from "./Paul_team.png";

const useStyles = makeStyles((theme) => ({
    image: {
        height: "300px",
        width: "300px",
        borderRadius: "25%",
        marginBottom: "1rem",
        transition: theme.transitions.create("box-shadow", { duration: theme.transitions.duration.complex }),
        "&:hover": {
            cursor: "pointer",
            boxShadow: theme.shadows[10],
            transition: theme.transitions.create("box-shadow", { duration: theme.transitions.duration.complex }),
        },
    },
    gridContainer: {
        marginTop: "8rem",
        marginBottom: "8rem",
    },
    gridItem: {},
}));

export const OurTeamContent = () => {
    const [loading, setLoading] = useState<boolean>(false);
    const onImageClick = () => {
        window.open("https://www.linkedin.com/in/paul-gradie-phd-743b8b58/", "_blank");
    };

    const cls = useStyles();
    return (
        <Grid container direction="row" alignContent="center" justify="center" className={cls.gridContainer}>
            <Grid item className={cls.gridItem}>
                <img
                    onClick={onImageClick}
                    className={cls.image}
                    key={Date.now()}
                    src={paulface}
                    alt="Paul"
                    onChange={() => setLoading(true)}
                    onLoadStart={() => setLoading(true)}
                    onLoad={() => setLoading(false)}
                />
                <Typography align="center" variant="h4">
                    Paul Gradie
                </Typography>
                <Typography display="block" align="center" variant="h6">
                    Software Engineer
                </Typography>
                <Typography display="block" align="center" variant="h6">
                    Founder
                </Typography>
            </Grid>
        </Grid>
    );
};
