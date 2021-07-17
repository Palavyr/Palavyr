import { makeStyles } from "@material-ui/core";
import { PreCheckError } from "@Palavyr-Types";
import React, { useEffect, useState } from "react";

type StyleProps = {
    errors: boolean;
    shadow: boolean;
};
const useStyles = makeStyles((theme) => ({
    frame: (props: StyleProps) => ({
        marginTop: props.errors ? "0rem" : "2rem",
        marginBottom: props.errors ? "0rem" : "2rem",
        height: "560px",
        width: "420px",
        borderRadius: "9px",
        border: "0px",
        background: "#FFFFFF",
        boxShadow: props.shadow ? theme.shadows[10] : "none",
    }),
}));

interface IIframe {
    widgetUrl: string;
    apiKey: string;
    iframeRefreshed: boolean;
    preCheckErrors: PreCheckError[];
    demo?: boolean;
    shadow?: boolean;
}

type Iframe = HTMLElement & {
    src: string;
};

export const IFrame = ({ widgetUrl, apiKey, iframeRefreshed, preCheckErrors, demo = true, shadow = false }: IIframe) => {
    const [state, setState] = useState<boolean | null>(null);
    const cls = useStyles({ errors: preCheckErrors.length > 0, shadow });

    const url = `${widgetUrl}/widget?key=${apiKey}&demo=${demo}`;

    useEffect(() => {
        if (iframeRefreshed != state) {
            setState(iframeRefreshed);
            (document.getElementById("chatDemoIframe")! as Iframe).src = url;
        }
    }, [iframeRefreshed]);

    return <iframe id="chatDemoIframe" title={demo ? "demo" : "widget"} className={cls.frame} src={url}></iframe>;
};
