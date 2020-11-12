import { useQuery } from "react-query";
import { Areas } from "@Palavyr-Types";
import { ApiClient } from "@api-client/Client";

const client = new ApiClient();

export const useAreas = () => {
    return useQuery<Areas>("areas", async () => {
        const { data } = await client.Area.GetAreasReactQuery();
        return data;
    });
}