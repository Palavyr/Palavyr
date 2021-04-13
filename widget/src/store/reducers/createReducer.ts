import { AllActions } from "@Palavyr-Types";

export const createReducer = <T, A extends AllActions>(reducer: { [key: string]: Function }, state: T, action: A) => {
    return reducer[action.type] ? reducer[action.type](state, action) : state;
};
