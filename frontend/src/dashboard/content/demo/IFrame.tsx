import { makeStyles } from '@material-ui/core'
import React, { useEffect } from 'react'
import { IncompleteAreas } from './ChatDemo';


const useStyles = makeStyles(theme => ({
    frame: props => ({
        marginTop: props ? "0rem" : "2rem",
        marginBottom: props ? "0rem" : "2rem",
        height: "500px",
        width: "380px",
        borderRadius: "9px",
        border: "0px"
    }),

}))

interface IIframe {
    widgetUrl: string;
    apiKey: string;
    iframeRefreshed: boolean;
    incompleteAreas: Array<IncompleteAreas>;
}

export const IFrame = ({ widgetUrl, apiKey, iframeRefreshed, incompleteAreas }: IIframe) => {

    const classes = useStyles(incompleteAreas.length > 0);

    // https://widget.palavyr.com/widget?key={apikey}
    return <iframe id="chatDemoIframe" title="demo" className={classes.frame} src={`${widgetUrl}/widget?key=${apiKey}`}></iframe>

    // return <iframe id="chatDemoIframe" title="demo" className={classes.frame} src={`${widgetUrl}/widget/${apiKey}`}></iframe>

}
