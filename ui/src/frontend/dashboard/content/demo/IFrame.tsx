import { makeStyles } from "@material-ui/core";
import { PreCheckError } from "@Palavyr-Types";
import React, { useEffect, useState } from "react";
import PalavyrChatWidget from "palavyr-chat-widget";

type StyleProps = {
    errors: boolean;
    shadow: boolean;
};
const useStyles = makeStyles(theme => ({
    frame: (props: StyleProps) => ({
        marginTop: props.errors ? "0rem" : "2rem",
        marginBottom: props.errors ? "0rem" : "2rem",
        height: "660px",
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

const iframeId = "chatDemoIframe";

//https://www.thoughtco.com/targeting-links-in-frames-3468670
export const IFrame = ({ widgetUrl, apiKey, iframeRefreshed, preCheckErrors, demo = true, shadow = false }: IIframe) => {
    const [state, setState] = useState<boolean | null>(null);
    const cls = useStyles({ errors: preCheckErrors.length > 0, shadow });

    const url = `${widgetUrl}/widget?key=${apiKey}&demo=${demo}`;

    useEffect(() => {
        if (iframeRefreshed != state) {
            const iframe = document.getElementById(iframeId) as Iframe;
            if (iframe) {
                iframe.src = url;
            }
            setState(iframeRefreshed);
        }
    }, [iframeRefreshed]);

    return <PalavyrChatWidget className={cls.frame}  startOpen={true} fixedPosition={false} src={url} id={iframeId} />;
};
