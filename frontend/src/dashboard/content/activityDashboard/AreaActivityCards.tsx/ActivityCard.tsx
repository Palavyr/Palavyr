import { makeStyles } from "@material-ui/core";
import { PalavyrCard } from "material/surface/PalavyrCard";
import React from "react";

import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';


const useStyles = makeStyles({
  root: {
    maxWidth: 345,
    margin: "1rem",
    marginLeft: "2rem",
    marginRight: "2rem",
  },
  paper: {},
});

export interface ActivityCardProps {
    areaName: string
}

export const ActivityCard = ({ areaName }: ActivityCardProps) => {
    const classes = useStyles();

    return (
        <PalavyrCard className={classes.root}>
            <CardActionArea>
                <CardMedia component="img" alt="Contemplative Reptile" height="140" image="https://picsum.photos/200/300" title="Contemplative Reptile" />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="h2">
                        {areaName}
                    </Typography>

                </CardContent>
            </CardActionArea>
            <CardActions>
                <Button size="small" color="primary">
                    Share
                </Button>
                <Button size="small" color="primary">
                    Learn More
                </Button>
            </CardActions>
        </PalavyrCard>
    );
};
