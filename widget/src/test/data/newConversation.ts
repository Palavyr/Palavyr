import { NewConversation } from "@Palavyr-Types";
import { convoA } from "./conversationNodes";
import { widgetPreferences } from "./widgetPreferences";

export const newConversation = (areaId: string): NewConversation => {
    return {
        conversationId: "test-123-newId",
        widgetPreferences: widgetPreferences,
        conversationNodes: convoA(areaId),
    };
};
