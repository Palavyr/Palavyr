import { useLocation } from "react-router-dom";
import CreateClient, { IClient } from "./client/Client";

export const StatusCheck = async () => {
    var secretKey = new URLSearchParams(useLocation().search).get("key");

    let client: IClient;
    if (secretKey) client = CreateClient(secretKey);

    const { data: statusResult } = await client.Widget.Access.fetchWidgetState();

    return statusResult;
};
