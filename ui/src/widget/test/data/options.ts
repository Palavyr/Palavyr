import { SelectedOption } from "@common/types/widget/widget";

export const getSelectedOption = (IntentId: string) => {
    return {
        intentDisplay: "An intentName",
        IntentId: IntentId,
    };
};

export const options: SelectedOption[] = [
    {
        intentDisplay: "An intentName",
        intentId: "abc123",
    },
];
