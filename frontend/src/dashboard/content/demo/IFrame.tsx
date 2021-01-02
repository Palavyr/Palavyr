import { makeStyles } from '@material-ui/core'
import React, { useEffect, useState } from 'react'
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

type Iframe = HTMLElement & {
    src: string;
}

export const IFrame = ({ widgetUrl, apiKey, iframeRefreshed, incompleteAreas }: IIframe) => {

    const [state, setState] = useState<boolean | null>(null);
    const classes = useStyles(incompleteAreas.length > 0);

    const url = `${widgetUrl}/widget?key=${apiKey}&demo=true`
    // console.log(`URL from IFRAME: ${url}`);

    useEffect(() => {
        if (iframeRefreshed != state) {
            setState(iframeRefreshed);
            (document.getElementById("chatDemoIframe")! as Iframe).src = url;
        }
    }, [iframeRefreshed])

    return <iframe id="chatDemoIframe" title="demo" className={classes.frame} src={url} style={{background: "#FFFFFF"}}></iframe>

}
