import { NewConversation } from "@Palavyr-Types";
import { convoA } from "../../../frontend/dashboard/content/designer/dummy_conversations";
import { widgetPreferences } from "./widgetPreferences";

export const newConversation = (areaId: string): NewConversation => {
    return {
        conversationId: "test-123-newId",
        widgetPreferences: widgetPreferences,
        conversationNodes: convoA(areaId),
    };
};
