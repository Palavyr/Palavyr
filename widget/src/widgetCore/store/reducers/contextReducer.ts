import { createReducer } from "src/widgetCore/utils/createReducer";
import { ContextPropertyActions, GET_CONTEXT_PROPERTIES, SET_CONTEXT_PROPERTIES } from "../actions/types";
import { ContextProperties, ContextState } from "../types";

export const defaultContextProperties: ContextProperties = {
    dynamicResponses: [],
    keyValues: [],
    emailAddress: "",
    phoneNumber: "",
    name: "",
    region: "",
};

// const initialState = {
//     contextProperties: defaultContextProperties,
// };

const contextReducer = {
    [GET_CONTEXT_PROPERTIES]: (state: ContextState) => ({ ...state }),
    [SET_CONTEXT_PROPERTIES]: (state: ContextState, updatedContextProperties: ContextProperties) => ({ ...state, contextProperties: updatedContextProperties }),
};

export default (state: ContextProperties = defaultContextProperties, action: ContextPropertyActions) => createReducer<ContextProperties>(contextReducer, state, action);
