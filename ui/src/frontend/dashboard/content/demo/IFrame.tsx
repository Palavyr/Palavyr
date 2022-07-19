import { makeStyles, useTheme } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import PalavyrChatWidget from "palavyr-chat-widget";
import { PreCheckErrorResource } from "@common/types/api/ApiContracts";

const useStyles = makeStyles(theme => ({
    frame: {
        height: "100%",
        width: "100%",
    },
}));

interface IIframe {
    widgetUrl: string;
    apiKey: string;
    iframeRefreshed: boolean;
    preCheckErrors: PreCheckErrorResource[];
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
    const cls = useStyles();

    const url = `${widgetUrl}/widget?key=${apiKey}&demo=${demo}`;
    const theme = useTheme();

    const containerStyles = {
        marginTop: preCheckErrors.length > 0 ? "0rem" : "2rem",
        marginBottom: preCheckErrors.length > 0 ? "0rem" : "2rem",
        height: "660px",
        maxHeight: "80vh",
        width: "420px",
        borderRadius: "9px",
        border: "0px",
        background: "#FFFFFF",
        boxShadow: shadow ? theme.shadows[10] : "none",
        zIndex: 0,
    };

    useEffect(() => {
        if (iframeRefreshed != state) {
            const iframe = document.getElementById(iframeId) as Iframe;
            if (iframe) {
                iframe.src = url;
            }
            setState(iframeRefreshed);
        }
    }, [iframeRefreshed]);

    return <PalavyrChatWidget IframeProps={{ className: cls.frame, id: iframeId }} fixedPosition={false} src={url} containerStyles={containerStyles} />;
};
