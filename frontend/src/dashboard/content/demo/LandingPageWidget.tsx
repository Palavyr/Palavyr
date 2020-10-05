import React, { useEffect } from 'react'
import { widgetUrl, widgetApiKey } from '@api-client/clientUtils';
import HighlightOffIcon from '@material-ui/icons/HighlightOff';
import SpeedDial from '@material-ui/lab/SpeedDial';
import SpeedDialIcon from '@material-ui/lab/SpeedDialIcon';
import { createStore } from "redux";
import { Provider } from "react-redux";
import { StoreRounded } from '@material-ui/icons';
import { makeStyles } from '@material-ui/core';
import { useState } from 'react';


const useStyles = makeStyles(theme => ({
    frame: {
        position: "static",
        width: "100%",
        height: "100%",
        borderRadius: "9px",
        border: "0px",
        zIndex: 9999
    },
    circle: {
        position: "relative",
        top: "0px",
        left: "0px",

    },
    closedCircle: {
        position: "absolute",
        bottom: "10px",
        right: "50px",
        height: "100px",
        widgth: "100px"
    },
    frameWrapper: {
        position: "absolute",
        height: "500px",
        width: "320px",
        bottom: "80px",
        right: "40px",
        borderColor: ""

    }
}));

const chatStatus = (state = true, action) => {
    console.log("Updateding State")
    switch (action.type) {
        case "SetVisible":
            console.log("Setting VISIBLE")
            return true;
        case "SetInvisible":
            console.log("Setting INVISIBLE")
            return false;
        default:
            console.log("Setting default state: " + state)
            return state;
    }
}

const store = createStore(chatStatus);


export const LandingPageWidget = () => {
    const classes = useStyles();
    const [visible, setVisible] = useState(false);
    const [mounted, setMounted] = useState(true);
    const [hidden, setHidden] = useState(false);

    console.log("COMPONENT: NOT YET MOUNTED...")
    useEffect(() => {

        setMounted(true)
        console.log("USE EFFECT: MOUNTED")
        if (mounted) {
            console.log("MOUNTED: " + store.getState())
            setHidden(false);
        }
        else {
            setHidden(true);
        }
        return () => {
            console.log("TRYING TO DISMOUNT.")
            setMounted(false)
            setHidden(true);
            console.log("FROM DISMOUNT: " + store.getState())
        }
    }, [])



    return (
        <div className={classes.frameWrapper}>
            {
                visible &&
                <HighlightOffIcon
                    fontSize={"large"}
                    className={classes.circle}
                    onClick={() => setVisible(false)}
                />
            }
            {
                visible
                    ? <iframe className={classes.frame} title="demo" src={`${widgetUrl}/widget/${widgetApiKey}`}></iframe>
                    : !visible && !hidden && <SpeedDial
                        ariaLabel="SpeedDial example"
                        className={classes.closedCircle}
                        onClick={() => setVisible(true)}
                        icon={<SpeedDialIcon />}
                        open={true}
                        direction={"up"}
                    />
            }
        </div>
    )
}



