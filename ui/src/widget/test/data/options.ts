import { SelectedOption } from "@Palavyr-Types";

export const getSelectedOption = (areaId: string) => {
    return {
        areaDisplay: "An Area Name",
        areaId: areaId,
    };
};

export const options: SelectedOption[] = [
    {
        IntentDisplay: "An Area Name",
        IntentId: "abc123",
    },
];
