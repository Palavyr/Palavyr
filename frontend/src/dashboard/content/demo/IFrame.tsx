import { makeStyles } from "@material-ui/core";
import { IncompleteAreas, PreCheckError } from "@Palavyr-Types";
import React, { useEffect, useState } from "react";

type StyleProps = {
    errors: boolean;
};
const useStyles = makeStyles((theme) => ({
    frame: (props: StyleProps) => ({
        marginTop: props.errors ? "0rem" : "2rem",
        marginBottom: props.errors ? "0rem" : "2rem",
        height: "500px",
        width: "380px",
        borderRadius: "9px",
        border: "0px",
    }),
}));

interface IIframe {
    widgetUrl: string;
    apiKey: string;
    iframeRefreshed: boolean;
    preCheckErrors: PreCheckError[];
}

type Iframe = HTMLElement & {
    src: string;
};

export const IFrame = ({ widgetUrl, apiKey, iframeRefreshed, preCheckErrors }: IIframe) => {
    const [state, setState] = useState<boolean | null>(null);
    const cls = useStyles(preCheckErrors.length > 0);

    const url = `${widgetUrl}/widget?key=${apiKey}&demo=true`;

    useEffect(() => {
        if (iframeRefreshed != state) {
            setState(iframeRefreshed);
            (document.getElementById("chatDemoIframe")! as Iframe).src = url;
        }
    }, [iframeRefreshed]);

    return <iframe id="chatDemoIframe" title="demo" className={cls.frame} src={url} style={{ background: "#FFFFFF" }}></iframe>;
};
