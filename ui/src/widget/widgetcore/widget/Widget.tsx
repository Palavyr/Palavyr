import * as React from "react";
import { makeStyles } from "@material-ui/core";
import { WidgetLayout } from "./WidgetLayout";
import { useLocation } from "react-router-dom";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { getRootNode } from "@widgetcore/BotResponse/utils/utils";
import { renderNextBotMessage } from "@widgetcore/BotResponse/utils/renderNextComponent";
import "@widgetcore/widget/widget.module.scss";
import { IAppContext } from "widget/hook";

export const useWidgetStyles = makeStyles(theme => ({
    pwbox: {
        display: "flex",
        flexFlow: "column",
        height: "100vh",
        overflowY: "hidden",
    },
    pwrow: {},
    pheader: {
        flex: "0 1 auto",
    },
    pcontent: {
        flexFlow: "column",
        flexGrow: 1,
        overflowY: "auto",
        width: "100%",
    },
    pfooter: {
        display: "flex",
        flex: "0 1 30px",
        flexFlow: "row",
        justifyContent: "space-between",
    },
}));

export const Widget = () => {
    const cls = useWidgetStyles();

    const location = useLocation();
    let secretKey = new URLSearchParams(location.search).get("key");
    if (!secretKey) {
        secretKey = "123";
    }
    const client = new PalavyrWidgetRepository(secretKey);

    const initializer = async (context: IAppContext) => {
        const intro = await client.Widget.Get.IntroSequence();
        const rootNode = getRootNode(intro);
        renderNextBotMessage(context, rootNode, intro, client, null);
    };

    return (
        <div className={cls.pwbox}>
            <WidgetLayout initializer={initializer} />
        </div>
    );
};
